using System.Collections;
using UnityEngine;

namespace Detection
{
    public class MusicAnalyzer : MonoBehaviour
    {
        // dependency
        private MusicSystem musicSystem;
        [SerializeField] public float minLoudness = 0.0f;
        [SerializeField] public float maxLoudness = 250;
        [SerializeField] public float updateStep = 0.01f;
        [SerializeField] public int sampleDataLength = 512;
        [SerializeField] public float sizeFactor = 1.0f;
        private float curTimeCount = 0.0f;
        private float[] audioSamples;

        public AudioSource songPlaying;

        // This represents the current song Loudness at any point in the game.
        public float currentLoudness = 0.0f;

        private void Start()
        {
            musicSystem = FindObjectOfType<MusicSystem>();
            audioSamples = new float[sampleDataLength];

            StartCoroutine(UpdateSongPlaying(this, musicSystem));
        }

        private static IEnumerator UpdateSongPlaying(MusicAnalyzer analyzer, MusicSystem musicSystem)
        {
            while (true)
            {
                Sound soundPlaying;
                if (musicSystem.musicQueue.TryPeek(out soundPlaying))
                {
                    analyzer.songPlaying = soundPlaying.source;
                }

                yield return new WaitForSeconds(1);
            }
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
                    currentLoudness = 0;
                    foreach (float sample in audioSamples)
                    {
                        currentLoudness += Mathf.Abs(sample);
                    }
                    currentLoudness /= sampleDataLength;

                    currentLoudness *= sizeFactor;
                    currentLoudness = Mathf.Clamp(currentLoudness, minLoudness, maxLoudness);
                }
            }
        }
    }
}