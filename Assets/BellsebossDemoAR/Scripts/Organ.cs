using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BellsebossDemoAR.Scripts
{
    public class Organ : MonoBehaviour
    {
        public List<Material> materials;
        [SerializeField] private Canvas canvas;
        [SerializeField] private GameObject exclamationButton, description;

        public void Configure()
        {
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
            exclamationButton.SetActive(true);
            description.SetActive(false);
            canvas.gameObject.SetActive(true);
        }

        public void HideOrganCanvas()
        {
            canvas.gameObject.SetActive(false);
        }

        public void ShowDescription()
        {
            exclamationButton.SetActive(false);
            description.SetActive(true);
        }
    }
}