using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractableInWord : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject[] otherOrgans;
    private int _index;

    private void Start()
    {
        body.SetActive(true);
        HideOtherObjects();
    }

    public void NextObject()
    {
        ShowOrgan();
        _index++;
        if (_index > otherOrgans.Length - 1)
        {
            _index = 0;
        }
    }

    private void ShowOrgan()
    {
        //Body in transparent transition
        if (_index >= otherOrgans.Length)
        {
            body.SetActive(true);
            HideOtherObjects();
        }
        else
        {
            body.SetActive(false);
            //next object set active true
            otherOrgans[_index].SetActive(true);
        }
    }

    private void HideOtherObjects()
    {
        foreach (var organ in otherOrgans)
        {
            organ.SetActive(false);
        }
    }

    public void PreviousObject()
    {
        ShowOrgan();
        _index--;
        if (_index < 0)
        {
            _index = otherOrgans.Length;
        }
    }
}
