using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Node : MonoBehaviour {
        //TODO remove unnecessary things

        #region Variables

        public bool CanBuild = true;
        public Color HoverColor;
        public Color CantBuildColor;
        public Vector3 PositionOffset;
        [HideInInspector]
        public GameObject Turret;
        [HideInInspector]
        public TurretBlueprint TurretBlueprint;
        [HideInInspector]
        public bool IsUpgraded = false;
        public bool IsSelectable = true;

        private Renderer _rend;
        private Color _startColor;
        private BuildManager _buildManager;
        private NodeSelect _nodeSelect;
        #endregion
        
        #region Properties
        public Vector3 GetBuildPosition { get { return transform.position + PositionOffset; } }
        #endregion

        public void Start()
        {
            _rend = GetComponent<Renderer>();
            _startColor = _rend.material.color;
            _buildManager = BuildManager.Instance;
            _nodeSelect = gameObject.GetComponentInParent<NodeSelect>();
        }

       
        public void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; //checar se o mouse não tá na IU
            }

            BuildOnSelectedNode();
        }
        
        public void OnMouseEnter() //Hover
        {

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return; //checar se o mouse não tá na IU
            }

            _nodeSelect.changeSelectedNode(this);
        }

        public void OnMouseExit()
        {
            _rend.material.color = _startColor;
        }

        public void Highlight()
        {
            if (CanBuild)
            {
                _rend.material.color = HoverColor;
            }
            else
            {
                _rend.material.color = CantBuildColor;
            }
        }

        public void BuildOnSelectedNode()
        {
            if (Turret != null)
            {//se já existe um turret no node, seleciona o node 
                _buildManager.SelectNode(this);
                return;
            }

            if (!CanBuild)
                return;

            _buildManager.SelectNodeToBuild(this);
        }

        public void BuildTurret(TurretBlueprint blueprint)
        {
            if (PlayerStats.Seeds < blueprint.Cost)
            {
                Debug.Log("Not enough money");
                return;
            }
            PlayerStats.Seeds -= blueprint.Cost;

            GameObject turret = (GameObject)Instantiate(blueprint.Prefab, GetBuildPosition, Quaternion.identity);
            Turret = turret;

            TurretBlueprint = blueprint;

            if (_buildManager.BuildParticleEffectPrefab != null)
            {
                GameObject effect =
                    (GameObject)Instantiate(_buildManager.BuildParticleEffectPrefab, GetBuildPosition, Quaternion.identity);
                Destroy(effect, 5f);
            }

            Debug.Log("Turret built!");
        }

        public void UpgradeTurret()
        {
            if (PlayerStats.Seeds < TurretBlueprint.UpgradeCost)
            {
                Debug.Log("Not enough money to upgrade");
                return;
            }
            PlayerStats.Seeds  -= TurretBlueprint.UpgradeCost;

            //Get rid of the old turret
            Destroy(Turret);

            //Build the upgraded version
            GameObject turret = (GameObject)Instantiate(TurretBlueprint.UpgradedPrefab, GetBuildPosition, Quaternion.identity);
            Turret = turret;

            if (_buildManager.BuildParticleEffectPrefab != null)
            {
                GameObject effect =
                    (GameObject)Instantiate(_buildManager.BuildParticleEffectPrefab, GetBuildPosition, Quaternion.identity);
                Destroy(effect, 5f);
            }

            IsUpgraded = true;

            Debug.Log("Turret upgraded!");
        }



        /*
        public void SellTurret()
        {
            
            PlayerStats.Money += TurretBlueprint.GetSellCost();
            //todo: spawn effect

            if (_buildManager.SellParticleEffectPrefab != null)
            {
                GameObject effect =
                    (GameObject)Instantiate(_buildManager.SellParticleEffectPrefab, GetBuildPosition, Quaternion.identity);
                Destroy(effect, 5f);
            }

            Destroy(Turret);
            TurretBlueprint = null;
            

            Debug.Log("Turret sold!");
        }*/
        
    }
}
