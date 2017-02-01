using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class MenuUI : MonoBehaviour {

        public int LevelToLoad;

        public void StartGame()
        {
            SceneManager.LoadScene(LevelToLoad);
        }

        public void QuitGame()
        {
           Application.Quit();
        }
    }
}
