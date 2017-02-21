using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TurretUI : MonoBehaviour
    {
        //TODO remove unnecessary things
        public GameObject UpdateUI;
        public GameObject BuildUI;
        private Node _target;
        public Text UpgradeCost;
        public Button UpgradeButton;
        public Sprite[] BuildBackgrounds;
        public Image BuildBackgroundImage;
        //public Text SellCost;
        
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

            //SellCost.text = "$" + _target.TurretBlueprint.GetSellCost();
            UpdateUI.SetActive(true);
            BuildUI.SetActive(false);
        }

        public void SetTargetToBuild(Node target)
        {
            _target = target;
            transform.position = _target.GetBuildPosition;
            UpdateUI.SetActive(false);
            BuildUI.SetActive(true);
        }

        public void Hide()
        {
            UpdateUI.SetActive(false);
            BuildUI.SetActive(false);
        }

        public void Upgrade()
        {
            _target.UpgradeTurret();
            BuildManager.Instance.DeselectNode();
        }
        
        public void Sell()
        {
            //_target.SellTurret(); 
            BuildManager.Instance.DeselectNode();
        }

        public void SetBackground (int item)
        {
            BuildBackgroundImage.sprite = BuildBackgrounds[item];
        }
    }
}
