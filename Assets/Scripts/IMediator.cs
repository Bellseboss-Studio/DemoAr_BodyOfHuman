using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public interface IMediator
{
    void Write(string text);
    bool HasClickInScream();
    bool ShootRaycast(Action action);
    bool FinishGame();
    bool HasWait();
    void ConfigureShooter();
    Camera GetSessionOrigin();
    void HideDebuggers();
    Camera GetCamera();
    Vector3 GetMousePositionInScream();
    UniTaskVoid ShowTrackingAnimator();
    UniTaskVoid ShowLookAroundAnimator();
    bool RespawnScene();
    void RestartAllObjectsToRespawn();
    void StopAudioGeneral();
}