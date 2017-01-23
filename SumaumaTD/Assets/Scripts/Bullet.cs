using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour {

        public float Speed = 70f;
        public float ExplosionRadius = 0f;
        public GameObject ImpactEffect;
        public int Damage = 50;
        private Transform _target;

        public void Seek(Transform target){
            this._target = target;
        }

        // Update is called once per frame
        public void Update () {
            if (_target == null) {
                Destroy (gameObject);
                return;
            }

            Vector3 dir = _target.position - transform.position;
            float distanceThisFrame = Speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame) {
                HitTarget ();
                return;
            }

            transform.Translate (dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(_target);
        }

        //instantiate effect and explode/hit enemy and destroy bullet
        public void HitTarget() {
            GameObject effectIns = (GameObject) Instantiate (ImpactEffect, transform.position, transform.rotation);
            Destroy (effectIns, 5f);

            if (ExplosionRadius > 0f)
            {
                Explode();
            }
            else
            {
                DamageEnemy(_target);
            }
            
            Destroy (gameObject);
        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);//pega todos os colliders dentro de um raio (de uma esfera com centro em position)
            foreach (var collid in colliders)
            {
                if (collid.tag == "Enemy")
                {
                    DamageEnemy(collid.transform);
                }
            }
        }

        //calls enemy function to handle damage
        private void DamageEnemy(Transform enemy)
        {
            Enemy e = enemy.GetComponent<Enemy>();

            if (e != null)
            {
                e.TakeDamage(Damage);
            }

        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}
