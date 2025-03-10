using UnityEngine;

namespace ForSun
{
    public class SunMovement : MonoBehaviour
    {
        public float rotationSpeed = 0.01f; // Vitesse de rotation du soleil
        public float intensityDecreaseSpeed = 0.05f; // Vitesse de diminution de l'intensité
        public float minIntensity = 0.1f; // Intensité minimale
        public float maxIntensity = 1.0f; // Intensité maximale

        private Light _sunLight;
        private float _initialIntensity;
        private bool _isSunset = true;

        void Start()
        {
            _sunLight = GetComponent<Light>();
            if (_sunLight == null)
            {
                Debug.LogError("Aucune lumière trouvée sur cet objet!");
                return;
            }
            _initialIntensity = _sunLight.intensity;
        }

        void Update()
        {
            // Rotation progressive du soleil (déplacement dans le ciel)
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        
            // Diminution et augmentation de l'intensité pour simuler un cycle jour/nuit
            if (_isSunset)
            {
                _sunLight.intensity -= intensityDecreaseSpeed * Time.deltaTime;
                if (_sunLight.intensity <= minIntensity)
                {
                    _isSunset = false;
                }
            }
            else
            {
                _sunLight.intensity += intensityDecreaseSpeed * Time.deltaTime;
                if (_sunLight.intensity >= maxIntensity)
                {
                    _isSunset = true;
                }
            }
        }
    }
}