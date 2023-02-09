using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ServiceLocatorPath;
using TMPro;
using UnityEngine;

class Localization : ILocalization, ISoundService
{
    private Dictionary<string, Dictionary<string, string>> _localization;
    private Dictionary<string, Dictionary<string, AudioClip>> _audioClips;
    private string _currentLanguage;
    private bool _canSeeSubtitle;
    private Action UpdateTextForNewLanguage;
    public Localization(string fileName, SystemLanguage language, bool getLocalLanguage, char separator, List<AudioClip> clipsEs, List<AudioClip> clipsEn)
    {
        _localization = new Dictionary<string, Dictionary<string, string>>();
        var dataset = Resources.Load<TextAsset>(fileName);
        // Splitting the dataset in the end of line
        var splitDataset = dataset.text.Split(new char[] {'\n'});
        for (var i = 1; i < splitDataset.Length; i++) {
            try
            {
                string[] row = splitDataset[i].Split(separator);
                //Debug.Log($"id={row[0]}, ES={row[1]} EN={row[2]}");
                _localization.Add(row[0], new Dictionary<string, string>() {
                    { "ES", row[1] },
                    { "EN", row[2] }
                });
            }catch(Exception){}
        }
        ChangeLanguage(getLocalLanguage ? Application.systemLanguage : language);
        
        _audioClips = new Dictionary<string, Dictionary<string, AudioClip>>();
        _audioClips.Add("ES", new Dictionary<string, AudioClip>());
        _audioClips.Add("EN", new Dictionary<string, AudioClip>());
        
        foreach (var clip in clipsEs)
        {
            _audioClips["ES"].Add(clip.name, clip);
        }
        
        foreach (var clip in clipsEn)
        {
            _audioClips["EN"].Add(clip.name, clip);
        }
    }

    public string GetWord(string key)
    {
        key = key.Trim();
        return !_localization.ContainsKey(key) ? "Not Found" : _localization[key][_currentLanguage];
    }

    public void ChangeLanguage(SystemLanguage language)
    {
        _currentLanguage = language switch
        {
            SystemLanguage.Spanish => "ES",
            SystemLanguage.English => "EN",
            _ => "EN"
        };
    }

    public string GetLanguage()
    {
        return _currentLanguage;
    }

    public async UniTaskVoid StartSubtitle(AudioSource audioSource, TextMeshProUGUI textOfSubtitles, GameObject panel)
    {
        audioSource.Play();
        var canSeeSubtitle = ServiceLocator.Instance.GetService<ISoundService>().CanSeeSubtitle();
        panel.SetActive(canSeeSubtitle);
        //obteniendo los objetos de los subtitulos
        panel.SetActive(false);
    }

    public void ChangeLanguage(string language)
    {
        _currentLanguage = language;
        UpdateTextForNewLanguage?.Invoke();
    }

    public string GetWordUpdate(string key, Action action)
    {
        UpdateTextForNewLanguage += action;
        return GetWord(key);
    }

    public void UnsubscribeGetWordUpdate(Action action)
    {
        UpdateTextForNewLanguage -= action;
    }

    public AudioClip GetAudio(string audioClipName)
    {
        return _currentLanguage switch
        {
            "ES" => _audioClips["ES"][audioClipName],
            "EN" => _audioClips["EN"][audioClipName],
            _ => null
        };
    }

    public void CanSeeSubtitleAgain()
    {
        _canSeeSubtitle = !_canSeeSubtitle;
        Debug.Log($"Can see subtitle? {_canSeeSubtitle}");
    }

    public bool CanSeeSubtitle()
    {
        return _canSeeSubtitle;
    }
}