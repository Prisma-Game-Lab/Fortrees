using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace Assets.Scripts
{
    public class EventSystemManager : MonoBehaviour
    {
        [Tooltip("Botão que será selecionado quando este canvas for habilitado")]
        public GameObject FirstSelectableButton;

        public void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(FirstSelectableButton);
        }
    }
}
