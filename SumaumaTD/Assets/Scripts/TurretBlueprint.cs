using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [System.Serializable]
    public class TurretBlueprint
    {

        public GameObject Prefab;
        public GameObject UpgradedPrefab;
        public Text CostText;
        public int Cost;
        public int UpgradeCost;

        public int GetSellCost()
        {
            return Cost/2;
        }

        public void Start()
        {
            CostText.text =  "x" + Cost + "seeds";
        }

    }
}
