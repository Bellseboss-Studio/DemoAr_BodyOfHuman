using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public interface IMediator
{
    void Write(string text);
    void StartSessionOfAR();
    bool HasClickInScream();
    bool ShootRaycast(Action action);
    bool FinishGame();
    void ConfiguraEnemigoSpawner();
    void RestarVida(int i);
    bool HasWait();
    void StopSpawnAndDestroidAll();
    void ShowTheGameOver();
    void ConfigureShooter();
    ARRaycastManager GetRayCastManager();
    Camera GetSessionOrigin();
    void HideDebuggers();
    Camera GetCamera();
    Vector3 GetMousePositionInScream();
    UniTaskVoid ShowTrackingAnimator();
    UniTaskVoid ShowLookAroundAnimator();
    void ShowButtonRespawn();
    bool RespawnScene();
    void RestartAllObjectsToRespawn();
    void StopAudioGeneral();
}