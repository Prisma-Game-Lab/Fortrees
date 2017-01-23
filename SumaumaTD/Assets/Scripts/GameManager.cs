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
