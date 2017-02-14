using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class SceneMapGenerator : MonoBehaviour
    {
        #region Variables   
        
        [Header("Mapa em txt")]
        public TextAsset File;

        [Header("Prefabs")]
        public GameObject NodesParentPrefab;
        public GameObject NodePrefab;
        public GameObject PathPrefab;
        public GameObject TurretUI;
        public GameObject WaypointPrefab;
        public GameObject StartPrefab;
        public GameObject EndPrefab;

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
        private WaypointsCoords _waypointsCoords;
        private List<bool> _waypointDirections;

        private const char StartLetter = 'S';
        private const char EndLetter = 'E';
        private const char PathLetter = 'P';
        private const char NodeLetter = 'N';
        #endregion

        public void GenerateMap()
        {
            Debug.Log("Generating Map");
            _waypointsCoords = new WaypointsCoords();

            GenerateGameObjectsParents();

            GenerateMapReadingsMatrix();

            BuildSceneMap();

            SetAllWayPoints();
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
            _mapReadings = new char[result.J][];
            for (int l = 0; l < result.J; l++)
            {
                _mapReadings[l] = new char[result.I[l]];
            }

            FillMapReadingsMatrix();
        }

        private Sizes GetFileMapMatrixSizes()
        {
            int j = 0;
            int[] i = Enumerable.Repeat(0, 100).ToArray(); ;
            bool lineWasCreatedLastIteration = false;
            foreach (var letter in File.text)
            {
                bool addWaypointDirection = false;
                if (letter == '\n' || letter == '\r')
                {
                    if (lineWasCreatedLastIteration)
                    {
                        lineWasCreatedLastIteration = false;
                        continue;
                    }
                    j++;
                    addWaypointDirection = true;
                    lineWasCreatedLastIteration = true;
                }
                else if (letter == '<' || letter == '>')
                {
                    addWaypointDirection = true;
                }
                else
                {
                    if(!IsALetter(letter)) continue;
                    lineWasCreatedLastIteration = false;
                    i[j] = i[j] + 1;
                }

                if (addWaypointDirection)
                {
                    bool right = true;
                    switch (letter)
                    {
                        case '<': //enemy moving left
                            right = false;
                            break;
                        case '>': //enemy moving right
                            right = true;
                            break;
                    }
                    _waypointDirections.Add(right);
                }
            }
            j++;
            return new Sizes(j, i);
        }

        private bool IsALetter(char letter)
        {
            return letter == NodeLetter || letter == PathLetter || letter == StartLetter || letter == EndLetter;
        }

        private void FillMapReadingsMatrix()
        {
            int i = 0, j = 0;
            foreach (var letter in File.text)
            {
                if (letter == '\r' || letter == '\n') continue;
                if (!IsALetter(letter)) continue;
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
                        case NodeLetter:
                            InstantiateNodeCase();
                            break;
                        case PathLetter:
                            InstantiatePathCase(x,y);
                            break;
                        case StartLetter:
                            InstantiateNodeCase();
                            InstantiateStartCase();
                            break;
                        case EndLetter:
                            InstantiateNodeCase();
                            InstantiateEndCase(y);
                            break;
                       default:
                            _currentX -= 4.5f;
                            break;

                    }
                    _currentX+=4.5f;
                }
                _currentX = 0;
                _currentZ-=4.5f;
            }
        }

        

        private void InstantiateNodeCase()
        {
            GameObject temp = InstantiateNode();
            if (!_startNodeSet)
            {
                _nodesGameObject.StartNode = temp.GetComponent<Node>();
                _startNodeSet = true;
            }
        }

        private GameObject InstantiateNode()
        {
            var node = (GameObject)Instantiate(NodePrefab, new Vector3(_currentX, 0, _currentZ), Quaternion.identity);
            node.transform.parent = _nodesGameObject.transform;
            return node;
        }

        private void InstantiatePathCase(int i, int j)
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
                    SaveWayPoint(j);
                }
                else if (adjPath.Right)
                {
                    SetSprite(path, PathCurveRU);
                    SaveWayPoint(j);
                }
                else
                    SetSprite(path, PathVertical);
            }
            else if (adjPath.Down)
            {
                if (adjPath.Left)
                {
                    SetSprite(path, PathCurveLD);
                    SaveWayPoint(j);
                }
                else if (adjPath.Right)
                {
                    SetSprite(path, PathCurveRD);
                    SaveWayPoint(j);
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

        private void SaveWayPoint(int j)
        {
           _waypointsCoords.AddCoordinateToColumn(_currentX, _currentZ, j, _waypointDirections[j]);
        }

        private void SetAllWayPoints()
        {
            foreach (var coords in _waypointsCoords.Coordinates)
            {
                if(coords[0].Right)
                    for (var j = 0; j < coords.Count; j++)
                    {
                        var current = coords[j];
                        SetWayPoint(current.X, current.Z);
                    }
                else
                    for (var j = coords.Count -1; j >= 0; j--)
                    {
                        var current = coords[j];
                        SetWayPoint(current.X, current.Z);
                    }
            }
        }

        private void SetWayPoint(float x, float z)
        {
            var waypoint = (GameObject)Instantiate(WaypointPrefab, new Vector3(x, WaypointPrefab.transform.localPosition.y, z), Quaternion.identity);
            waypoint.transform.parent = _waypoints.transform;
        }

        private AdjacentPath CheckForAdjacentPaths(int x, int y)
        {
            var  adj = new AdjacentPath();
            adj.ResetAdjPaths();
            var coords = new [] {y - 1, x - 1, y + 1, x + 1};
            if (coords[0] >= 0)
                if (_mapReadings[coords[0]][x] == PathLetter || _mapReadings[coords[0]][x] == StartLetter || _mapReadings[coords[0]][x] == EndLetter)
                    adj.Up = true;
            if(coords[1] >= 0)
                if (_mapReadings[y][coords[1]] == PathLetter || _mapReadings[coords[0]][x] == StartLetter || _mapReadings[coords[0]][x] == EndLetter)
                    adj.Left = true;
            if (coords[2] < _mapReadings.Length)
                if (_mapReadings[coords[2]][x] == PathLetter || _mapReadings[coords[0]][x] == StartLetter || _mapReadings[coords[0]][x] == EndLetter)
                    adj.Down = true;
            if (coords[3] < _mapReadings[y].Length)
                if (_mapReadings[y][coords[3]] == PathLetter || _mapReadings[coords[0]][x] == StartLetter || _mapReadings[coords[0]][x] == EndLetter)
                    adj.Right = true;

            return adj;
        }

        private void InstantiateStartCase()
        {
            var start = (GameObject)Instantiate(StartPrefab, new Vector3(_currentX, StartPrefab.transform.localPosition.y, _currentZ), Quaternion.identity);

            var waveSpawner = GameObject.FindObjectOfType<WaveSpawner>();
            waveSpawner.SpawnPoint = start.transform;
        }

        private void InstantiateEndCase(int j)
        {
            SaveWayPoint(j);
            var end = (GameObject)Instantiate(EndPrefab, new Vector3(_currentX, EndPrefab.transform.localPosition.y, _currentZ), Quaternion.identity);
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

        public struct Sizes
        {
            public int J;
            public int[] I;

            public Sizes(int j, int[] i)
            {
                J = j;
                I = i;
            }
        }

        
    }
}
