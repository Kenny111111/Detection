using UnityEngine;
using System.Collections.Generic;


namespace Detection
{
    public class PlayerWeaponSoundTrigger : MonoBehaviour
    {
        // Later we want to make OnShot return the weapon var on the weapons max hearing radius
        [SerializeField] private float maxWeaponHearingRadius = 10f;

        void Awake()
        {
            GetComponent<IShootable>().OnShot += DoEnemyCheckForSound;
        }

        void DoEnemyCheckForSound()
        {
            // find ai around player then call each enemies Alert() function in radius
            List<Enemy> enemiesInRange = new List<Enemy>();

            foreach (KeyValuePair<Enemy, GameObject> entry in EnemyManager.instance.enemies)
            {
                if (Vector3.Distance(transform.position, entry.Value.transform.position) <= maxWeaponHearingRadius)
                {
                    enemiesInRange.Add(entry.Key);
                }
            }

            foreach (Enemy enemy in enemiesInRange)
            {
                if (enemy != null)
                {
                    Vector3 soundPos = transform.position;
                    enemy.Alerted(soundPos);
                }  
            }
        }
    }
}
