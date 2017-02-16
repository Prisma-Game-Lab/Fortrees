﻿using UnityEngine;

namespace Assets.Scripts
{
    public class Shop : MonoBehaviour
    {
        private BuildManager _buildManager;
        public TurretBlueprint StardardTurretBlueprint;
        public TurretBlueprint AnotherTurretBlueprint;
        public TurretBlueprint MoreTurretBlueprint;

        public void Start()
        {
            _buildManager = BuildManager.Instance;
        }

        public void SelectStandardTurret()
        {
            _buildManager.SelectTurretToBuild(StardardTurretBlueprint);
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
