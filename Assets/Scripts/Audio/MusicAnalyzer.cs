using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    public class MusicAnalyzer : MonoBehaviour
    {
        public static MusicAnalyzer instance;
        [SerializeField] public float updateStep = 0.01f;
        [SerializeField] public int defaultSampleDataLength = 512;
        private int sampleDataLength;
        private float[] audioSamples;

        private float currentMaxLoudness = 0.0f;
        private float currentAvgLoudness = 0.0f;
        public float currentAvgLoudnessNormalized = 0.0f;
        private bool analyzingCoroutineRunning = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else Destroy(gameObject);

            sampleDataLength = defaultSampleDataLength;
            audioSamples = new float[sampleDataLength];

            MusicSystem.UpdatedSongPlaying += UpdateSongPlaying;
        }

        private void OnDestroy()
        {
            MusicSystem.UpdatedSongPlaying -= UpdateSongPlaying;
        }

        private void UpdateSongPlaying(Sound newSong)
        {
            sampleDataLength = defaultSampleDataLength * newSong.clip.channels;
            audioSamples = new float[sampleDataLength];

            if (analyzingCoroutineRunning)
            {
                StopCoroutine("AnalyzeSongPlaying");
                analyzingCoroutineRunning = false;
            }
            StartCoroutine(AnalyzeSongPlaying(newSong.source));
        }

        private static IEnumerator AnalyzeSongPlaying(AudioSource song)
        {
            instance.analyzingCoroutineRunning = true;

            const float earlyStopAmount = 0.1f;
            float songLength = song.clip.length - earlyStopAmount;

            for (float t = 0f; t < songLength; t += Time.deltaTime)
            {
                if (song.isPlaying)
                {
                    if (song.clip.GetData(instance.audioSamples, song.timeSamples))
                    {
                        // reset and recalculate the current sound
                        instance.currentAvgLoudness = 0;
                        instance.currentAvgLoudnessNormalized = 0;
                        foreach (float sample in instance.audioSamples)
                        {
                            instance.currentAvgLoudness += Mathf.Abs(sample);
                        }
                        instance.currentAvgLoudness /= instance.sampleDataLength;

                        if (instance.currentMaxLoudness < instance.currentAvgLoudness) instance.currentMaxLoudness = instance.currentAvgLoudness;

                        const float minLoudness = 0f;
                        instance.currentAvgLoudness = Mathf.Clamp(instance.currentAvgLoudness, minLoudness, instance.currentMaxLoudness);

                        // normalize it from 0 to 1 based on the min max range
                        instance.currentAvgLoudnessNormalized = (instance.currentAvgLoudness - minLoudness) / (instance.currentMaxLoudness - minLoudness);
                        if (double.IsNaN(instance.currentAvgLoudnessNormalized)) instance.currentAvgLoudnessNormalized = 0;
                    }
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

    }
}