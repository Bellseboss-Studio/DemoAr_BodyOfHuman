using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace BellsebossDemoAR.Scripts
{
    public class Organ : MonoBehaviour
    {
        public List<Material> materials;
        [SerializeField] private OrganLabel label;
        public OrganLabel Label => label;


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
    }
}