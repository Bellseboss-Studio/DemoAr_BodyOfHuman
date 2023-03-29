using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

        private void Start()
        {
            _arCameraTransform = ARCamera.transform;
            stateOfGame.OnInstantiateElement += () =>
            {
                _objectWasInstantiated = true;
                stateOfGame.GetObjectInstantiate().TryGetComponent(out _objectInteractableInWord);
                _objectInteractableInWord.OnChangeObject += () =>
                {
                    _objectIsFocus = false;
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
                    Debug.Log(rayCastInfo.Length);
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
                    Debug.Log(rayCastInfo.Length);
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
            _objectInteractableInWord.CurrentOrganStoppedBeingFocused();
        }

        private void CurrentOrganWasFocused(RaycastHit raycastHit)
        {
            _objectInteractableInWord.CurrentOrganWasFocused();
            _objectIsFocus = true;
            _rayCastHit = raycastHit;
        }
    }
}