using System.Collections.Generic;
using UnityEngine;

namespace Battle
{
    public class Attacker : MonoBehaviour
    {
        public List<IDamageable> Targets { get; } = new List<IDamageable>();

        public void Reset() => Targets.Clear();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damageable) && !Targets.Contains(damageable))
            {
                Targets.Add(damageable);
            }
        }
    }
}
