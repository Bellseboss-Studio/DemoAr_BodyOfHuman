using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BellsebossDemoAR.Scripts
{
    public class ModelFocusedChecker : MonoBehaviour
    {
        [SerializeField] private Camera ARCamera;
        [SerializeField] private StateOfGame stateOfGame;
        [SerializeField] private LayerMask layer;
        private bool _objectWasInstantiated = false;
        private ObjectInteractableInWord _objectInteractableInWord;
        private bool _objectIsFocus = false;
        private RaycastHit _rayCastHit;
        private Transform _arCameraTransform;

        [Header("BodyUI")] 
        [SerializeField] private GameObject bodyUIPanel;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Button exclamationButton;
        

        private void Start()
        {
            _arCameraTransform = ARCamera.transform;
            stateOfGame.OnInstantiateElement += () =>
            {
                _objectWasInstantiated = true;
                stateOfGame.GetObjectInstantiate().TryGetComponent(out _objectInteractableInWord);
                _objectInteractableInWord.OnChangeObject += (label) =>
                {
                    _objectIsFocus = false;
                    ChangeObjectUI(label);
                };
            };
        }

        private void Update()
        {
            if (_objectWasInstantiated)
            {
                Debug.DrawRay(_arCameraTransform.position, _arCameraTransform.forward * 1000, Color.blue);
                var rayCastInfo = Physics.RaycastAll(_arCameraTransform.position, _arCameraTransform.forward, 1000/*, layer.value*/);
                if (rayCastInfo.Length >= 1)
                {
                    if (_objectIsFocus)
                    {
                        foreach (var raycastHit in rayCastInfo)
                        {
                            if (raycastHit.collider == _rayCastHit.collider)
                            {
                                return;
                            }
                        }
                        CurrentOrganStoppedBeingFocused();
                    }
                    foreach (var raycastHit in rayCastInfo)
                    {
                        if (raycastHit.transform.gameObject == _objectInteractableInWord.GetCurrentOrgan())
                        {
                            CurrentOrganWasFocused(raycastHit);
                        }
                        else
                        {
                            if (raycastHit.transform.parent.gameObject == _objectInteractableInWord.GetCurrentOrgan())
                            {
                                CurrentOrganWasFocused(raycastHit);
                            }
                        }
                    }
                }
                else
                {
                    if (_objectIsFocus)
                    {
                        CurrentOrganStoppedBeingFocused();
                    }
                }
            }
        }

        private void CurrentOrganStoppedBeingFocused()
        {
            _objectIsFocus = false;
            HideOrganCanvas();
        }

        private void CurrentOrganWasFocused(RaycastHit raycastHit)
        {
            _objectIsFocus = true;
            _rayCastHit = raycastHit;
            ShowOrganCanvas();
        }

        private void ChangeObjectUI(OrganLabel label)
        {
            description.text = label.Description;
            HideOrganCanvas();
        }
        
        private void ShowOrganCanvas()
        {
            exclamationButton.gameObject.SetActive(true);
            description.gameObject.SetActive(false);
            bodyUIPanel.gameObject.SetActive(true);
        }

        private void HideOrganCanvas()
        {
            bodyUIPanel.gameObject.SetActive(false);
        }

        public void ShowDescription()
        {
            exclamationButton.gameObject.SetActive(false);
            description.gameObject.SetActive(true);
        }
        
    }
}