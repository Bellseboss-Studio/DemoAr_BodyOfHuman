using System;
using System.Collections;
using System.Collections.Generic;
using ServiceLocatorPath;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EstacionInteractiva : MonoBehaviour
{
    public ColliderInCamera.OnCollision OnFinishView;
    public ColliderInCamera.OnCollision OnCollisionEnterCustom;
    [SerializeField] private Animator anim, animDeContenido, animColibri;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image iconoDeVideo, IconoDeAudio;
    [SerializeField] private Sprite imagenDeVideo;
    [SerializeField] private VideoClip videoParaMostrarVideo;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Canvas canvasCanvas;
    [SerializeField] private List<Outline> outline;
    [SerializeField] private Color colorOutLineOff;
    [SerializeField] private TextMeshProUGUI subtitle;
    [SerializeField] private GameObject panelToSubtitle;
    private IMediator _mediator;
    private bool _canuse;
    private Transform _targetForLookAt;
    private bool hasUse;
    private IEscenarioInteractivo _escenarioInteractivo;

    public bool HasUse => hasUse;
    public void Configuracion(Camera camera1, IMediator mediator, Transform targetForLookAt, IEscenarioInteractivo escenarioInteractivo)
    {
        _mediator = mediator;
        _escenarioInteractivo = escenarioInteractivo;
        canvas.SetActive(false);
        
        canvasCanvas.worldCamera = camera1;
        
        iconoDeVideo.sprite = imagenDeVideo;
        _canuse = true;
        _targetForLookAt = targetForLookAt;
        if (audioClip == null)
        {
            audioSource.gameObject.SetActive(false);
            IconoDeAudio.gameObject.SetActive(false);
        }
        else
        {
            var clip = ServiceLocator.Instance.GetService<ISoundService>().GetAudio(audioClip.name);
            //Debug.Log(clip.name);
            audioSource.clip =
                ServiceLocator.Instance.GetService<ISoundService>().GetAudio(audioClip.name); //audioClip;
        }

        if (videoParaMostrarVideo == null)
        {
            iconoDeVideo.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_canuse) return;
        //transform.LookAt(_targetForLookAt);
    }

    public void ButtonVideo()
    {
        _mediator.Write("click in video");
        ServiceLocator.Instance.GetService<IShowVideo>().Play(videoParaMostrarVideo, () =>
        {
            audioSource.Pause();
        }, () =>
        {
            audioSource.Play();
        });
    }

    public void ButtonAudio()
    {
        StartCoroutine(StopAudioClips());
    }
    public void ExitButton()
    {
        //Debug.Log("click exit");
        animDeContenido.SetBool("showText", false);
        animDeContenido.SetBool("showVideo", false);
        animDeContenido.SetBool("showAudio", false);
        anim.gameObject.SetActive(true);
        anim.SetBool("open", true);
    }
    
    public void ButtonNext()
    {
        //Debug.Log("click next");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        canvas.SetActive(true);
        anim.SetBool("open", true);
        animColibri.SetBool("open", true);
        if (hasUse) return;
        _mediator.StopAudioGeneral();
    }

    private void OnTriggerExit(Collider other)
    {
        anim.SetBool("open", false);
        animColibri.SetBool("open", false);
        StartCoroutine(CloseCanvas());
        audioSource.Stop();
        hasUse = true;
        ServiceLocator.Instance.GetService<IShowVideo>().Stop();
        foreach (var outline1 in outline)
        {
            
        }
        OnFinishView?.Invoke();
    }

    IEnumerator CloseCanvas()
    {
        yield return new WaitForSeconds(.5f);
        canvas.SetActive(false);
    }

    IEnumerator StopAudioClips()
    {
        OnCollisionEnterCustom?.Invoke();
        yield return new WaitForSeconds(.2f);
        try
        {
            audioSource.Play();
            //ServiceLocator.Instance.GetService<ILocalization>().StartSubtitle(audioSource, _escenarioInteractivo.GetTextMesh(), _escenarioInteractivo.GetPanel()).Forget();
        }
        catch (Exception e)
        {
            //ignored
        }
    }
}