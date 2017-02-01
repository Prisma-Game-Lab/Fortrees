using UnityEngine;

namespace Assets.Scripts
{
	public class NodeSelect : MonoBehaviour {

		public Node startNode;
		public float nodeSize;
        public GameObject turretUI;

        [Header("Controller")]
        [Tooltip("Valor mínimo que um eixo do controle precisa ir para uma direção para detectar movimento")] public float minimumAxisToMove = 0.5f;
        [Tooltip("Valor de frames para esperar entre 2 movimentos. Usado para evitar que o node selecione 'ande' muito para o lado")] public int framesToWait = 5;

        private Vector3 _selectPosition;
		private float _radius;
		private Node _selectedNode;
		private int _framesLeftToWait = 0;
        private GameObject _buildCanvas;
        private GameObject _updateCanvas;
        private TurretUI _turretUIScript;
        private Shop _shop;


        // Use this for initialization
        void Start () {
			_selectPosition = startNode.transform.position;
			_radius = nodeSize / 2;
			_selectedNode = startNode;
            _updateCanvas = turretUI.transform.GetChild(0).gameObject;
            _buildCanvas = turretUI.transform.GetChild(1).gameObject;
            _shop = turretUI.gameObject.GetComponent<Shop>();
            _turretUIScript = turretUI.gameObject.GetComponent<TurretUI>();
        }

		// Update is called once per frame
		void Update () {
            //Evita que a seleção ande muito quando o botão é pressionado
            if(_framesLeftToWait > 0)
            {
                _framesLeftToWait--;
                return;
            }

			_selectedNode.Highlight ();
            //if (Input.GetKeyDown (KeyCode.LeftArrow)) {
            if (Input.GetAxis("HorizontalSelection") > minimumAxisToMove || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                changeSelectedNode(Vector3.left);
                _framesLeftToWait = framesToWait;
			}

            //if (Input.GetKeyDown (KeyCode.RightArrow)) {
            if (Input.GetAxis("HorizontalSelection") < -minimumAxisToMove || Input.GetKeyDown(KeyCode.RightArrow))
            {
                changeSelectedNode (Vector3.right);
                _framesLeftToWait = framesToWait;
            }

            //if (Input.GetKeyDown (KeyCode.UpArrow)) {
            if (Input.GetAxis("VerticalSelection") > minimumAxisToMove || Input.GetKeyDown(KeyCode.UpArrow)) { 
                changeSelectedNode (Vector3.forward);
                _framesLeftToWait = framesToWait;
            }

            //if (Input.GetKeyDown (KeyCode.DownArrow)) {
            if (Input.GetAxis("VerticalSelection") < -minimumAxisToMove || Input.GetKeyDown(KeyCode.DownArrow)) { 
                changeSelectedNode (Vector3.back);
                _framesLeftToWait = framesToWait;
            }

            //checa controles do Update Canvas (se ele estiver ativado)
            if (_updateCanvas.activeInHierarchy) {
                if (Input.GetAxis("UpgradeButton") != 0.0)
                {
                    _turretUIScript.Upgrade();
                    _framesLeftToWait = framesToWait;
                }

                if(Input.GetAxis("CloseMenuButton") != 0.0)
                {
                    _updateCanvas.SetActive(false);
                    _framesLeftToWait = framesToWait;
                }
            }

            //checa controles do Build Canvas (se ele estiver ativado)
            else if (_buildCanvas.activeInHierarchy) {
                if (Input.GetAxis("StandardBuyButton") != 0.0)
                {
                    _shop.SelectStandardTurret();
                    _framesLeftToWait = framesToWait;
                }


                if (Input.GetAxis("MissileBuyButton") != 0.0)
                {
                    _shop.SelectAnotherTurret();
                    _framesLeftToWait = framesToWait;
                }

                if (Input.GetAxis("CloseMenuButton") != 0.0)
                {
                    _buildCanvas.SetActive(false);
                    _framesLeftToWait = framesToWait;
                }
            }

            //if(Input.GetKeyDown(KeyCode.Return)){
            //se os 2 canvas estiverem desativados, checa botão de selecionar node
            else {
                if (Input.GetAxis("NodeSelectionButton") != 0.0)
                {
                    _selectedNode.BuildOnSelectedNode();
                    _framesLeftToWait = framesToWait;
                }
            }
        }

		void changeSelectedNode(Vector3 dir)
        {
            dir = 5 * dir; //5 é o tamanho do espaço entre os centros dos nodes
			Vector3 temp = _selectPosition + dir; //temp guarda o centro onde a esfera vai buscar por nodes
			Collider[] cast = Physics.OverlapSphere (temp, _radius);

			if (cast.Length < 1) { //se nada foi overlapped, estamos no último node daquela direção
				return;
			}

			while (cast[0].gameObject.tag != "Node" || !cast[0].GetComponent<Node>().IsSelectable) { //procura por um node selecionável
				temp += dir;
				cast = Physics.OverlapSphere (temp, _radius);

                Debug.Log("Teste");
                if (cast.Length < 1)
					return;
			}

			_selectedNode.OnMouseExit (); //tira o highlight
			_selectedNode = cast [0].GetComponent<Node>();
			_selectedNode.Highlight ();
			_selectPosition = temp;
		}

        public void changeSelectedNode (Node node)
        {
            _selectedNode.OnMouseExit();
            _selectedNode = node;
            _selectedNode.Highlight();
            _selectPosition = _selectedNode.transform.position;
        }
	}
}
