using UnityEngine;
using Detection;

public class Enemy : Combatant
{
    private Animator animator;
    private Rigidbody[] rigidbodies;
    private AIWeaponManager weaponManager;

    private void Awake()
    {
        health = 100f;
        maxHealth = health;
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        weaponManager = GetComponent<AIWeaponManager>();
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
        // disable animator when ragdolling
        if (state)
            animator.enabled = false;

        foreach(Rigidbody rb in rigidbodies)
            rb.isKinematic = !state;
    }
}