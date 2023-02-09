using System.Collections.Generic;
using UnityEngine;

interface ISoundService
{
    AudioClip GetAudio(string audioClipName);
    void CanSeeSubtitleAgain();
    bool CanSeeSubtitle();
}