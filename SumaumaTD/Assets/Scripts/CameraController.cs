using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour {

        ///TODO: Clamp side movements
        public float MinY = 10f;
        public float MaxY = 80f;
        public float ScrollSpeed = 5f;
        public float PanSpeed = 30f;
        public float PanBorderThickness = 10f;
        
        // Update is called once per frame
        public void Update ()
        {

            if (GameManager.GameIsOver)
            {
                this.enabled = false;
                return;
            }

            if (Input.GetKey ("w") || Input.mousePosition.y >= Screen.height - PanBorderThickness) {
                transform.Translate (Vector3.forward * PanSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey ("s") || Input.mousePosition.y <= PanBorderThickness) {
                transform.Translate (Vector3.back * PanSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey ("d") || Input.mousePosition.x >= Screen.width - PanBorderThickness) {
                transform.Translate (Vector3.right * PanSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey ("a") || Input.mousePosition.x <= PanBorderThickness) {
                transform.Translate (Vector3.left * PanSpeed * Time.deltaTime, Space.World);
            }

            float scroll = Input.GetAxis ("Mouse ScrollWheel");
            Vector3 pos = transform.position;
            pos.y -= scroll * 1000 * ScrollSpeed * Time.deltaTime;
            pos.y = Mathf.Clamp (pos.y, MinY, MaxY);
            transform.position = pos;
        }
    }
}
