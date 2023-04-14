using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoRules : MonoBehaviour
{
   [SerializeField] private VideoPlayer video;
   [SerializeField] private VideoClip videoEs;

   private void Start()
   {
      /*SkipVideo();*/
      StartVideo();
   }

   private void StartVideo()
   {
      video.clip = videoEs;
      video.Play();
      StartCoroutine(FinishVideo());
   }

   IEnumerator FinishVideo()
   {
      //Debug.Log($"start {video.clip.length}");
      yield return new WaitForSeconds((float) video.clip.length + 0.5f);
      //Debug.Log("finish");
      SkipVideo();
   }

   public void SkipVideo()
   {
      SceneManager.LoadScene(1);
   }
}