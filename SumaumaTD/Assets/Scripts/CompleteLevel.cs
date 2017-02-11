using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class CompleteLevel : MonoBehaviour
    {
        public GameObject FirstSelectedButton;
        public int NextLevelSceneIndex;
        public int MenuSceneIndex = 0;

        public void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(FirstSelectedButton);
        }

        public void Continue()
        {
            SceneManager.LoadScene(NextLevelSceneIndex);
        }

        public void Menu()
        {
            SceneManager.LoadScene(MenuSceneIndex);
        }
    }
}
