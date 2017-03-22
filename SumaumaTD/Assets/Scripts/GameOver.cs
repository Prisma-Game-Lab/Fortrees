using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class GameOver : MonoBehaviour
    {
        public GameObject FirstSelectedButton;
        public int MenuSceneIndex = 0;

        public void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
        }

        public void Retry()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Menu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(MenuSceneIndex);
        }
    }
}
