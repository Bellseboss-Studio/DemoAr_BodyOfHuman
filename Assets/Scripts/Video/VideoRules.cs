using System.Collections;
using ServiceLocatorPath;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoRules : MonoBehaviour
{
   [SerializeField] private VideoPlayer video;
   [SerializeField] private VideoClip videoEs, videoEn;
   [SerializeField] private GameObject panelSelectLanguage;

   public void SelectNewLanguage(string language)
   {
      ServiceLocator.Instance.GetService<ILocalization>().ChangeLanguage(language);
      
      ServiceLocator.Instance.GetService<ILoadScream>().Close(() =>
      {
         panelSelectLanguage.SetActive(false);
         ServiceLocator.Instance.GetService<ILoadScream>().Open(() =>
         {
            StartVideo();
         }).Forget();
      }).Forget();
   }
   public void StartVideo()
   {
      video.clip = ServiceLocator.Instance.GetService<ILocalization>().GetLanguage() switch
      {
         "ES" => videoEs,
         "EN" => videoEn,
         _ => video.clip
      };
      video.Play();
      StartCoroutine(FinishVideo());
   }

   IEnumerator FinishVideo()
   {
      //Debug.Log($"start {video.clip.length}");
      yield return new WaitForSeconds((float) video.clip.length + 0.1f);
      //Debug.Log("finish");
      SkipVideo();
   }

   public void SkipVideo()
   {
      ServiceLocator.Instance.GetService<ILoadScream>().Close(() =>
      {
         SceneManager.LoadScene(1);
      }).Forget();
   }
}
