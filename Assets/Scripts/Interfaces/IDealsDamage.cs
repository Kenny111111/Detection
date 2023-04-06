namespace Detection
{
    public interface IDealsDamage
    {
        public enum Weapons
        {
            None,
            Knife,
            Pistol,
            Shotgun,
            Rifle,
            Grenade
        }

        public void Attack();

        public Weapons GetWeaponEnum();
    }
}

