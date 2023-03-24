using UnityEngine;


namespace Detection
{
    public class PlayerWeaponSoundTrigger : MonoBehaviour
    {
        void Awake()
        {
            GetComponent<IShootable>().OnShot += DoEnemyCheckForSound;
        }

        void DoEnemyCheckForSound()
        {
            Debug.Log("shoot memeee");
            // find ai around player then call each enemies Alert() function in radius
        }
    }
}