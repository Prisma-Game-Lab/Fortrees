using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class CompleteLevel : MonoBehaviour
    {
        public int NextLevelSceneIndex;
        public int MenuSceneIndex = 0;

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
