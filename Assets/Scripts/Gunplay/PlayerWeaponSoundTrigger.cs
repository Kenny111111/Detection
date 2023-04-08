using UnityEngine;
using System.Collections.Generic;


namespace Detection
{
    public class PlayerWeaponSoundTrigger : MonoBehaviour
    {
        private IShootable shootable;
        // Later we want to make OnShot return the weapon var on the weapons max hearing radius
        [SerializeField] private float maxWeaponHearingRadius = 15f;

        void Start()
        {
            shootable = gameObject.GetComponent<IShootable>();
            if (shootable != null) shootable.OnShot += DoEnemyCheckForSound;
        }

        void OnDestroy()
        {
            if (shootable != null) shootable.OnShot -= DoEnemyCheckForSound;
        }

        void DoEnemyCheckForSound()
        {
            if (gameObject != null)
            {
                // find ai around player then call each enemies Alert() function in radius
                foreach (KeyValuePair<Enemy, GameObject> entry in EnemyManager.instance.GetActiveEnemies())
                {
                    if (Vector3.Distance(transform.position, entry.Value.transform.position) <= maxWeaponHearingRadius)
                    {
                        if (entry.Key != null && entry.Value != null)
                        {
                            Vector3 soundPos = transform.position;
                            entry.Key.Alerted(soundPos);
                        }
                    }
                }
            }
        }
    }
}
