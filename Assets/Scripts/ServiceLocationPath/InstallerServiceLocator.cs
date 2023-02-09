using System.Collections.Generic;
using UnityEngine;

namespace ServiceLocatorPath
{
    public class InstallerServiceLocator : MonoBehaviour
    {
        [SerializeField] private LoadSceneComponent loadSceneComponent;
        [SerializeField] private ShowVideo showVideo;
        [SerializeField] private string nameOfTableLanguage;
        [SerializeField] private SystemLanguage defaultLanguage;
        [SerializeField] private bool usageLocalLanguage;
        [SerializeField] private char separator;
        [SerializeField] private List<AudioClip> audioClipsEs, audioClipsEn;
        private void Awake()
        {
            if (FindObjectsOfType<InstallerServiceLocator>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }

            var menumanager = new MenuManager();
            ServiceLocator.Instance.RegisterService<ILoadScream>(loadSceneComponent);
            ServiceLocator.Instance.RegisterService<IShowVideo>(showVideo);
            var locazation = new Localization(nameOfTableLanguage, defaultLanguage, usageLocalLanguage, separator, audioClipsEs, audioClipsEn);
            ServiceLocator.Instance.RegisterService<ILocalization>(locazation);
            ServiceLocator.Instance.RegisterService<ISoundService>(locazation);
            ServiceLocator.Instance.RegisterService<IMenuManager>(menumanager);
            DontDestroyOnLoad(gameObject);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }
}