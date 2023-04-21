using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class BackgroundManager : MonoBehaviour
    {
        [Header("Settings")]
        private Camera cam;
        public List<Color32> colors;
        // Start is called before the first frame update
        void Start()
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            StartCoroutine(Cycle());
        }

        public IEnumerator Cycle()
        {
            int startColorIndex = Random.Range(0, colors.Count);
            int endColorIndex = Random.Range(0, colors.Count);
            const float loudnessMultiplier = 0.5f;

            while (true)
            {
                float interpolant = 0;
                while (interpolant < 1f)
                {
                    cam.backgroundColor = Color.Lerp(colors[startColorIndex], colors[endColorIndex], interpolant);

                    float interpMultiplier = 1 + (MusicAnalyzer.instance.currentAvgLoudnessNormalized * loudnessMultiplier);
                    
                    interpolant += 0.005f * interpMultiplier;
                    yield return null;
                }

                startColorIndex = endColorIndex;
                endColorIndex = Random.Range(0, colors.Count);
            }
        }
    }
}
