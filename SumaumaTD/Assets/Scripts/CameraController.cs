using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour {

        ///TODO: Clamp side movements
        public float ScrollSpeed = 5f;
        public float PanSpeed = 30f;
        public float PanBorderThickness = 10f;

        [Header("Camera Clamp")]
        public float MinX = -40f;
        public float MaxX = 70f;
        public float MinZ = -80f;
        public float MaxZ = 80f;

        [Header("Zoom")]
        [Tooltip("Quando menor o ortographic size, maior o zoom")] public float MinOrtographicSize = 5f;
        [Tooltip("Quando menor o ortographic size, maior o zoom")] public float MaxOrtographicSize = 45f;

        private Camera cam;

        public void Start()
        {
            //Pega o componente da câmera (necessário para mudar o zoom -> ortographic size)
            cam = this.GetComponent<Camera>();
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

            if (Input.GetKey ("w") || Input.mousePosition.y >= Screen.height - PanBorderThickness) {
                pos.z += PanSpeed * Time.deltaTime;
                pos.z = Mathf.Clamp(pos.z, MinZ, MaxZ);
            }
            if (Input.GetKey ("s") || Input.mousePosition.y <= PanBorderThickness) {
                pos.z -= PanSpeed * Time.deltaTime;
                pos.z = Mathf.Clamp(pos.z, MinZ, MaxZ);
            }
            if (Input.GetKey ("d") || Input.mousePosition.x >= Screen.width - PanBorderThickness) {
                pos.x += PanSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            }
            if (Input.GetKey ("a") || Input.mousePosition.x <= PanBorderThickness) {
                pos.x -= PanSpeed * Time.deltaTime;
                pos.x = Mathf.Clamp(pos.x, MinX, MaxX);
            }
            transform.position = pos;

            float scroll = Input.GetAxis ("Mouse ScrollWheel");
            cam.orthographicSize -= scroll * 1000 * ScrollSpeed * Time.deltaTime;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, MinOrtographicSize, MaxOrtographicSize);
        }
    }
}
