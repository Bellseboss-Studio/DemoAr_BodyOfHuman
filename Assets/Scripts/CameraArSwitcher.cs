using System;
using System.Collections;
using System.Collections.Generic;
using ServiceLocatorPath;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

public class CameraArSwitcher : MonoBehaviour
{
    [SerializeField]
    ARCameraManager m_CameraManager;

    [SerializeField] private ARFaceManager faceManager;
    
    [SerializeField] private ARSessionOrigin sessionOrigin;

    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private List<GameObject> arFaces;

    [SerializeField] private int indexInMenu;
    
    private int indexFaces;
    
    public ARCameraManager cameraManager
    {
        get => m_CameraManager;
        set => m_CameraManager = value;
    }
    
    [SerializeField]
    ARSession m_Session;

    private bool _hasClick = true;
    private bool _isCameraSwitch;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Start()
    {
        //text.text += "inicio\n";
        ARSession.stateChanged += ARSessionOnstateChanged;
        m_CameraManager.requestedFacingDirection = CameraFacingDirection.User;
        ServiceLocator.Instance.GetService<ILoadScream>().Open(() =>{});
        ServiceLocator.Instance.GetService<IMenuManager>().SetIndexDefault(indexInMenu);
    }

    private void ARSessionOnstateChanged(ARSessionStateChangedEventArgs obj)
    {
        text.text += $"{obj.state}\n";
        if (obj.state is ARSessionState.Ready or ARSessionState.SessionInitializing or ARSessionState.SessionTracking)
        {
            faceManager.facePrefab = arFaces[1];
        }
    }

    public ARSession session
    {
        get => m_Session;
        set => m_Session = value;
    }

    public void ChangeModel(int index)
    {
        if (index >= arFaces.Count)
        {
            index = arFaces.Count;
        }
        if(index < 0)
        {
            index = 0;
        }
        faceManager.facePrefab = arFaces[index];
        
        StartCoroutine(ChangeModelDelay());
    }

    private IEnumerator ChangeModelDelay()
    {
        ARSession.stateChanged -= OnChange;  
        m_Session.enabled = false;
        m_Session.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        m_Session.enabled = true;
        m_Session.gameObject.SetActive(true);
        ARSession.stateChanged += OnChange;
    }

    private void OnChange(ARSessionStateChangedEventArgs obj)
    {
        StatesOfGame(obj.state);
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
                break;
            case ARSessionState.Ready:
            case ARSessionState.SessionTracking:
            case ARSessionState.SessionInitializing:
                break;
        }
    }

    public void OnTouch(InputAction.CallbackContext context)
    {
        _hasClick = true;
    }

    public void SwitchCamera()
    {
        m_CameraManager.requestedFacingDirection = _isCameraSwitch ? CameraFacingDirection.World : CameraFacingDirection.User;
        _isCameraSwitch = !_isCameraSwitch;
        ChangeModel(1);
    }

    public void GoHome()
    {
        SceneManager.LoadScene(1);
    }
}