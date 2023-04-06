using UnityEngine;
using Detection;
using System;

public class Enemy : Combatant
{
    private Animator animator;
    private Rigidbody[] rigidbodies;
    private AIWeaponManager weaponManager;
    private AIController aiController;
    public Action<Enemy, IDealsDamage.Weapons, AttackerType> OnDeath;

    public bool isAlive { get; private set; } = true;

    private void Awake()
    {
        health = 100f;
        maxHealth = health;
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        weaponManager = GetComponent<AIWeaponManager>();
        aiController = GetComponent<AIController>();
    }

    private void Start()
    {
        ToggleRagdoll(false);
    }

    public override void Die(IDealsDamage.Weapons weapon, AttackerType attacker)
    {

        // Check if already dead
        if (!isAlive) return;

        // Set isAlive to false
        isAlive = false;

        OnDeath?.Invoke(this, weapon, attacker);

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

    public void Alerted(Vector3 position)
    {
        aiController.Alerted(position);
    }
}