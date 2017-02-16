using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public class MovingSeedUI : MonoBehaviour
    {
        public Camera Cam;
        public Transform Target;
        public float Speed = 10.0f;
        public MovingSeedsManager Manager;

        private Vector3 _dir;
        private Vector2 _targetVec2;
        private Vector2 _positionVec2;
        private Transform _centerOfScreen;
        private Image _image;

        // Use this for initialization
        void Start()
        {
            _centerOfScreen = Cam.transform.GetChild(0);
            _targetVec2 = new Vector2(Target.position.x, Target.position.z);
            _positionVec2 = new Vector2(transform.position.x, transform.position.z);
            _dir = (Target.position - transform.position).normalized;
            _image = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(_dir * Speed * Time.deltaTime);
            _positionVec2 = new Vector2(transform.position.x, transform.position.z);

            if (transform.localScale.x < 1)
            {
                transform.localScale *= 1 + (Time.deltaTime * Speed / 50);
                Color color = _image.color;
                color.a = transform.localScale.x;
                _image.color = color;
            }
            
            if ((_targetVec2 - _positionVec2).magnitude <= 1)
            {
                PlayerStats.Seeds++;
                Destroy(gameObject);
            }
        }
    }
}
