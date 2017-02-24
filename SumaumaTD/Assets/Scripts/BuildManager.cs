using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class BuildManager : MonoBehaviour {
        //TODO remove unnecessary things

        #region Variables
        public static BuildManager Instance;
        public TurretUI TurretUI;
        //public bool CanBuild = true;

        [Header("Optional")]
        public GameObject BuildParticleEffectPrefab;
        public GameObject SellParticleEffectPrefab;

        private TurretBlueprint _turretToBuild;
        private Node _selectedNode;
        #endregion

        #region Properties
        public TurretBlueprint GetTurretToBuild { get { return _turretToBuild; } }
        #endregion

        public void Awake()
        {
            if (Instance != null) {
                Debug.LogError ("More than one BuildManager in scene!");
                return;
            }
            Instance = this;
        }
        
        public void SelectTurretToBuild(TurretBlueprint turret)
        {
            if (turret == null)
            {
                Debug.Log("prefab da torre não setado no shop");
                return;
            }
            _turretToBuild = turret;
            _selectedNode.BuildTurret(_turretToBuild);
            DeselectNode(); //selecting a turret from shop disables selection of node
        }


        public void SelectNode(Node node)
        {
            if (_selectedNode == node)
            {
                DeselectNode();
                return;
            }

            _selectedNode = node;
            _turretToBuild = null; 

            TurretUI.SetTarget(node);

        }

        public void SelectNodeToBuild(Node node)
        {
            if (_selectedNode == node)
            {
                DeselectNode();
                return;
            }

            _selectedNode = node;
            _turretToBuild = null; 

            TurretUI.SetTargetToBuild(node);
        }

        public void DeselectNode()
        {
            _selectedNode = null;
            TurretUI.Hide();
        }

        //public void BuildTorrentOn(Node node)
        //{
        //    if (PlayerStats.Money < _turretToBuild.Cost)
        //    {
        //        Debug.Log("Not enough money");
        //        return;
        //    }
        //    PlayerStats.Money -= _turretToBuild.Cost;

        //    GameObject turret = (GameObject)Instantiate(_turretToBuild.Prefab, node.GetBuildPosition, Quaternion.identity);
        //    node.Turret = turret;

        //    if (BuildParticleEffectPrefab != null)
        //    {
        //        GameObject effect =
        //            (GameObject) Instantiate(BuildParticleEffectPrefab, node.GetBuildPosition, Quaternion.identity);
        //        Destroy(effect, 5f);
        //    }

        //}
        
    }
}
