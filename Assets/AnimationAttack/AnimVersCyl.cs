using UnityEngine;

public class AnimVersCyl : MonoBehaviour
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
    public float waveAmplitude = 5f;  
    public float animationHeight = 0f; 
    public float waveSpeed = 3f;  
    public int waveCount = 2;  
    
    private float[] timeOffsets = new float[19];
    
    private float startTime;

    void Start()
    {
        cylinders = new Transform[hexagonPositions.Length];
        for (int i = 0; i < hexagonPositions.Length; i++)
        {
            GameObject cylinder = Instantiate(cylinderPrefab, hexagonPositions[i], Quaternion.identity);
            cylinders[i] = cylinder.transform;
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
        float t = (Time.time - startTime) * waveSpeed;
        
        float currentWaveAmplitude = waveAmplitude * Mathf.Exp(-0.1f * (Time.time - startTime));
        
        for (int i = 0; i < cylinders.Length; i++)
        {
            float offsetTime = t + timeOffsets[i]; 
            float yPos = Mathf.Sin(offsetTime) * 0.5f + 0.5f; 
            yPos = yPos * currentWaveAmplitude + animationHeight; 
            
            cylinders[i].position = new Vector3(cylinders[i].position.x, yPos, cylinders[i].position.z);
        }
    }
}
