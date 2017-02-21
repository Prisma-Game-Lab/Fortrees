using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject GameOverUi;
        public GameObject CompleteLevelUi;
        public GameObject PressStartUi;
        public static bool GameIsOver;
        [HideInInspector]
        public static bool GameStarted;

        public void Start()
        {
            GameIsOver = false;
            GameStarted = false;
            #if DEBUG
            Debug.Assert(GameObject.Find("WaveManager") != null, "Scene precisa de WaveManager seu mongol");
            #endif

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
        }

        private void EndGame()
        {
            GameIsOver = true;
            GameOverUi.SetActive(true);
        }
    }
}
