using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class WinScreenManager : MonoBehaviour
    {
        public Button creditsButton;

        private AudioManager _audioManager;
        
        private void Start()
        {
            creditsButton.onClick.AddListener(ShowCredits);
            _audioManager = GetComponent<AudioManager>();
            _audioManager.Play("MenuMusic");
        }

        private static void ShowCredits()
        {
            SceneManager.LoadScene(5);
        }
        
    }
}