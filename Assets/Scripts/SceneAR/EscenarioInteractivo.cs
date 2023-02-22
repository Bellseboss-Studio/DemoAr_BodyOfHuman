
using System.Collections.Generic;
using System.Linq;
using ServiceLocatorPath;
using TMPro;
using UnityEngine;

public class EscenarioInteractivo : MonoBehaviour, IEscenarioInteractivo
{
    [SerializeField] private List<EstacionInteractiva> estacionesInteractivas;
    [SerializeField] private List<OpenURL> urls;
    [SerializeField] private AudioSource source, outroSound;
    [SerializeField] private TextMeshProUGUI texto;
    [SerializeField] private GameObject panel;
    [SerializeField] private bool IsNotInteractive;
    private bool isPlaying;
    
    private IMediator _mediator;

    public void Configuracion(Camera camera, IMediator mediator, Transform player)
    {
        _mediator = mediator;
        if (IsNotInteractive) return;
        foreach (var estacionInteractiva in estacionesInteractivas)
        {
            estacionInteractiva.Configuracion(camera, _mediator, player, this);
            estacionInteractiva.OnFinishView +=OnFinishView;
            estacionInteractiva.OnCollisionEnterCustom += OnCollisionEnterCustom;
        }
        var positionOfCamera = camera.gameObject.transform.position;
        transform.position = new Vector3(positionOfCamera.x, transform.position.y, positionOfCamera.z);
        foreach (var url in urls)
        {
            url.gameObject.SetActive(false);
        }

        outroSound.clip = ServiceLocator.Instance.GetService<ISoundService>().GetAudio(outroSound.clip.name);
        outroSound.Play();
        //Aqui configuramos los subtitulos
        //ServiceLocator.Instance.GetService<ILocalization>().StartSubtitle(outroSound, texto, panel);
    }

    private void OnCollisionEnterCustom()
    {
        outroSound.Stop();
        source.Stop();
    }

    private void OnFinishView()
    {
        //change audio for the correct language
        source.clip = ServiceLocator.Instance.GetService<ISoundService>().GetAudio(source.clip.name);
        if (isPlaying) return;
        var allFinished = true;
        foreach (var estacionInteractiva in estacionesInteractivas.Where(estacionInteractiva => !estacionInteractiva.HasUse))
        {
            allFinished = false;
        }

        if (!allFinished) return;
        foreach (var url in urls)
        {
            url.gameObject.SetActive(true);
        }
        source.Play();
        isPlaying = true;
    }

    public void StopAudioGeneral()
    {
        source.Stop();
    }

    public GameObject GetPanel()
    {
        return panel;
    }

    public TextMeshProUGUI GetTextMesh()
    {
        return texto;
    }
}