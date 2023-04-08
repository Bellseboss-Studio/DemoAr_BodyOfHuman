using System;
using System.Collections;
using System.Collections.Generic;
using BellsebossDemoAR.Scripts;
using UnityEngine;

public class RulesInExperience : MonoBehaviour
{
    [SerializeField] private StateOfGame stateOfGame;
    [SerializeField] private GameObject ui;
    private ObjectInteractableInWord _objectInteractableInWord;

    private void Start()
    {
        ui.SetActive(false);
        stateOfGame.OnInstantiateElement += () =>
        {
            ui.SetActive(true);
            stateOfGame.GetObjectInstantiate().TryGetComponent(out _objectInteractableInWord);
            _objectInteractableInWord.ConfigureOrgansCanvas(stateOfGame.GetUIEventsCamera(), stateOfGame);
        };
    }

    public void NextModel()
    {
        _objectInteractableInWord?.NextObject();
    }

    public void PreviousModel()
    {
        _objectInteractableInWord?.PreviousObject();
    }
}