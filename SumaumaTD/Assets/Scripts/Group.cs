using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class Group 
    {
        
        public GameObject EnemyPrefab;
        public int Count;
        public float EnemySpawnRate;
        public float SecondsToNextGroup;
    }
}
