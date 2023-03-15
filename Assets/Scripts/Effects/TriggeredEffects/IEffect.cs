using System;

namespace Detection
{
    public interface IEffect
    {
        public void DoEffect(Action callback);
    }
}