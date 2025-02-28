using UnityEngine;

namespace DefaultNamespace.ForSun
{
    public class SunMovement : MonoBehaviour
    {
        public float rotationSpeed = 0.01f; // Vitesse de rotation du soleil
        public float intensityDecreaseSpeed = 0.05f; // Vitesse de diminution de l'intensité
        public float minIntensity = 0.1f; // Intensité minimale
        public float maxIntensity = 1.0f; // Intensité maximale

        private Light sunLight;
        private float initialIntensity;
        private bool isSunset = true;

        void Start()
        {
            sunLight = GetComponent<Light>();
            if (sunLight == null)
            {
                Debug.LogError("Aucune lumière trouvée sur cet objet!");
                return;
            }
            initialIntensity = sunLight.intensity;
        }

        void Update()
        {
            // Rotation progressive du soleil (déplacement dans le ciel)
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        
            // Diminution et augmentation de l'intensité pour simuler un cycle jour/nuit
            if (isSunset)
            {
                sunLight.intensity -= intensityDecreaseSpeed * Time.deltaTime;
                if (sunLight.intensity <= minIntensity)
                {
                    isSunset = false;
                }
            }
            else
            {
                sunLight.intensity += intensityDecreaseSpeed * Time.deltaTime;
                if (sunLight.intensity >= maxIntensity)
                {
                    isSunset = true;
                }
            }
        }
    }
}