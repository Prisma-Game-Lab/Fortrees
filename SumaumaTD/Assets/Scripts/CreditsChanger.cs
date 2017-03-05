using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CreditsChanger : MonoBehaviour
    {
        public Sprite[] CreditsImages;

        [Header("Unity Objects")]
        public MenuUI MenuUI;
        public Image ImageScript;

        [Header("Controller")]
        [Tooltip("Valor mínimo que um eixo do controle precisa ir para uma direção para detectar movimento")] public float MinimumAxisToMove = 0.5f;
        [Tooltip("Valor de segundos para esperar entre 2 movimentos. Usado para evitar que a seleção 'ande' muito para o lado")] public float SecondsToWait = 0.2f;

        private int _currentImage = 0;
        private float _secondsLeftToWait = 0f;

        // Update is called once per frame
        void Update()
        {
            //Evita que a seleção ande muito quando o botão é pressionado
            if (_secondsLeftToWait > 0f)
            {
                _secondsLeftToWait -= Time.deltaTime;
                return;
            }

            if (Input.GetAxis("CloseMenuButton") != 0.0)
            {
                MenuUI.Return();
            }

            if (Input.GetAxis("HorizontalSelection") < -MinimumAxisToMove || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetMouseButtonDown(0))
            {
                if (_currentImage <= 0) _currentImage = CreditsImages.Length;
                ImageScript.sprite = CreditsImages[--_currentImage];
                _secondsLeftToWait = SecondsToWait;
            }
            
            if (Input.GetAxis("HorizontalSelection") > MinimumAxisToMove || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetMouseButtonDown(1))
            {
                if (_currentImage >= CreditsImages.Length) _currentImage = 0;
                ImageScript.sprite = CreditsImages[_currentImage++];
                _secondsLeftToWait = SecondsToWait;
            }
        }
    }
}
