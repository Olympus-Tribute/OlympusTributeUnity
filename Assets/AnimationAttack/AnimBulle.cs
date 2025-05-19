using UnityEngine;
using System.Collections;

public class AnimBulle : MonoBehaviour
{
    public GameObject bubblePrefab; 
    public GameObject buildingTarget;
    private float spawnInterval = 0.03f; 
    private float tileRadius = 8.7f; 
    public float duration = 5f;
    private float timeElapsed = 0f; 
    private bool stopGeneration = false; 

    private void Start()
    {
        if (buildingTarget != null)
        {
            buildingTarget.SetActive(false); 
        }
        
        InvokeRepeating("SpawnBubble", 0f, spawnInterval);
        
        Invoke("StopBubbleGeneration", duration);
    }

    void Update()
    {
        if (stopGeneration)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= duration)
            {
                if (buildingTarget != null)
                {
                    buildingTarget.SetActive(true); 
                }
                DestroyRemainingBubbles();
                Destroy(this.gameObject);
            }
        }
    }

    void SpawnBubble()
    {
        if (stopGeneration) return; 

        Vector3 spawnPosition = GetRandomPositionOnHexagon(tileRadius);
        spawnPosition.y = -1.2f; 

        GameObject newBubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);
        StartCoroutine(MoveBubble(newBubble));
    }

    Vector3 GetRandomPositionOnHexagon(float radius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI); 
        float distance = Random.Range(0f, radius); 
        
        float x = distance * Mathf.Cos(angle);
        float z = distance * Mathf.Sin(angle);
        
        return new Vector3(x, 0f, z);
    }

    void StopBubbleGeneration()
    {
        stopGeneration = true;
    }

    void DestroyRemainingBubbles()
    {
        GameObject[] bubbles = GameObject.FindGameObjectsWithTag("Bubble");
        foreach (GameObject bubble in bubbles)
        {
            Destroy(bubble); 
        }
    }
    
    private IEnumerator MoveBubble(GameObject bubble)
    {
        float animationDuration = 2f; 
        float speedMultiplier = 2f; 
        float elapsedTime = 0f;

        Vector3 startPos = bubble.transform.position;
        Vector3 targetPos = new Vector3(startPos.x, 1.16f, startPos.z); 

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.deltaTime * speedMultiplier;
            float t = Mathf.Clamp01(elapsedTime / animationDuration);
            
            bubble.transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null; 
        }
        
        Destroy(bubble);
    }
}
