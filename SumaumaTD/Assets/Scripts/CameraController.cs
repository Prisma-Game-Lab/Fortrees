using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour {

        ///TODO: Clamp side movements
        public float MouseScrollSpeed = 5f;
        public float ControllerScrollSpeed = 0.05f;
        public float PanSpeed = 30f;
        public float PanBorderThickness = 10f;

        [Header("Camera Clamp")]
		[Tooltip("Posição mínima em X da câmera quando o zoom está no mínimo")] public float MinXZoomMin = -40f;
		[Tooltip("Posição máxima em X da câmera quando o zoom está no mínimo")] public float MaxXZoomMin = 70f;
		[Tooltip("Posição mínima em Z da câmera quando o zoom está no mínimo")] public float MinZZoomMin = -80f;
		[Tooltip("Posição máxima em Z da câmera quando o zoom está no mínimo")] public float MaxZZoomMin = 80f;

		[Tooltip("Posição mínima em X da câmera quando o zoom está no máximo")] public float MinXZoomMax = -25f;
		[Tooltip("Posição máxima em X da câmera quando o zoom está no máximo")] public float MaxXZoomMax = 55f;
		[Tooltip("Posição mínima em Z da câmera quando o zoom está no máximo")] public float MinZZoomMax = -40f;
		[Tooltip("Posição máxima em Z da câmera quando o zoom está no máximo")] public float MaxZZoomMax = 40f;

        [Header("Zoom")]
        [Tooltip("Quando menor o ortographic size, maior o zoom")] public float MinOrtographicSize = 5f;
        [Tooltip("Quando menor o ortographic size, maior o zoom")] public float MaxOrtographicSize = 45f;

        [Header("Controller")]
        [Tooltip("Valor mínimo que um eixo do controle precisa ir para uma direção para detectar movimento")] public float minimumAxisToMove = 0.5f;

        private Camera cam;
		private float minX, minZ, maxX, maxZ;

        public void Start()
        {
            //Pega o componente da câmera (necessário para mudar o zoom -> ortographic size)
            cam = this.GetComponent<Camera>();

			UpdateClamp ();
        }

        // Update is called once per frame
        public void Update ()
        {

            if (GameManager.GameIsOver)
            {
                this.enabled = false;
                return;
            }

            Vector3 pos = transform.position;
			pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
			pos.x = Mathf.Clamp(pos.x, minX, maxX);

            if (Input.GetAxis("CameraVertical") > minimumAxisToMove || Input.GetKey ("w") || Input.mousePosition.y >= Screen.height - PanBorderThickness) {
                pos.z += PanSpeed * Time.deltaTime;
                //pos.z = Mathf.Clamp(pos.z, MinZ, MaxZ);
            }
            if (Input.GetAxis("CameraVertical") < -minimumAxisToMove || Input.GetKey ("s") || Input.mousePosition.y <= PanBorderThickness) {
                pos.z -= PanSpeed * Time.deltaTime;
                //pos.z = Mathf.Clamp(pos.z, MinZ, MaxZ);
            }
            if (Input.GetAxis("CameraHorizontal") > minimumAxisToMove || Input.GetKey ("d") || Input.mousePosition.x >= Screen.width - PanBorderThickness) {
                pos.x += PanSpeed * Time.deltaTime;
                //pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            }
            if (Input.GetAxis("CameraHorizontal") < -minimumAxisToMove || Input.GetKey ("a") || Input.mousePosition.x <= PanBorderThickness) {
                pos.x -= PanSpeed * Time.deltaTime;
                //pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            }
            transform.position = pos;

            float controllerScroll = Input.GetAxis("CameraZoom");
            float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");
       
            float scroll = Mathf.Abs(controllerScroll) > Mathf.Abs(mouseScroll) ? controllerScroll : mouseScroll; //o maior valor entre controllerScroll e mouseScroll vira o valor de scroll
            float scrollSpeed = (scroll == mouseScroll) ? MouseScrollSpeed : ControllerScrollSpeed;

            cam.orthographicSize -= scroll * 1000 * scrollSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, MinOrtographicSize, MaxOrtographicSize);

			UpdateClamp ();
        }

		private void UpdateClamp(){
			float lerpAmount = 1 - (cam.orthographicSize - MinOrtographicSize) / (MaxOrtographicSize - MinOrtographicSize);
			minX = Mathf.Lerp (MinXZoomMin, MinXZoomMax, lerpAmount); 
			maxX = Mathf.Lerp (MaxXZoomMin, MaxXZoomMax, lerpAmount); 
			minZ = Mathf.Lerp (MinZZoomMin, MinZZoomMax, lerpAmount); 
			maxZ = Mathf.Lerp (MaxZZoomMin, MaxZZoomMax, lerpAmount); 
		}
    }
}
