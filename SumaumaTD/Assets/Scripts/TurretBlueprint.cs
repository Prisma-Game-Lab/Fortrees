using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    [System.Serializable]
    public class TurretBlueprint
    {
        public RuntimeAnimatorController TurretAnimator;
        public GameObject Prefab;
        public GameObject UpgradedPrefab;
        public int Cost;
        public int UpgradeCost;

        public int GetSellCost()
        {
            return Cost/2;
        }

    }
}
