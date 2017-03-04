using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject GameOverUi;
        public GameObject CompleteLevelUi;
        public GameObject PressStartUi;
        public GameObject WaveManager;
        public GroupSound GroupSound;
        public static bool GameIsOver;
        [HideInInspector]
        public static bool GameStarted;

        public void Awake()
        {
            GameIsOver = false;
            GameStarted = false;
            PressStartUi.SetActive(true);
            #if DEBUG
            Debug.Assert(WaveManager != null, "Scene precisa de WaveManager seu mongol");
#endif

            if (GroupSound.AudioSources[2] == null) Debug.LogError("Necessário setar audio sources dos inimigos (estão na câmera)");
        }

        public void Update ()
        {
            if(GameIsOver)
                return;

            if (PlayerStats.Lives<=0)
            {
                EndGame();
            }

            if (Input.GetButtonDown("StartGameButton") && !GameStarted)
            {
                PressStartUi.SetActive(false);
                GameStarted = true;
            }
        }
        
        public void WinLevel()
        {
            GameIsOver = true;
            CompleteLevelUi.SetActive(true);
            Time.timeScale = 0f;
        }

        private void EndGame()
        {
            GameIsOver = true;
            GameOverUi.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
