using UnityEngine;

namespace Detection
{
    public class MenuManager : MonoBehaviour
    {
        // Start is called before the first frame update
        /// <summary>
        public GameObject Canvas;
        /// </summary>
        void Start()
        {
            if (Canvas != null) Canvas.SetActive(false);
            else Debug.Log("Need to set the canvas to an object...");
        }
    }
}
