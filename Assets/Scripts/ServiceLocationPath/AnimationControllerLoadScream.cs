using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControllerLoadScream : MonoBehaviour
{
    [SerializeField] private LoadSceneComponent loadSceneComponent;

    public void FinishAnimation()
    {
        loadSceneComponent.Finished();
    }

    public void NotFinish()
    {
        loadSceneComponent.NotFinished();
    }
}
