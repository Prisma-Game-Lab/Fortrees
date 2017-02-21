using UnityEngine;

namespace Assets.Scripts
{
    public class Shop : MonoBehaviour
    {
        private BuildManager _buildManager;
        public TurretBlueprint StandardTurretBlueprint;
        public TurretBlueprint AnotherTurretBlueprint;
        public TurretBlueprint MoreTurretBlueprint;

        public void Start()
        {
            _buildManager = BuildManager.Instance;
        }

        public void SelectStandardTurret()
        {
            _buildManager.SelectTurretToBuild(StandardTurretBlueprint);
        }

        public void SelectAnotherTurret()
        {
            _buildManager.SelectTurretToBuild(AnotherTurretBlueprint);

        }

        public void SelectMoreTurret()
        {
            _buildManager.SelectTurretToBuild(MoreTurretBlueprint);

        }
    }
}
