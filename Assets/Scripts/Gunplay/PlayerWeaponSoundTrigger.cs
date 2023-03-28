using UnityEngine;


namespace Detection
{
    public class PlayerWeaponSoundTrigger : MonoBehaviour
    {
        // Later we want to make OnShot return the weapon var on the weapons max hearing radius
        [SerializeField] private float maxWeaponHearingRadius = 20f;

        void Awake()
        {
            GetComponent<IShootable>().OnShot += DoEnemyCheckForSound;
        }

        void DoEnemyCheckForSound()
        {
            // find ai around player then call each enemies Alert() function in radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, maxWeaponHearingRadius);
            foreach (Collider collider in colliders)
            {
                AIController enemy = collider.GetComponent<AIController>();
                if (enemy != null)
                {
                    Vector3 soundPos = transform.position;
                    enemy.Alerted(soundPos);
                }
            }

        }
    }
}
