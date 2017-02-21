using UnityEngine;
using UnityEngine.UI;
using UnityEditor.Animations;

namespace Assets.Scripts
{
    [System.Serializable]
    public class TurretBlueprint
    {
        public AnimatorController TurretAnimator;
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
