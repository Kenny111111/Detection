using UnityEngine;
using Detection;
using static Detection.IDealsDamage;

public class RaycastPistol : TwoHandInteractable, IShootable, IShootsParticle, IDealsDamage
{
    [SerializeField] private Color bulletColor = new Color(240, 208, 81);
    [SerializeField] private float bulletLifetime = 0.5f;
    [SerializeField] private float bulletSize = 0.15f;

    [SerializeField] private GunData gunData;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private ParticleSystem _particleSystem;
    private int currentAmmo;

    private float fireRate = 0.25f;
    private float nextShot = 0f;

    // bullet trail created
    public LineRenderer bulletTrail;

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
    }

    public override void StartObjectAction()
    {
        Attack();
    } 
    
    public void Attack()
    {
        Shoot();
    }

    public Weapons GetWeaponEnum()
    {
        return Weapons.Pistol;
    }

    public void Shoot()
    {
        if(currentAmmo > 0)
        {
            if(Time.time > nextShot)
            {
                nextShot = Time.time + fireRate;
                Ray ray = new(bulletSpawn.position, bulletSpawn.forward);
                ShootAndEmitParticle(ray);
                AudioSystem.manager.Play("beretta_shot");
                --currentAmmo;
            }
        }
        else
        {
            AudioSystem.manager.Play("gun_empty");
        }
    }

    public void ShootAndEmitParticle(Ray ray)
    {
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, gunData.range))
        {
            ITakeDamage damageTaker = hit.collider.GetComponent<ITakeDamage>();
            if(damageTaker != null)
            {
                damageTaker.TakeDamage(gunData.damage);
            }

            var scannableObject = hit.collider.GetComponent<IScannable>();
            if (scannableObject == null) return;

            VFXEmitArgs overrideArgs = new VFXEmitArgs(bulletColor, bulletSize, bulletLifetime);
            scannableObject.EmitParticle(hit, overrideArgs);

            SpawnBulletTrail(hit.point);

        }
    }
}
