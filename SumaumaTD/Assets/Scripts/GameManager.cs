using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public GameObject GameOverUi;
        public static bool GameIsOver;

        public void Start()
        {
            GameIsOver = false;

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
        }

        private void EndGame()
        {
            GameIsOver = true;
            GameOverUi.SetActive(true);
        }
    }
}
