using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class PlantedTrees : MonoBehaviour
    {

        public Text PlantedTreeText;
        public Text PlantedTreeTextOutline;
        
        public void Update ()
        {
            PlantedTreeText.text = PlayerStats.PlantedTrees.ToString();
            PlantedTreeTextOutline.text = PlayerStats.PlantedTrees.ToString();
        }
    }
}
