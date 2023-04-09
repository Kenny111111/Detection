using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    public class MusicAnalyzer : MonoBehaviour
    {
        [SerializeField] public float updateStep = 0.01f;
        [SerializeField] public int defaultSampleDataLength = 512;
        private int sampleDataLength;
        private float[] audioSamples;

        private float currentMaxLoudness = 0.0f;
        private float currentAvgLoudness = 0.0f;
        public float currentAvgLoudnessNormalized = 0.0f;

        private void Awake()
        {
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

            StartCoroutine(AnalyzeSongPlaying(this, newSong.source));
        }

        private static IEnumerator AnalyzeSongPlaying(MusicAnalyzer analyzer, AudioSource song)
        {
            const float earlyStopAmount = 0.1f;
            float songLength = song.clip.length - earlyStopAmount;

            for (float t = 0f; t < songLength; t += Time.deltaTime)
            {
                if (song.clip.GetData(analyzer.audioSamples, song.timeSamples))
                {
                    // reset and recalculate the current sound
                    analyzer.currentAvgLoudness = 0;
                    analyzer.currentAvgLoudnessNormalized = 0;
                    foreach (float sample in analyzer.audioSamples)
                    {
                        analyzer.currentAvgLoudness += Mathf.Abs(sample);
                    }
                    analyzer.currentAvgLoudness /= analyzer.sampleDataLength;

                    if (analyzer.currentMaxLoudness < analyzer.currentAvgLoudness) analyzer.currentMaxLoudness = analyzer.currentAvgLoudness;

                    const float minLoudness = 0f;
                    analyzer.currentAvgLoudness = Mathf.Clamp(analyzer.currentAvgLoudness, minLoudness, analyzer.currentMaxLoudness);

                    // normalize it from 0 to 1 based on the min max range
                    analyzer.currentAvgLoudnessNormalized = (analyzer.currentAvgLoudness - minLoudness) / (analyzer.currentMaxLoudness - minLoudness);
                    if (double.IsNaN(analyzer.currentAvgLoudnessNormalized)) analyzer.currentAvgLoudnessNormalized = 0;
                }

                yield return new WaitForSeconds(0.01f);
            }
        }

    }
}