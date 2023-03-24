using System.Collections;
using UnityEngine;

namespace Detection
{
    public class MusicAnalyzer : MonoBehaviour
    {
        // dependency
        private MusicSystem musicSystem;
        [SerializeField] public float updateStep = 0.01f;
        [SerializeField] public int sampleDataLength = 512;
        private float curTimeCount = 0.0f;
        private float[] audioSamples;

        public AudioSource songPlaying;

        private float currentMaxLoudness = 0.0f;
        private float currentAvgLoudness = 0.0f;

        public float currentAvgLoudnessNormalized = 0.0f;

        private void Start()
        {
            musicSystem = FindObjectOfType<MusicSystem>();
            audioSamples = new float[sampleDataLength];

            MusicSystem.UpdatedSongPlaying += UpdateSongPlaying;
        }

        private void UpdateSongPlaying(Sound newSong)
        {
            songPlaying = newSong.source;
        }

        private void Update()
        {
            if (songPlaying == null || songPlaying.clip == null || songPlaying.clip.length == 0) return;

            curTimeCount += Time.deltaTime;
            if (curTimeCount >= updateStep)
            {
                curTimeCount = 0f;
                if (songPlaying.clip.GetData(audioSamples, songPlaying.timeSamples))
                {
                    // reset and recalculate the current sound
                    currentAvgLoudness = 0;
                    currentAvgLoudnessNormalized = 0;
                    currentMaxLoudness = .1f;
                    foreach (float sample in audioSamples)
                    {
                        currentAvgLoudness += Mathf.Abs(sample);

                        if (currentMaxLoudness < sample) currentMaxLoudness = Mathf.Abs(sample);
                    }
                    currentAvgLoudness /= sampleDataLength;


                    const float minLoudness = 0f;
                    currentAvgLoudness = Mathf.Clamp(currentAvgLoudness, minLoudness, currentMaxLoudness);

                    // normalize it from 0 to 1 based on the min max range
                    currentAvgLoudnessNormalized = (currentAvgLoudness - minLoudness) / (currentMaxLoudness - minLoudness);
                }
            }
        }
    }
}