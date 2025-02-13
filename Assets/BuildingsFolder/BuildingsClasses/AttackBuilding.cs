using UnityEngine;

namespace BuildingsFolder.BuildingsClasses
{
    public class AttackBuilding : Building
    {
        public float AttackRange { get; private set; }
        public int Damage { get; private set; }
        public float AttackSpeed { get; private set; }
        
        private bool _isAttacking;
        
        public AttackBuilding(string name, string description, GameObject gameObject, (int, int) position, ushort ownerId, bool isAttacking, float attackRange, int damage, float attackSpeed) : base(name, description, gameObject, position, ownerId)
        {
            _isAttacking = isAttacking;
            AttackRange = attackRange;
            Damage = damage;
            AttackSpeed = attackSpeed;
        }
    }
}
