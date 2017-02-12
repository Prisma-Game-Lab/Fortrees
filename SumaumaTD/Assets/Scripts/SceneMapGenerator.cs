using UnityEngine;

namespace Assets.Scripts
{
    public class SceneMapGenerator : MonoBehaviour
    {
        #region Variables   
        public GameObject NodesParentPrefab;
        public GameObject NodePrefab;
        public GameObject PathPrefab;
        public GameObject TurretUI;
        public TextAsset File;
        public GameObject WaypointPrefab;

        [Header("Path sprites")]
        public Sprite PathCurveRD;
        public Sprite PathCurveLD;
        public Sprite PathCurveRU;
        public Sprite PathCurveLU;
        public Sprite PathVertical;
        public Sprite PathHorizontal;

        private char[][] _mapReadings;
        private NodeSelect _nodesGameObject;
        private GameObject _environmentGameObject;
        private Waypoints _waypoints;
        private float _currentX;
        private float _currentZ;
        private bool _startNodeSet = false;
        #endregion

        public void GenerateMap()
        {
            Debug.Log("Generating Map");

            GenerateGameObjectsParents();

            GenerateMapReadingsMatrix();

            BuildSceneMap();
        }

        private void GenerateGameObjectsParents()
        {
            _nodesGameObject = Instantiate(NodesParentPrefab).GetComponent<NodeSelect>();
            _nodesGameObject.TurretUI = TurretUI;
            _environmentGameObject = new GameObject("Environment");
            _waypoints = new GameObject("Waypoints").AddComponent<Waypoints>();
        }

        private void GenerateMapReadingsMatrix()
        {
            var result = GetFileMapMatrixSizes();
            _mapReadings = new char[result[0]][];
            for (int l = 0; l < result[0]; l++)
            {
                _mapReadings[l] = new char[result[1]];
            }

            FillMapReadingsMatrix();
        }

        private int[] GetFileMapMatrixSizes()
        {
            int i = 0, j = 0;
            var found = false;
            foreach (var letter in File.text)
            {
                if (letter == '\n' || letter == '\r')
                {
                    found = true;
                    j++;
                }
                else
                    if (!found)
                    i++;
            }
            j++;
            return new[] { j, i };
        }

        private void FillMapReadingsMatrix()
        {
            int i = 0, j = 0;
            foreach (var letter in File.text)
            {
                if (letter == '\r' || letter == '\n') continue;

                _mapReadings[i][j] = letter;
                j++;
                if (j >= _mapReadings[i].Length)
                {
                    i++;
                    j = 0;
                }
            }
        }
        
        private void BuildSceneMap()
        {
            _currentZ = 0;
            _currentX = 0;
            for (int y = 0; y < _mapReadings.Length; y++)
            {
                for (int x = 0; x < _mapReadings[y].Length; x++)
                {
                    var letter = _mapReadings[y][x];

                    switch (letter)
                    {
                        case 'N':
                            GameObject temp = InstantiateNode();
                            if (!_startNodeSet)
                            {
                                _nodesGameObject.StartNode = temp.GetComponent<Node>();
                                _startNodeSet = true;
                            }
                            break;
                        case 'P':
                            InstantiatePath(x,y);
                            break;
                    }
                    _currentX+=4.5f;
                }
                _currentX = 0;
                _currentZ-=4.5f;
            }
        }

        private GameObject InstantiateNode()
        {
            var node = (GameObject)Instantiate(NodePrefab, new Vector3(_currentX, 0, _currentZ), Quaternion.identity);
            node.transform.parent = _nodesGameObject.transform;
            return node;
        }

        private void InstantiatePath(int i, int j)
        {
            var path = (GameObject)Instantiate(PathPrefab, new Vector3(_currentX, 0, _currentZ), Quaternion.identity);
            path.transform.parent = _environmentGameObject.transform;
            
            SetPathSprites(i, j, path);
        }

        private void SetPathSprites(int i, int j, GameObject path)
        {
            var adjPath = CheckForAdjacentPaths(i, j);
            if (adjPath.Up)
            {
                if (adjPath.Left)
                {
                    SetSprite(path, PathCurveLU);
                    SetWayPoint();
                }
                else if (adjPath.Right)
                {
                    SetSprite(path, PathCurveRU);
                    SetWayPoint();
                }
                else
                    SetSprite(path, PathVertical);
            }
            else if (adjPath.Down)
            {
                if (adjPath.Left)
                {
                    SetSprite(path, PathCurveLD);
                    SetWayPoint();
                }
                else if (adjPath.Right)
                {
                    SetSprite(path, PathCurveRD);
                    SetWayPoint();
                }
                else
                    SetSprite(path, PathVertical);
            }
            else
            {
                SetSprite(path, PathHorizontal);
            }
        }
        
        private void SetSprite(GameObject path, Sprite sprite)
        {
            path.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        }

        private void SetWayPoint()
        {
            var waypoint = (GameObject)Instantiate(WaypointPrefab, new Vector3(_currentX, 0, _currentZ), Quaternion.identity);
            waypoint.transform.parent = _waypoints.transform;
        }

        private AdjacentPath CheckForAdjacentPaths(int x, int y)
        {
            var  adj = new AdjacentPath();
            adj.ResetAdjPaths();
            var coords = new [] {y - 1, x - 1, y + 1, x + 1};
            if (coords[0] >= 0)
                if (_mapReadings[coords[0]][x] == 'P')
                    adj.Up = true;
            if(coords[1] >= 0)
                if (_mapReadings[y][coords[1]] == 'P')
                    adj.Left = true;
            if (coords[2] < _mapReadings.Length)
                if (_mapReadings[coords[2]][x] == 'P')
                    adj.Down = true;
            if (coords[3] < _mapReadings[y].Length)
                if (_mapReadings[y][coords[3]] == 'P')
                    adj.Right = true;

            return adj;
        }
        
        public struct AdjacentPath
        {
            public bool Up;
            public bool Down;
            public bool Left;
            public bool Right;

            public void ResetAdjPaths()
            {
                Up = false;
                Down = false;
                Left = false;
                Right = false;
            }
        }
    }
}
