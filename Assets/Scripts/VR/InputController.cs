using System;
using System.Collections;
using UnityEngine;

namespace Detection
{
    [RequireComponent(typeof(LookScanner))]
    public class InputController : MonoBehaviour
    {
        private Camera cam;
        private Vector2 aimPos;
        private LookScanner mainScanner;
        [SerializeField] private int tempOffsetX;
        [SerializeField] private int tempOffsetY;

        // Start is called before the first frame update
        void Start()
        {
            cam = gameObject.GetComponent(typeof(Camera)) as Camera;
            mainScanner = gameObject.GetComponent(typeof(LookScanner)) as LookScanner;
            if (mainScanner == null) Debug.LogError("Could not find LookScanner");

            UpdateAimPos();
            StartCoroutine(ConstantMainScanner());
        }

        void UpdateAimPos()
        {
            Ray eyePos = cam.ScreenPointToRay(new Vector3((Screen.width / 2), (Screen.height / 2), 0));
            aimPos.x = eyePos.direction.x + tempOffsetX;
            aimPos.y = eyePos.direction.y + tempOffsetY;
        }



        // Update is called once per frame
        void Update()
        {
            UpdateAimPos();
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