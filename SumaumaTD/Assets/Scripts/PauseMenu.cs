using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class PauseMenu : MonoBehaviour
    {

        public GameObject PauseUI;
		[Tooltip("Igual o Frames To Wait do NodeController.")] public int FramesToWait = 5; //TODO: juntar com o FramesToWait do NodeController.cs

		private int _framesLeftToWait = 0;

        public void Update()
		{
            if (!GameManager.GameIsOver)
            {
                while (_framesLeftToWait > 0)
                {
                    _framesLeftToWait--;
                    return;
                }

                if (Input.GetButtonDown("PauseButton") || Input.GetKeyDown(KeyCode.P))
                {
                    TogglePause();
                    _framesLeftToWait = FramesToWait;
                }
            }
        }

        public void TogglePause()
        {
            PauseUI.SetActive( !PauseUI.activeSelf);

            if (PauseUI.activeSelf)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }

        public void Retry()
        {
            TogglePause();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Menu()
        {
            TogglePause();
            SceneManager.LoadScene(0);
        }

    }
}
