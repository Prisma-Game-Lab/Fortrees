using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TurretUI : MonoBehaviour
    {
        //TODO remove unnecessary things
        public Sprite[] BuildBackgrounds;

        [Header("Dependencies")]
        public GameObject UpdateUI;
        public GameObject BuildUI;
        public Text UpgradeCost;
        public Text BuildCost;
        public Animator BuildDescriptionAnimator;
        public Image BuildBackgroundImage;
        public Shop TurretShop;
        public Button UpgradeButton;
        private Node _target;
        //public Text SellCost;
        
        public void SetTarget(Node target)
        {
            _target = target;
            transform.position = _target.GetBuildPosition;
            
            if (!_target.IsUpgraded)
            {
                UpgradeCost.text = _target.TurretBlueprint.UpgradeCost.ToString();
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

        public void Select (int item)
        {
            BuildBackgroundImage.sprite = BuildBackgrounds[item];
            switch (item)
            {
                case 0:
                    BuildCost.text = TurretShop.AnotherTurretBlueprint.Cost.ToString();
                    BuildDescriptionAnimator.runtimeAnimatorController = TurretShop.AnotherTurretBlueprint.TurretAnimator;
                    break;
                case 1:
                    BuildCost.text = TurretShop.StandardTurretBlueprint.Cost.ToString();
                    BuildDescriptionAnimator.runtimeAnimatorController = TurretShop.StandardTurretBlueprint.TurretAnimator;
                    break;
                case 2:
                    BuildCost.text = TurretShop.MoreTurretBlueprint.Cost.ToString();
                    BuildDescriptionAnimator.runtimeAnimatorController = TurretShop.MoreTurretBlueprint.TurretAnimator;
                    break;
                default:
                    Debug.Log("Invalid Item Selected");
                    break;
            }
        }
    }
}
