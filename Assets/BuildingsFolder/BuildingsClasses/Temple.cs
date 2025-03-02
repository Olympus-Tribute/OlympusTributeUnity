using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class Temple : Building
    {
        public float AttackRange { get; private set; }
        public int Damage { get; private set; }
        public float AttackSpeed { get; private set; }
        
        private bool _isAttacking;
        
        public Temple(string name, string description, GameObject gameObject, (int, int) position, uint ownerId, GameObject flag) : base(name, description, gameObject, position, ownerId, flag)
        {
        }
    }
}
