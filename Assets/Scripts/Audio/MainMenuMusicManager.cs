using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Class to play music in the menu
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class MainMenuMusicManager : MonoBehaviour
    {
        /// <summary>
        /// The AudioManager
        /// </summary>
        private AudioManager _audioManager;

        /// <summary>
        /// The intro of the song
        /// </summary>
        public string intro = "MenuIntro";
        /// <summary>
        /// The loop of the song
        /// </summary>
        public string loop = "MenuLoop";

        /// <summary>
        /// Starts this instance and plays the music
        /// </summary>
        private void Start()
        {
            _audioManager = AudioManager.Instance;
            _audioManager.SetMusic(intro, loop);
        }
    }
}
