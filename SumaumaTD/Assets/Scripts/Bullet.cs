using UnityEngine;

namespace Assets.Scripts
{
    public class Bullet : MonoBehaviour {

        public float Speed = 70f;
        public float ExplosionRadius = 0f;
        public GameObject ImpactEffect;
        public int Damage = 50;
        private Transform _target;
        private Vector3 _startScale;


        public void Start()
        {
            _startScale = transform.localScale;
            //Time.timeScale = 0.5f;
        }

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


            //alternativa 1
            //float temp = dir.y;
            //var angle = 30;
            //float RadAngle = angle * Mathf.Deg2Rad;
            //float distance = dir.magnitude;
            ////dir.y = distance * Mathf.Tan(RadAngle); //elevation angle
            ////Debug.Log("dir:" + dir);

            //distance += temp / Mathf.Tan(RadAngle); // Correction for small height differences

            //float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * RadAngle));
            ////Debug.Log("velocity:"+ velocity);

            //var vector = velocity * dir.normalized;
           
            //alternativa2
            //y = ((tan ângulo) * x) - (g*x²/(2*(v0*cos(ângulo))²)
            //var y = (Mathf.Tan(RadAngle) * dir.x) - (Physics.gravity.magnitude * Mathf.Pow(dir.x,2)/ (2*Mathf.Pow(20*distanceThisFrame * Mathf.Cos(RadAngle),2) ));
            //dir.y = y;
            //var newScale = _startScale*Mathf.Sin( distanceThisFrame);

            //transform.localScale = newScale;


            transform.Translate (dir.normalized*distanceThisFrame, Space.World);
            transform.LookAt(_target);
        }

       private Vector3 CalculateVelocityVector(Transform source, Transform target, float angle)
        {
            Vector3 direction = target.position - source.position;            // get target direction
            float h = direction.y;                                            // get height difference
            direction.y = 0;                                                // remove height
            float distance = direction.magnitude;                            // get horizontal distance
            float a = angle * Mathf.Deg2Rad;                                // Convert angle to radians
            direction.y = distance * Mathf.Tan(a);                            // Set direction to elevation angle
            distance += h / Mathf.Tan(a);                                        // Correction for small height differences

            // calculate velocity
            float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
            return velocity * direction.normalized;
            
        }

        private Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
        {
            // calculate vectors
            Vector3 distance = target - origin;
            Vector3 distanceXZ = distance;
            distanceXZ.y = 0;

            // calculate xz and y
            float y = distance.y;
            float xz = distanceXZ.magnitude;

            // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
            // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
            // so xz = v0xz * t => v0xz = xz / t
            // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
            float t = timeToTarget;
            float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
            float v0xz = xz / t;

            // create result vector for calculated starting speeds
            Vector3 result = distanceXZ.normalized;        // get direction of xz but with magnitude 1
            result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
            result.y = v0y;                                // set y to v0y (starting speed of y plane)

            return result;
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
