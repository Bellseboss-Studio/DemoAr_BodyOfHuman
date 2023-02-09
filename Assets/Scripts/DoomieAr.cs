using System;
using ServiceLocatorPath;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class DoomieAr : MonoBehaviour, IMediadorAR
{
    [SerializeField] private Transform hitPose;
    [SerializeField] private StateOfGame state;
    [SerializeField] private Transform player;
    private bool _hasClick;
    private Vector2 _mousePosition;

    private void Start()
    {
        state.Configuracion(this);
    }

    public void StartSession()
    {
        
    }

    public Camera GetSessionOrigin()
    {
        return Camera.main;
    }

    public GameObject InstantiateObjectInRaycast(Vector2 pointToRay, GameObject prefab)
    {
        
        var spawnedObject = Instantiate(prefab);
        spawnedObject.transform.position = hitPose.position;
        spawnedObject.transform.rotation = hitPose.rotation;
        return spawnedObject;
    }

    public ARRaycastManager GetRayCastManager()
    {
        throw new NotImplementedException();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        state.Write($"{context.ReadValue<Vector2>()}");
        _mousePosition = context.ReadValue<Vector2>();
    }
    public void OnTouch(InputAction.CallbackContext context)
    {
        state.Write($"Click");
        if (context.started)
        {
            //button is press
            _hasClick = true;
        }
        else if (context.canceled)
        {
            //button is released
            _hasClick = false;
        }
    }
    public bool Touch()
    {
        return _hasClick;//Input.GetMouseButtonDown(0);                                
    }

    public void HideDebuggers()
    {
        
    }

    public Vector2 GetMousePosition()
    {
        return _mousePosition;
    }
    
    public Transform GetPlayer()
    {
        return player;
    }

    public void ResetSession()
    {
        
    }
}