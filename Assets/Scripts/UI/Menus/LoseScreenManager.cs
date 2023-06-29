using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class LoseScreenManager : MonoBehaviour
    {
        [Header("Buttons")] public Button retryButton;
        public Button backToTitleButton;

        private AudioManager _audioManager;
        
        private void Start()
        {
            retryButton.onClick.AddListener(RetryGame);
            backToTitleButton.onClick.AddListener(BackToTitle);
            _audioManager = GetComponent<AudioManager>();
            _audioManager.Play("MenuMusic");
        }

        private static void RetryGame()
        {
            SceneManager.LoadScene(2);
        }

        private static void BackToTitle()
        {
            SceneManager.LoadScene(1);
        }
        
    }
}