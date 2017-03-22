using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MenuUI : MonoBehaviour {

        public int LevelToLoad;
        public GameObject FirstButtonSelected;
        public GameObject ReturnButton;
        public GameObject Credits;

        public void OnEnable()
        {
            SetCurrentElement(FirstButtonSelected);
            Credits.SetActive(false);
            //EventSystem.current.SetSelectedGameObject(FirstButtonSelected);
        }

        public void StartGame()
        {
            PlayerStats.PlantedTrees = 0;
            SceneManager.LoadScene(LevelToLoad);
        }

        public void Return()
        {
            Credits.SetActive(false);
            SetCurrentElement(FirstButtonSelected);
        }

        public void LoadCredits()
        {
            Credits.SetActive(true);
            SetCurrentElement(ReturnButton);
        }

        public void QuitGame()
        {
           Application.Quit();
        }

        private void SetCurrentElement(GameObject selected)
        {
            EventSystem.current.SetSelectedGameObject(selected);
        }

    }
}
