using System;
using System.Collections;
using System.Collections.Generic;
using BellsebossDemoAR.Scripts;
using UnityEngine;

public class RulesInExperience : MonoBehaviour
{
    [SerializeField] private StateOfGame stateOfGame;
    private ObjectInteractableInWord _objectInteractableInWord;

    private void Start()
    {
        stateOfGame.OnInstantiateElement += () =>
        {
            stateOfGame.GetObjectInstantiate().TryGetComponent(out _objectInteractableInWord);
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