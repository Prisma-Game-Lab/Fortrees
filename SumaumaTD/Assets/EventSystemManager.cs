using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventSystemManager : MonoBehaviour {
    [Tooltip("Botão que será selecionado quando este canvas for habilitado")] public GameObject FirstSelectableButton;

    public void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(FirstSelectableButton);
    }
}
