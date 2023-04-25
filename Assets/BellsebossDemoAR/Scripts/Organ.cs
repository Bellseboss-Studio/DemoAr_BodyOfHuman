using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BellsebossDemoAR.Scripts
{
    public class Organ : MonoBehaviour
    {
        [HideInInspector] public List<Material> materials;
        [SerializeField] private OrganLabel label;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject exclamationButton, description;
        [SerializeField] private Transform objectToRotate;
        private bool _isFocused;
        private Transform _cameraTransform;
        public OrganLabel Label => label;

        public void Configure()
        {
            _cameraTransform = Camera.allCameras[0].transform;
            gameObject.SetActive(true);
            var renderers = gameObject.GetComponentsInChildren<Renderer>().ToList();
            materials = new List<Material>();
            foreach (var material in renderers.SelectMany(renderer1 => renderer1.materials))
            {
                materials.Add(material);
            }
        }

        public void ShowOrganCanvas()
        {
            _isFocused = true;
            RotateToPlayer();
            exclamationButton.SetActive(true);
            description.SetActive(false);
            canvas.gameObject.SetActive(true);
        }

        public void HideOrganCanvas()
        {
            canvas.gameObject.SetActive(false);
            _isFocused = false;
        }

        public void ShowDescription()
        {
            exclamationButton.SetActive(false);
            description.SetActive(true);
        }

        private void Update()
        {
            if (_isFocused)
            {
                RotateToPlayer();
                Debug.Log($"rotando {objectToRotate.gameObject} hacia {_cameraTransform}");
            }
        }

        private void RotateToPlayer()
        {
            objectToRotate.LookAt(_cameraTransform);
        }
        /*public void ConfigureCamera(Camera getCamera)
        {
            
            canvas.worldCamera = getCamera;
        }

        public Canvas GetCanvas()
        {
            return canvas;
        }*/
    }
}