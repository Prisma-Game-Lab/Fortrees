using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject PauseUI;
        [Tooltip("Botão selecionado quando o menu é acionado")] public GameObject FirstSelectedButton;
		[Tooltip("Igual o Frames To Wait do NodeController.")] public int FramesToWait = 5; //TODO: juntar com o FramesToWait do NodeController.cs

		private int _framesLeftToWait = 0;
        private float _audioListenerVolume; //audio listener volume before game was paused

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
            EventSystem.current.SetSelectedGameObject(FirstSelectedButton);

            if (PauseUI.activeSelf)
            {
                Time.timeScale = 0f;
                _audioListenerVolume = AudioListener.volume;
                AudioListener.volume = 0;
            }
            else
            {
                Time.timeScale = 1f;
                AudioListener.volume = _audioListenerVolume;
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
