using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BellsebossDemoAR.Scripts
{
    public class ObjectInteractableInWord : MonoBehaviour
    {
        [SerializeField] private Organ body;
        private int _index;
        [SerializeField] private List<Organ> organs;
        public Action OnChangeObject;
    
    
        private void Start()
        {
            ConfigureOrgans();
            HideOtherObjects();
            ShowOrgan();
        }

        private void ConfigureOrgans()
        {
            organs.Insert(0, body);
            foreach (var organ in organs)
            {
                organ.Configure();
            }
        }

        public void NextObject()
        {
            CurrentOrganStoppedBeingFocused();
            _index++;
            if (_index >= organs.Count)
            {
                _index = 0;
            }
            ShowOrgan();
            OnChangeObject?.Invoke();
        }

        private void ShowOrgan()
        {
            //Body in transparent transition
            /*if (_index >= otherOrgans.Length)
        {
            body.SetActive(true);
            HideOtherObjects();
        }
        else
        {
            body.SetActive(false);
            //next object set active true
            otherOrgans[_index].SetActive(true);
        }*/
            HideOtherObjects();
            foreach (var material in organs[_index].materials)
            {
                var color = material.color;
                material.color = new Color(color.r, color.g, color.b, 1);
                material.renderQueue++;
            }
        }

        private void HideOtherObjects()
        {
        
            foreach (var material in organs.SelectMany(organ => organ.materials))
            {
                var color = material.color;
                material.color = new Color(color.r, color.g, color.b, .24f);
                material.renderQueue = 3000;
            }
        }

        public void PreviousObject()
        {
            CurrentOrganStoppedBeingFocused();
            _index--;
            if (_index < 0)
            {
                _index = organs.Count -1;
            }
            ShowOrgan();
            OnChangeObject?.Invoke();
        }

        public GameObject GetCurrentOrgan()
        {
            return organs[_index].gameObject;
        }

        public void CurrentOrganWasFocused()
        {
            organs[_index].ShowOrganCanvas();
        }

        public void CurrentOrganStoppedBeingFocused()
        {
            organs[_index].HideOrganCanvas();
        }

        public void ConfigureOrgansCanvas(Camera getCamera, StateOfGame stateOfGame)
        {
            foreach (var organ in organs)
            {
                stateOfGame.WriteAngel(organ.GetCanvas().worldCamera == null
                    ? "la camara es nula"
                    : organ.GetCanvas().worldCamera.name);
                organ.ConfigureCamera(getCamera);
                stateOfGame.WriteAngel(organ.GetCanvas().worldCamera == null
                    ? "la camara es nula"
                    : organ.GetCanvas().worldCamera.name);
            }
        }
    }
}