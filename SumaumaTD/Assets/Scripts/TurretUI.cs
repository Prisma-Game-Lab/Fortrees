using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TurretUI : MonoBehaviour
    {
        public GameObject UI;
        private Node _target;
        public Text UpgradeCost;
        public Button UpgradeButton;
        public Text SellCost;

        public void SetTarget(Node target)
        {
            _target = target;
            transform.position = _target.GetBuildPosition;


            if (!_target.IsUpgraded)
            {
                UpgradeCost.text = "$" + _target.TurretBlueprint.UpgradeCost;
                UpgradeButton.interactable = true;
            }
            else
            {
                UpgradeCost.text = "MAX";
                UpgradeButton.interactable = false;
            }

            SellCost.text = "$" + _target.TurretBlueprint.GetSellCost();

            UI.SetActive(true);
        }

        public void Hide()
        {
            UI.SetActive(false);
        }

        public void Upgrade()
        {
            _target.UpgradeTurret();
            BuildManager.Instance.DeselectNode();
        }

        public void Sell()
        {
            _target.SellTurret();
            BuildManager.Instance.DeselectNode();
        }

    }
}
