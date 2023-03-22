using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuFacade : MonoBehaviour
    {
        [SerializeField] private GameObject creditsPanel, mainPanel;
        
        public void StartApp()
        {
            SceneManager.LoadScene(2);
        }

        public void ShowCredits()
        {
            creditsPanel.SetActive(true);
            mainPanel.SetActive(false);
        }
        
        public void HideCredits()
        {
            creditsPanel.SetActive(false);
            mainPanel.SetActive(true);
        }
        
        public void Exit()
        {
            Application.Quit();
        }
    }
}