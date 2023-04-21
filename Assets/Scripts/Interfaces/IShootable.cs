using System;

namespace Detection
{
    public interface IShootable
    {
        public event Action OnShot;
        public void Shoot();
    }
}
