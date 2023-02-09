using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

public class LoadSceneComponent : MonoBehaviour, ILoadScream
{
    [SerializeField] private Animator anim;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VideoClip open, close;
    private bool finishAnimation;

    private void Awake()
    {
        //videoPlayer.gameObject.SetActive(false);
    }

    public void Finished()
    {
        finishAnimation = true;
    }

    public void NotFinished()
    {
        finishAnimation = false;
    }

    private async void Open()
    {
        anim.SetBool("open",true);
        //Finished();
    }

    public async UniTaskVoid Open(Action a)
    {
        Open();
        await UniTask.Delay(TimeSpan.FromMilliseconds(5000));
        a?.Invoke();
        //NotFinished();
        //videoPlayer.gameObject.SetActive(false);
    }

    private async void Close()
    {
        anim.SetBool("open",false);
        //Finished();
        
    }
    public async UniTaskVoid Close(Action a)
    {
        //videoPlayer.gameObject.SetActive(true);
        Close();
        //Debug.Log($"init loop {finishAnimation}");
        await UniTask.Delay(TimeSpan.FromMilliseconds(5000));
        //Debug.Log($"finish loop {finishAnimation}");
        a?.Invoke();
        //NotFinished();
    }
}