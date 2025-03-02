using UnityEngine;

public class AnimVersCyl2 : MonoBehaviour
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
    private Transform[] cylinders; 
    private Vector3[] initialPositions; 

    private float waveAmplitude = 10f; 
    private float animationHeight = 0f; 
    private float waveSpeed = 5f; 
    private float animationDuration = 6f; 
    private float returnSpeed = 2f; 

    private float[] timeOffsets = new float[19]; 
    private float startTime; 
    private bool isReturning = false; 

    void OnEnable()
    {
        cylinders = new Transform[hexagonPositions.Length];
        initialPositions = new Vector3[hexagonPositions.Length];

        for (int i = 0; i < hexagonPositions.Length; i++)
        {
            GameObject cylinder = Instantiate(cylinderPrefab, hexagonPositions[i] + this.gameObject.transform.position, Quaternion.identity);
            cylinders[i] = cylinder.transform;
            initialPositions[i] = hexagonPositions[i] + this.gameObject.transform.position; 
        }

        startTime = Time.time;
        
        for (int i = 0; i < cylinders.Length; i++)
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
        float elapsedTime = Time.time - startTime;

        if (!isReturning && elapsedTime >= animationDuration)
        {
            isReturning = true;
        }

        if (isReturning)
        {
            for (int i = 0; i < cylinders.Length; i++)
            {
                cylinders[i].position = Vector3.Lerp(cylinders[i].position, initialPositions[i], returnSpeed * Time.deltaTime);
            }

            if (elapsedTime - animationDuration > 10)
            {
                foreach (var cylinder in cylinders)
                {
                    Destroy(cylinder.gameObject);
                }
                Destroy(this.gameObject);
            }
            
        }
        else
        {
            float t = elapsedTime * waveSpeed;
            float currentWaveAmplitude = waveAmplitude * Mathf.Exp(-0.5f * elapsedTime);

            for (int i = 0; i < cylinders.Length; i++)
            {
                float offsetTime = t + timeOffsets[i];
                float yPos = Mathf.Sin(offsetTime) * 0.5f + 0.5f; 
                yPos = yPos * currentWaveAmplitude + animationHeight;

                cylinders[i].position = new Vector3(cylinders[i].position.x, yPos, cylinders[i].position.z);
            }
        }
    }
}