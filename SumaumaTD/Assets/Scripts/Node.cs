using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class Node : MonoBehaviour {
        //TODO remove unnecessary things

        #region Variables

        public bool CanBuild = true;
        public Vector3 PositionOffset;
        [HideInInspector]
        public GameObject Turret;
        [HideInInspector]
        public TurretBlueprint TurretBlueprint;
        [HideInInspector]
        public bool IsUpgraded = false;

        private SpriteRenderer _rend;
        private BuildManager _buildManager;
        private NodeSelect _nodeSelect;
        private Color _defaultColor;
        private GameObject _activeRangeCircle;
        private GameObject _highlight;
        private Transform _quad;

        [Header("Audio")]
        public AudioClip NodeBuildSound;
        [Range(0,1)] public float SoundVolume = 1f;


        #endregion
        
        #region Properties
        public Vector3 GetBuildPosition { get { return transform.position + PositionOffset; } }
        #endregion


        public void Start()
        {
            _quad = transform.GetChild(0);
            _rend = _quad.GetComponent<SpriteRenderer>();

            _defaultColor = _rend.color;
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
            //Remove o círculo de range
            if (_activeRangeCircle != null) Destroy(_activeRangeCircle);

            if (_highlight != null) Destroy(_highlight);
        }

        public void Highlight()
        {
			if (!CanBuild)
				return;
            
            ShowHighlight();

            ShowRangeCircle();
        }

        private void ShowHighlight()
        {
            if (_highlight == null)
            {
                GameObject prefab = PlayerStats.Seeds < 1 ? _nodeSelect.NoMoneyHighlight : _nodeSelect.Highlight;
                _highlight = (GameObject) Instantiate(prefab, transform);
                _highlight.transform.localScale = _quad.localScale;
                _highlight.transform.position = _quad.position;
            }
        }

        private void ShowRangeCircle()
        {
            if (Turret != null && _activeRangeCircle == null)
            {
                float range = Turret.GetComponent<Turret>().Range;

                //Habilita o círculo de Range
                _activeRangeCircle = (GameObject) Instantiate(_nodeSelect.RangeCircle, transform);

                //Ajusta o tamanho http://answers.unity3d.com/questions/139199/scale-object-to-certain-length.html
                float currentSize = _activeRangeCircle.GetComponent<Collider>().bounds.size.z;
                float newScale = range/currentSize;
                _activeRangeCircle.transform.localScale = new Vector3(newScale, newScale, newScale);

                //Ajusta a posição (+ alguns ajustes no tamanho)
                _activeRangeCircle.transform.position = transform.position;
                _activeRangeCircle.transform.localScale *= _nodeSelect.RangeFixAjustment;
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

        public void BuildTurret(TurretBlueprint blueprint, MenuSounds sounds)
        {
            if (PlayerStats.Seeds < blueprint.Cost)
            {
                Debug.Log("Not enough money");
                sounds.PressFail();
                return;
            }
            PlayerStats.Seeds -= blueprint.Cost;

            sounds.Press();
            GameObject turret = (GameObject)Instantiate(blueprint.Prefab, GetBuildPosition, Quaternion.identity);
            Turret = turret;

            TurretBlueprint = blueprint;

            if (_buildManager.BuildParticleEffectPrefab != null)
            {
                GameObject effect =
                    (GameObject)Instantiate(_buildManager.BuildParticleEffectPrefab, GetBuildPosition, Quaternion.identity);
                Destroy(effect, 5f);
            }

            AudioSource.PlayClipAtPoint(NodeBuildSound, GetBuildPosition, SoundVolume);
            Debug.Log("Turret built!");
        }

        public void UpgradeTurret(MenuSounds sounds)
        {
            if (PlayerStats.Seeds < TurretBlueprint.UpgradeCost)
            {
                Debug.Log("Not enough money to upgrade");
                sounds.PressFail();
                return;
            }
            PlayerStats.Seeds  -= TurretBlueprint.UpgradeCost;
            sounds.Press();

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
