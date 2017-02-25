using UnityEngine;
using System.Linq;

namespace Assets.Scripts
{
	public class NodeSelect : MonoBehaviour {

		public Node StartNode;
		public float NodeSize;
        [Tooltip("Espaço entre os centros dos nodes")] public float NodeDistance = 4f;
        public GameObject TurretUI;
        
	    [Header("Highlight")]
        public GameObject Highlight;
        public GameObject NoMoneyHighlight;

        [Header("Range Circle")]
        public GameObject RangeCircle;
        [Tooltip("Número pra multiplicar e ajeitar o tamanho do círculo de range")]
        public float RangeFixAjustment = 0.5f;

        [Header("Sprites")]
		public Sprite[] Grass;
		public Sprite[] Rocks;

        [Header("Audio")]
        public AudioSource UIAudioSource;
        public AudioClip ConfirmationSound;
        public AudioClip SelectionSound;

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
        
        public void Start () {
			_selectPosition = StartNode.transform.position;
			_radius = NodeSize / 2;
			_selectedNode = StartNode;
            _updateCanvas = TurretUI.transform.GetChild(0).gameObject;
            _buildCanvas = TurretUI.transform.GetChild(1).gameObject;
            _shop = TurretUI.gameObject.GetComponent<Shop>();
            _turretUIScript = TurretUI.gameObject.GetComponent<TurretUI>();
        }
        
        public void Update () {
            //Evita que a seleção ande muito quando o botão é pressionado
            if(_framesLeftToWait > 0)
            {
                _framesLeftToWait--;
                return;
            }

			_selectedNode.Highlight ();

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
            else if (_buildCanvas.activeInHierarchy)
            {
                /*
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
                */

                if (Input.GetAxis("CloseMenuButton") != 0.0)
                {
                    _buildCanvas.SetActive(false);
                    _framesLeftToWait = framesToWait;
                }
                
            }

            //if(Input.GetKeyDown(KeyCode.Return)){
            //se os 2 canvas estiverem desativados, checa botões de selecionar node se o jogo não estiver pausado
            else if (Time.timeScale > 0.0f){
                if (Input.GetAxis("NodeSelectionButton") != 0.0)
                {
                    UIAudioSource.PlayOneShot(ConfirmationSound);
                    _selectedNode.BuildOnSelectedNode();
                    _framesLeftToWait = framesToWait;
                }

                //if (Input.GetKeyDown (KeyCode.LeftArrow)) {
                if (Input.GetAxis("HorizontalSelection") < -minimumAxisToMove || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Debug.Log("Horizontal " + Input.GetAxis("HorizontalSelection"));
                    ChangeSelectedNode(Vector3.left);
                    _framesLeftToWait = framesToWait;
                }

                //if (Input.GetKeyDown (KeyCode.RightArrow)) {
                if (Input.GetAxis("HorizontalSelection") > minimumAxisToMove || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    ChangeSelectedNode(Vector3.right);
                    _framesLeftToWait = framesToWait;
                }

                //if (Input.GetKeyDown (KeyCode.UpArrow)) {
                if (Input.GetAxis("VerticalSelection") > minimumAxisToMove || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    ChangeSelectedNode(Vector3.forward);
                    _framesLeftToWait = framesToWait;
                }

                //if (Input.GetKeyDown (KeyCode.DownArrow)) {
                if (Input.GetAxis("VerticalSelection") < -minimumAxisToMove || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    ChangeSelectedNode(Vector3.back);
                    _framesLeftToWait = framesToWait;
                }
            }
        }

		void ChangeSelectedNode(Vector3 dir)
        {
            Debug.Log("Move");
            UIAudioSource.PlayOneShot(SelectionSound);
            dir = NodeDistance * dir;
			Vector3 temp = _selectPosition + dir; //temp guarda o centro onde a esfera vai buscar por nodes
			Collider[] cast = Physics.OverlapSphere (temp, _radius);

            if (cast.Length < 1) { //se nada foi overlapped, estamos no último node daquela direção
				return;
			}

			while (cast[0].gameObject.tag != "Node" || !cast[0].GetComponent<Node>().CanBuild) { //procura por um node selecionável
                Collider castedNode = FindNodeInCollisionArray(cast);

                if(castedNode != null) //o FindNodeInCollisionArray retorna null se não encontrar nada
                {
                    cast[0] = castedNode;
                    break;
                }

                Debug.Log("Skip");
                //Node não encontrado no vetor, procurando na próxima posição que pode ter um node
                temp += dir;
				cast = Physics.OverlapSphere (temp, _radius);
                if (cast.Length < 1)
					return;
			}
            
			_selectedNode.OnMouseExit (); //tira o highlight
			_selectedNode = cast [0].GetComponent<Node>();
			_selectedNode.Highlight ();
			_selectPosition = temp;
		}

        public void ChangeSelectedNode (Node node)
        {
            UIAudioSource.PlayOneShot(SelectionSound);
            _selectedNode.OnMouseExit();
            _selectedNode = node;
            _selectedNode.Highlight();
            _selectPosition = _selectedNode.transform.position;
        }

        Collider FindNodeInCollisionArray(Collider[] cast)
        {
            while (cast.Length > 2)
            {
                cast = cast.Skip(1).ToArray(); //obrigado http://stackoverflow.com/questions/27965131/how-to-remove-the-first-element-in-an-array

                if (cast[0].gameObject.tag == "Node") //achou o Node
                {
                    return cast[0];
                }
            }

            return null;
        }
	}
}
