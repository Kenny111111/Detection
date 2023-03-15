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
            Canvas.SetActive(false);
        }
    }
}
