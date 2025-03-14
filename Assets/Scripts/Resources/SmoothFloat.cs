using UnityEngine;

namespace Resources
{
    public class SmoothFloat
    {
        public float currentValue;
        public float targetValue;
        private readonly float agility;


        public SmoothFloat(float agility, float currentValue) {
            this.currentValue = currentValue;
            this.targetValue = currentValue;
            this.agility = agility;
        }

        public void Update(float delta) {
            float distance = targetValue - currentValue;
            currentValue += distance * Mathf.Min(agility * delta, 1);
        }
    }
}