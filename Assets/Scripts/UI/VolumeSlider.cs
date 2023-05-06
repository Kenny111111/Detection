using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

namespace Detection
{
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private AudioMixer volumeSlider;
        public void SetMusicVolume(float MusicSliderVolume)
        {
            volumeSlider.SetFloat("MusicVolume", Mathf.Log10(MusicSliderVolume) * 20);
        }

        public void SetGameVolume(float GameSliderVolume)
        {
            volumeSlider.SetFloat("GameVolume", Mathf.Log10(GameSliderVolume) * 20);
        }
    }
}
