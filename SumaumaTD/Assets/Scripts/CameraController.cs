using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {

        public bool IsFixed;

        ///TODO: Clamp side movements
        public float MouseScrollSpeed = 5f;
        public float ControllerScrollSpeed = 0.05f;
        public float PanSpeed = 30f;
        public float PanBorderThickness = 10f;

        [Header("Camera Clamp")]
        public GameObject CameraClamp;
        
        [Tooltip("Quando menor o ortographic size, maior o zoom")] public float MinOrtographicSize = 5f;
        [Tooltip("Quando menor o ortographic size, maior o zoom")] public float MaxOrtographicSize = 45f;

        [Header("Controller")]
        [Tooltip("Valor mínimo que um eixo do controle precisa ir para uma direção para detectar movimento")] public float minimumAxisToMove = 0.5f;

        [Header("Camera Shake")]
        public bool CanShake;
        public float ShakeAmount;
        public float ShakeTime;
        public float DecreaseFactor = 1.0f;

        private Camera cam;
		//private float minX, minZ, maxX, maxZ;
        private float _shakeTimeRemaining;
        private Transform _centerOfScreen;
        private Collider _cameraClampCollider;

        public void Start()
        {
            //Pega o componente da câmera (necessário para mudar o zoom -> ortographic size)
            cam = this.GetComponent<Camera>();

            _centerOfScreen = transform.GetChild(0);
            
            _cameraClampCollider = CameraClamp.GetComponent<Collider>();
        }

        // Update is called once per frame
        public void Update ()
        {

            if (GameManager.GameIsOver)
            {
                this.enabled = false;
                return;
            }

            if(IsFixed)
                return;

            Clamp();
            //UpdateClamp ();

            CamShake();
        }

        public void CamShake()
        {
            if (CanShake)
            {
                /* TENTATIVA 1: IREZUMI WAY
                 * float randomValue = Random.Range(-Mathf.PI, Mathf.PI);
                float newX = Mathf.SmoothDamp(transform.position.x, transform.position.x + Mathf.Cos(randomValue) * ShakeAmountX, ref ShakeSpeedX, ShakeTime);
                float newZ = Mathf.SmoothDamp(transform.position.z, transform.position.z + Mathf.Sin(randomValue) * ShakeAmountZ, ref ShakeSpeedZ, ShakeTime);
                transform.position = new Vector3(newX, transform.position.y, newZ);*/
                /* TENTATIVA 2: GOOGLE WAY (PT 1)
                 * float newX = 0, newZ = 0;
                if (ShakeAmountX > 0) newX = Mathf.Repeat(ShakeTime, ShakeAmountX);
                if (ShakeAmountZ > 0) newZ = Mathf.Repeat(ShakeTime, ShakeAmountZ);
                transform.position = new Vector3(transform.position.x + newX - ShakeAmountX/2, transform.position.y, transform.position.z + newZ - ShakeAmountZ/2);*/
                /* TENTATIVA 3: GOOGLE WAY (PT 2)
                 * if (ShakeTime > 0)
                {
                    transform.Translate(new Vector3(Random.Range(-ShakeAmountX, ShakeAmountX), 0, Random.Range(-ShakeAmountZ, ShakeAmountZ)));
                    ShakeTime -= Time.deltaTime;
                }
                else ShakeTime = 0.0f;
                */
                //Método atual: https://forum.unity3d.com/threads/screen-shake-effect.22886/#post-153233
                if (_shakeTimeRemaining > 0)
                {
                    transform.Translate(Random.insideUnitSphere * ShakeAmount);
                    _shakeTimeRemaining -= Time.deltaTime * DecreaseFactor;

                }
                else
                {
                    CanShake = false;
                    _shakeTimeRemaining = ShakeTime;
                }
            }
        }

        /*
		private void UpdateClamp(){
			float lerpAmount = 1 - (cam.orthographicSize - MinOrtographicSize) / (MaxOrtographicSize - MinOrtographicSize);
			minX = Mathf.Lerp (MinXZoomMin, MinXZoomMax, lerpAmount); 
			maxX = Mathf.Lerp (MaxXZoomMin, MaxXZoomMax, lerpAmount); 
			minZ = Mathf.Lerp (MinZZoomMin, MinZZoomMax, lerpAmount); 
			maxZ = Mathf.Lerp (MaxZZoomMin, MaxZZoomMax, lerpAmount); 
		} */
        
        private void Clamp()
        {
            //Clamp antes do movimento
            Vector3 pos = transform.position;

            /*
            pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            */
            
            //Movimento de câmera
            if (Input.GetAxis("CameraVertical") > minimumAxisToMove || Input.GetKey("w") || Input.mousePosition.y >= Screen.height - PanBorderThickness)
            {
                pos.z += PanSpeed * Time.deltaTime;
                //pos.z = Mathf.Clamp(pos.z, MinZ, MaxZ);
            }
            if (Input.GetAxis("CameraVertical") < -minimumAxisToMove || Input.GetKey("s") || Input.mousePosition.y <= PanBorderThickness)
            {
                pos.z -= PanSpeed * Time.deltaTime;
                //pos.z = Mathf.Clamp(pos.z, MinZ, MaxZ);
            }
            if (Input.GetAxis("CameraHorizontal") > minimumAxisToMove || Input.GetKey("d") || Input.mousePosition.x >= Screen.width - PanBorderThickness)
            {
                pos.x += PanSpeed * Time.deltaTime;
                //pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            }
            if (Input.GetAxis("CameraHorizontal") < -minimumAxisToMove || Input.GetKey("a") || Input.mousePosition.x <= PanBorderThickness)
            {
                pos.x -= PanSpeed * Time.deltaTime;
                //pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            }


            //Clamp depois do movimento
            transform.position = pos;
            if (!_cameraClampCollider.bounds.Contains(_centerOfScreen.transform.position)) FixOutOfBounds();

            //Zoom
            float controllerScroll = Input.GetAxis("CameraZoom");
            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

            float scroll = Mathf.Abs(controllerScroll) > Mathf.Abs(mouseScroll) ? controllerScroll : mouseScroll; //o maior valor entre controllerScroll e mouseScroll vira o valor de scroll
            float scrollSpeed = (scroll == mouseScroll) ? MouseScrollSpeed : ControllerScrollSpeed;

            cam.orthographicSize -= scroll * 1000 * scrollSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, MinOrtographicSize, MaxOrtographicSize); 

            
        }
        
        private void FixOutOfBounds()
        {
            Vector3 nearestBound = _cameraClampCollider.ClosestPointOnBounds(_centerOfScreen.transform.position);
            transform.Translate(nearestBound - _centerOfScreen.transform.position);
        }
    }
}
