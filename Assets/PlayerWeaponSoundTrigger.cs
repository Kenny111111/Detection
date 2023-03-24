using UnityEngine;


namespace Detection
{
    public class PlayerWeaponSoundTrigger : MonoBehaviour
    {
        [SerializeField] private float radius = 8f;

        void Awake()
        {
            GetComponent<IShootable>().OnShot += DoEnemyCheckForSound;
        }

        void DoEnemyCheckForSound()
        {
            Debug.Log("shoot memeee");
            // find ai around player then call each enemies Alert() function in radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
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