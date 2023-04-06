using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using static Detection.IDealsDamage;
using System;

namespace Detection
{
    public class DoubleBarrelShotgun : Weapon, IShootable, IShootsParticle, IDealsDamage
    {
        [SerializeField] private Color bulletColor = new Color(240, 208, 81);
        [SerializeField] private float bulletLifetime = 0.5f;
        [SerializeField] private float bulletSize = 0.2f;

        [SerializeField] private Transform bulletSpawn;
        [SerializeField] private int pelletsPerShot = 7;
        [SerializeField] private float spread;
        private int currentAmmo;
        private float nextShot = 0f;

        // bullet trail created
        public LineRenderer bulletTrail;

        public event Action OnShot;

        private void SpawnBulletTrail(Vector3 hitPoint)
        {
            GameObject bulletTrailEffect = Instantiate(bulletTrail.gameObject, bulletSpawn.position, Quaternion.identity);

            LineRenderer lineR = bulletTrailEffect.GetComponent<LineRenderer>();

            lineR.SetPosition(0, bulletSpawn.position);
            lineR.SetPosition(1, hitPoint);

            Destroy(bulletTrailEffect, 1f);
        }
        //end of bullet trail - being called by ShootAndEmitParticle function

        private void Start()
        {
            currentAmmo = gunData.startingAmmo;
            SetHapticIntensityDuration(gunData.hapticIntensity, gunData.hapticDuration);
        }

        protected override void StartAttacking(ActivateEventArgs args)
        {
            Attack();
        }

        public void Attack()
        {
            Shoot();
        }

        public Weapons GetWeaponEnum()
        {
            return Weapons.Shotgun;
        }

        public void Shoot()
        {
            if (currentAmmo > 0)
            {
                if (Time.time > nextShot)
                {
                    nextShot = Time.time + gunData.fireRate;
                    for (int i = 0; i < pelletsPerShot; ++i)
                    {
                        Ray ray = new(bulletSpawn.position, GetPelletDirection());
                        ShootAndEmitParticle(ray);
                    }

                    OnShot?.Invoke();

                    AudioSystem.instance.Play("shotgun_shot");
                    ActivateHapticFeedback();
                    --currentAmmo;
                }
            }
            else
            {
                AudioSystem.instance.Play("gun_empty");
            }
        }

        private Vector3 GetPelletDirection()
        {
            Vector3 target = bulletSpawn.position + bulletSpawn.forward * gunData.range;
            target = new Vector3(
                target.x + UnityEngine.Random.Range(-spread, spread),
                target.y + UnityEngine.Random.Range(-spread, spread),
                target.z + UnityEngine.Random.Range(-spread, spread)
            );

            Vector3 dir = target - bulletSpawn.position;
            return dir.normalized;
        }

        public void ShootAndEmitParticle(Ray ray)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, gunData.range))
            {
                Hitbox hitbox = hit.collider.GetComponent<Hitbox>();
                if (hitbox != null)
                {
                    hitbox.Damage(Weapons.Shotgun, gunData.damage, attackerType);
                }

                var scannableObject = hit.collider.GetComponent<IScannable>();
                if (scannableObject == null) return;

                VFXEmitArgs overrideArgs = new VFXEmitArgs(bulletColor, bulletSize, bulletLifetime);
                scannableObject.EmitParticle(hit, overrideArgs);

                SpawnBulletTrail(hit.point);
            }
        }
    }
}