using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Detection
{
    public class BackgroundManager : MonoBehaviour
    {
        [Header("Settings")]
        public Camera cam;
        public List<Color32> colors;
        // Start is called before the first frame update
        void Start()
        {
            cam = transform.GetComponent<Camera>();
            StartCoroutine(Cycle());
        }

        public IEnumerator Cycle()
        {
            int startColorIndex = Random.Range(0, colors.Count);
            int endColorIndex = Random.Range(0, colors.Count);

            while (true)
            {
                for (float interpolant = 0; interpolant < 1f; interpolant += 0.01f)
                {
                    cam.backgroundColor = Color.Lerp(colors[startColorIndex], colors[endColorIndex], interpolant);
                    yield return null;
                }

                startColorIndex = endColorIndex;
                endColorIndex = Random.Range(0, colors.Count);
            }
        }
    }
}
