using System;

namespace Detection
{
    public interface IEffect
    {
        public int Weight { get; set; }
        public void DoEffect(Action callback);
    }
}