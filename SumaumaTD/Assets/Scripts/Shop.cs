using UnityEngine;

namespace Assets.Scripts
{
    public class Shop : MonoBehaviour
    {
        private BuildManager _buildManager;
        public TurretBlueprint StandardTurretBlueprint;
        public TurretBlueprint AnotherTurretBlueprint;
        public TurretBlueprint MoreTurretBlueprint;
        public MenuSounds Sounds;

        public void Start()
        {
            _buildManager = BuildManager.Instance;
        }

        public void SelectStandardTurret()
        {
            _buildManager.SelectTurretToBuild(StandardTurretBlueprint, Sounds);
        }

        public void SelectAnotherTurret()
        {
            _buildManager.SelectTurretToBuild(AnotherTurretBlueprint, Sounds);

        }

        public void SelectMoreTurret()
        {
            _buildManager.SelectTurretToBuild(MoreTurretBlueprint, Sounds);

        }
    }
}
