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

        [Header("Range Circle")]
        public GameObject RangeCircle;
        [Tooltip("Número pra multiplicar e ajeitar o tamanho do círculo de range")] public float RangeFixAjustment = 0.5f;

        private SpriteRenderer _rend;
        private BuildManager _buildManager;
        private NodeSelect _nodeSelect;
        private Sprite _defaultSprite;
        private GameObject _activeRangeCircle;

        #endregion
        
        #region Properties
        public Vector3 GetBuildPosition { get { return transform.position + PositionOffset; } }
        #endregion

        [Header("Audio")]
        public AudioClip NodeBuildSound;

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

            //Remove o círculo de range
            if (_activeRangeCircle != null) Destroy(_activeRangeCircle);
        }

        public void Highlight()
        {
            _rend.sprite = CanBuild ? _nodeSelect.HighlightedSprite : _nodeSelect.CantBuildSprite;

            if(Turret != null && _activeRangeCircle == null)
            {
                float range = Turret.GetComponent<Turret>().Range;

                //Habilita o círculo de Range
                _activeRangeCircle = (GameObject)Instantiate(RangeCircle, transform);

                //Ajusta o tamanho http://answers.unity3d.com/questions/139199/scale-object-to-certain-length.html
                float currentSize = _activeRangeCircle.GetComponent<Collider>().bounds.size.z;
                float newScale = range / currentSize;
                _activeRangeCircle.transform.localScale = new Vector3(newScale, newScale, newScale);

                //Ajusta a posição (+ alguns ajustes no tamanho)
                _activeRangeCircle.transform.position = transform.position;
                _activeRangeCircle.transform.localScale *= RangeFixAjustment;
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

            AudioSource.PlayClipAtPoint(NodeBuildSound, GetBuildPosition);
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
