using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
    public class MovingSeedUI : MonoBehaviour
    {
        public Camera Cam;
        public Transform Target;
        public float Speed;
        public MovingSeedsManager Manager;
		public float MaxSize;

        private Vector3 _dir;
        private Vector2 _targetVec2;
        private Vector2 _positionVec2;
		private float _currVelocity = 0f;
		private Vector3 _currVelocityVec3;
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
			//transform.position = Vector3.SmoothDamp(transform.position, Target.position, ref _currVelocityVec3, 0.1f, 30, Time.deltaTime);
			Speed = Mathf.SmoothDamp(Speed, 10f, ref _currVelocity, 0.7f);
            transform.Translate(_dir * Speed * Time.deltaTime);
            _positionVec2 = new Vector2(transform.position.x, transform.position.z);

            if (transform.localScale.x < MaxSize)
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

			AlphaChange ();
        }

		void AlphaChange()
		{
			float distance = (_targetVec2 - _positionVec2).magnitude;
			if (distance <= 10) {
				Color color = _image.color;
				color.a = 1 - (10 - distance)/10;
				_image.color = color;
			}
		}
	}
}
