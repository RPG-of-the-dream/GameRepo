using System;

namespace Battle
{
    public interface IHealable
    {
        event Action<float> HealingTaken;
        void TakeHealing(float healing);
    }
}