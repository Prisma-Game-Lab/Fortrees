using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MenuUI : MonoBehaviour {

        public int LevelToLoad;
        public GameObject FirstButtonSelected;

        public void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(FirstButtonSelected);
        }

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
