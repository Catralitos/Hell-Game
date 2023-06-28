using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menus
{
    public class WinScreenManager : MonoBehaviour
    {
        public Button creditsButton;

        private void Start()
        {
            creditsButton.onClick.AddListener(ShowCredits);
        }

        private static void ShowCredits()
        {
            SceneManager.LoadScene(4);
        }
        
    }
}