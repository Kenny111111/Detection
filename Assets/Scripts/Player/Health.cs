using UnityEngine;
using UnityEngine.UI;

namespace Detection
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;
        public Image healthBar;

        void Start()
        {
            maxHealth = currentHealth;
        }

        void Update()
        {
            healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);

            if (currentHealth == 0)
            {
                //display game over menu

            }

        }
    }
}