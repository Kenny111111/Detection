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
        public void SetVolume(float sliderVolume)
        {
            volumeSlider.SetFloat("Volume", Mathf.Log10(sliderVolume) * 20);
        }
    }
}
