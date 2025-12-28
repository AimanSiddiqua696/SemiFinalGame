using System;
using System.Collections.Generic;
using System.Media;
using System.Threading.Tasks;

namespace SemiFinalGame.Sound
{
    public class SoundManager
    {
        private SoundPlayer backgroundMusic;
        private Dictionary<string, string> soundEffects; // name -> file path

        public SoundManager()
        {
            soundEffects = new Dictionary<string, string>();
        }

        // Load background music
        public void SetBackgroundMusic(string filePath)
        {
            backgroundMusic = new SoundPlayer(filePath);
        }

        // Play background music (loop)
        public void PlayBackgroundMusic()
        {
            if (backgroundMusic != null)
                Task.Run(() => backgroundMusic.PlayLooping());
        }

        public void StopBackgroundMusic()
        {
            backgroundMusic?.Stop();
        }

        // Add a sound effect
        public void AddSoundEffect(string name, string filePath)
        {
            if (!soundEffects.ContainsKey(name))
                soundEffects.Add(name, filePath);
        }

        // Play a sound effect
        public void PlaySoundEffect(string name)
        {
            if (!soundEffects.ContainsKey(name))
                return;

            Task.Run(() =>
            {
                using (SoundPlayer sfx = new SoundPlayer(soundEffects[name]))
                {
                    sfx.Play();
                }
            });
        }
    }
}
