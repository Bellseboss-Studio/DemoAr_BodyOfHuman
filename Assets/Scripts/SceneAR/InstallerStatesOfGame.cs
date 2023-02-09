using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class InstallerStatesOfGame : MonoBehaviour, IMediadorAR
{
    [SerializeField] ARSession m_Session;
    [SerializeField] ARRaycastManager m_RaycastManager;
    [SerializeField] private ARSessionOrigin sessionOrigin;
    [SerializeField] private StateOfGame stateOfGame;
    [SerializeField] private ARPlaneManager plane;
    [SerializeField] private ARPointCloudManager point;
    [SerializeField] private ColliderInCamera coli;
    [SerializeField] private Transform player;
    [SerializeField] private int indexInMenu;
    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
    private bool _hasClick;
    private Vector2 _mousePosition;
    private GameObject spawnedObject;

    public IEnumerator Start()
    {
        
        stateOfGame.Write($"{ARSession.state}");
        if (ARSession.state is ARSessionState.None or ARSessionState.CheckingAvailability)
        {
            yield return ARSession.CheckAvailability();
        }
        stateOfGame.Write($"{ARSession.state}");
        if (ARSession.state == ARSessionState.Unsupported)
        {
            // Start some fallback experience for unsupported devices
        }
        else
        {
            m_Session.enabled = true;
            StatesOfGame(ARSession.state);
            ARSession.stateChanged += onChange;
        }
        //ServiceLocator.Instance.GetService<IMenuManager>().SetIndexDefault(indexInMenu);

    }


    void onChange(ARSessionStateChangedEventArgs eventArgs)
    {
        StatesOfGame(eventArgs.state);
    }

    private void StatesOfGame(ARSessionState eventArgs)
    {
        switch (eventArgs)
        {
            case ARSessionState.None:
            case ARSessionState.Unsupported:
            case ARSessionState.CheckingAvailability:
            case ARSessionState.NeedsInstall:
            case ARSessionState.Installing:
                stateOfGame.Write($"{eventArgs} here is: None, unsuporeted, installing");
                stateOfGame.Restart();
                break;
            case ARSessionState.Ready:
            case ARSessionState.SessionTracking:
            case ARSessionState.SessionInitializing:
                stateOfGame.Write($"{eventArgs} here is: ready, traking, initializing");
                stateOfGame.Write($"configurando");
                stateOfGame.Configuracion(this);
                coli.Configurate(stateOfGame);
                coli.OnCollisionEnterDelegate += () => { };
                break;
        }
    }

    public void StartSession()
    {
        stateOfGame.Write($"StartSession");
    }

    public Camera GetSessionOrigin()
    {
        return sessionOrigin.camera;
    }

    public GameObject InstantiateObjectInRaycast(Vector2 pointToRay, GameObject prefab)
    {
        if (m_RaycastManager.Raycast(pointToRay,s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;
            stateOfGame.Write($"{hitPose.position} pose");
            if(spawnedObject == null)
            {
                spawnedObject = Instantiate(prefab);
            }
            spawnedObject.transform.position = hitPose.position;
            spawnedObject.transform.rotation = hitPose.rotation;
            
            stateOfGame.Write($"{spawnedObject.transform.position} spawned");
            
            return spawnedObject;
        }

        throw new Exception("no instancio nada");
    }

    

    public ARRaycastManager GetRayCastManager()
    {
        return m_RaycastManager;
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        stateOfGame.Write($"{context.ReadValue<Vector2>()}");
        _mousePosition = context.ReadValue<Vector2>();
    }
    
    public void OnTouch(InputAction.CallbackContext context)
    {
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
        stateOfGame.Write($"_hasClick {_hasClick}");
    }
    public bool Touch()
    {
        return _hasClick;// Input.touchCount > 0;
    }

    public void HideDebuggers()
    {
        plane.enabled = false;
        point.enabled = false;
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
        //Destroy(m_Session.gameObject);
        //Destroy(sessionOrigin.gameObject);
        m_Session.gameObject.SetActive(false);
    }

    public void Repetir()
    {
        SceneManager.LoadScene(0);
    }
}