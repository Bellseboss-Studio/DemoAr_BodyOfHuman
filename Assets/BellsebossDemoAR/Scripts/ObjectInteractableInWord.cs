using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectInteractableInWord : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject[] otherOrgans;
    private int _index;
    [SerializeField] private List<Organ> _organs;

    [Serializable]
    public class Organ
    {
        public GameObject Model;
        public List<Material> Materials;

        public Organ(GameObject organ)
        {
            Model = organ;
            var renderers = organ.GetComponentsInChildren<Renderer>().ToList();
            Materials = new List<Material>();
            foreach (var material in renderers.SelectMany(renderer => renderer.materials))
            {
                Materials.Add(material);
            }
        }
    }
    
    private void Start()
    {
        ConfigureOrgans();
        body.SetActive(true);
        HideOtherObjects();
        ShowOrgan();
    }

    private void ConfigureOrgans()
    {
        _organs = new List<Organ> {new(body)};
        foreach (var organ in otherOrgans)
        {
            organ.SetActive(true);
            _organs.Add(new Organ(organ));
        }
    }

    public void NextObject()
    {
        _index++;
        if (_index >= _organs.Count)
        {
            _index = 0;
        }
        ShowOrgan();
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
        foreach (var material in _organs[_index].Materials)
        {
            var color = material.color;
            material.color = new Color(color.r, color.g, color.b, 1);
            material.renderQueue++;
        }
    }

    private void HideOtherObjects()
    {
        
        foreach (var material in _organs.SelectMany(organ => organ.Materials))
        {
            var color = material.color;
            material.color = new Color(color.r, color.g, color.b, .24f);
            material.renderQueue = 3000;
        }
    }

    public void PreviousObject()
    {
        _index--;
        if (_index < 0)
        {
            _index = _organs.Count -1;
        }
        ShowOrgan();
    }
}
