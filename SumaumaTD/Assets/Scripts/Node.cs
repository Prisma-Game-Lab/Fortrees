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

        private SpriteRenderer _rend;
        private BuildManager _buildManager;
        private NodeSelect _nodeSelect;
        private Sprite _defaultSprite;

        #endregion
        
        #region Properties
        public Vector3 GetBuildPosition { get { return transform.position + PositionOffset; } }
        #endregion

        public void Start()
        {
            _rend = transform.GetChild(0).GetComponent<SpriteRenderer>();//transform.GetChild(0).GetComponent<Renderer>();
            _defaultSprite = _rend.sprite;
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

            _nodeSelect.ChangeSelectedNode(this);
        }

        public void OnMouseExit()
        {
            _rend.sprite = _defaultSprite;
        }

        public void Highlight()
        {
            _rend.sprite = CanBuild ? _nodeSelect.HighlightedSprite : _nodeSelect.CantBuildSprite;
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
