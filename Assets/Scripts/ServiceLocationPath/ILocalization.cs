using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public interface ILocalization
{
    string GetWord(string key);
    void ChangeLanguage(SystemLanguage language);
    string GetLanguage();
    UniTaskVoid StartSubtitle(AudioSource audioSource, TextMeshProUGUI textOfSubtitles, GameObject panelToSubtitle);
    void ChangeLanguage(string language);
    string GetWordUpdate(string key, Action action);
    void UnsubscribeGetWordUpdate(Action action);
}