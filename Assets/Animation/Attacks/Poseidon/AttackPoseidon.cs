using BuildingsFolder;
using UnityEngine;

namespace Animation.Attacks
{
    public class AttackPoseidon : MonoBehaviour
    {
        private Vector3[] hexagonPositions = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(17.3f, 0, 0),
            new Vector3(-17.3f, 0, 0),
            new Vector3(8.65f, 0, 15f),
            new Vector3(-8.65f, 0, 15f),
            new Vector3(8.65f, 0, -15f),
            new Vector3(-8.65f, 0, -15f),
            new Vector3(-17.3f, 0, 30f),
            new Vector3(-17.3f, 0, -30f),
            new Vector3(0, 0, 30f),
            new Vector3(0, 0, -30f),
            new Vector3(-25.95f, 0, 15f),
            new Vector3(25.95f, 0, 15f),
            new Vector3(-25.95f, 0, -15f),
            new Vector3(25.95f, 0, -15f),
            new Vector3(34.6f, 0, 0),
            new Vector3(-34.6f, 0, 0),
            new Vector3(17.3f, 0, 30f),
            new Vector3(17.3f, 0, -30f)
        };

        public GameObject cylinderPrefab; 
        private Transform[] _cylinders; 
        private Vector3[] initialPositions; 

        private float waveAmplitude = 10f; 
        private float animationHeight = 1f; 
        private float waveSpeed = 5f; 
        private float AnimationDuration = AllPrices.PoseidonParalyzeDuration; 
        private float returnSpeed = 2f; 
        private float[] timeOffsets = new float[19]; 
        private bool isReturning = false; 
        private float elapsedTime = 0f;

        void OnEnable()
        {
            _cylinders = new Transform[hexagonPositions.Length];
            initialPositions = new Vector3[hexagonPositions.Length];

            for (int i = 0; i < hexagonPositions.Length; i++)
            {
                GameObject cylinder = Instantiate(cylinderPrefab, hexagonPositions[i] + this.gameObject.transform.position, Quaternion.identity);
                _cylinders[i] = cylinder.transform;
                initialPositions[i] = hexagonPositions[i] + this.gameObject.transform.position; 
            }
            
            for (int i = 0; i < _cylinders.Length; i++)
            {
                if (i == 0)
                    timeOffsets[i] = 0f; 
                else if (i <= 6)
                    timeOffsets[i] = 1f; 
                else
                    timeOffsets[i] = 2f;
            }
        }

        void Update()
        {
            elapsedTime += Time.deltaTime;

            if (!isReturning && elapsedTime >= AnimationDuration)
            {
                isReturning = true;
            }

            if (isReturning)
            {
                for (int i = 0; i < _cylinders.Length; i++)
                {
                    _cylinders[i].position = Vector3.Lerp(_cylinders[i].position, initialPositions[i], returnSpeed * Time.deltaTime);
                }

                if (elapsedTime - AnimationDuration > 10)
                {
                    foreach (var cylinder in _cylinders)
                    {
                        Destroy(cylinder.gameObject);
                    }
                    Destroy(gameObject);
                }
            
            }
            else
            {
                float t = elapsedTime * waveSpeed;
                float currentWaveAmplitude = Mathf.Max(waveAmplitude * Mathf.Exp(-0.5f * elapsedTime),1);

                for (int i = 0; i < _cylinders.Length; i++)
                {
                    float offsetTime = t + timeOffsets[i];
                    float yPos = Mathf.Sin(offsetTime) * 0.5f + 0.5f; 
                    yPos = yPos * currentWaveAmplitude + animationHeight;

                    _cylinders[i].position = new Vector3(_cylinders[i].position.x, yPos, _cylinders[i].position.z);
                }
            }
        }
    }
}