using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameOver : MonoBehaviour
    {

        public Text WavesSurvivedText;

        public void OnEnable()
        {
            WavesSurvivedText.text = PlayerStats.Waves.ToString();
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Menu()
        {
            
        }
    }
}
