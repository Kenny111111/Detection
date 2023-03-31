using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    [RequireComponent(typeof(LookScanner))]
    public class InputController : MonoBehaviour
    {
        [Header("Settings")]
        public Camera cam;

        private Vector2 aimPos;
        private LookScanner mainScanner;
        [SerializeField] private int tempOffsetX;
        [SerializeField] private int tempOffsetY;

        // Start is called before the first frame update
        void Start()
        {
            mainScanner = gameObject.GetComponent(typeof(LookScanner)) as LookScanner;
            if (mainScanner == null) Debug.LogError("Could not find LookScanner");
            StartCoroutine(ConstantMainScanner());
        }

        // Update is called once per frame
        void Update()
        {
            // for some reason this isnt actually a centered vector for our view... I decided to use temporary offsets until this is fixed..
            Ray eyePos = cam.ScreenPointToRay(new Vector3((Screen.width / 2), (Screen.height / 2), 0));
            aimPos.x = eyePos.direction.x + tempOffsetX;
            aimPos.y = eyePos.direction.y + tempOffsetY;
        }

        IEnumerator ConstantMainScanner()
        {
            while (true)
            {
                mainScanner.Scan(aimPos);
                yield return null;
            }
        }
    }
}