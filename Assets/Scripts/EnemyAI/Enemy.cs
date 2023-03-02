using UnityEngine;

namespace Detection
{
    public class Enemy : Combatant
    {
        private Animator animator;
        private Rigidbody[] rigidbodies;
        private Collider[] colliders;
        private AIWeaponManager weaponManager;

        private void Awake()
        {
            health = 100f;
            maxHealth = health;
            animator = GetComponent<Animator>();
            colliders = GetComponentsInChildren<Collider>();
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            weaponManager = GetComponent<AIWeaponManager>();

            foreach (Rigidbody rb in rigidbodies)
            {
                rb.gameObject.AddComponent<Hitbox>();
                rb.gameObject.AddComponent<EnemyScannerSurface>();
            }
        }

        private void Start()
        {
            ToggleRagdoll(false);
        }

        public override void Die()
        {
            // Launch weapon towards player or drop on ground if not targeting player
            weaponManager.LaunchWeapon();

            animator.enabled = false;
            ToggleRagdoll(true);

            Destroy(gameObject, 1f);
        }

        private void ToggleRagdoll(bool state)
        {
            foreach (Rigidbody rb in rigidbodies)
            {
                rb.isKinematic = !state;
            }
        }
    }
}