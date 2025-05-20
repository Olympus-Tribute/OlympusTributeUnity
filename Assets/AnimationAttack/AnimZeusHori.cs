using UnityEngine;
using UnityEngine.Playables;

public class Anim_ZeusHorizontal : MonoBehaviour
{
    public GameObject lightningPrefab;          
    public PlayableDirector lightningTimeline; 
    private float heightOffset = 1f;            

    [HideInInspector]
    public Vector3 targetPosition; // fournie à l’instanciation

    void Start()
    {
        TriggerLightning();
    }

    void TriggerLightning()
    {
        if (lightningPrefab == null)
        {
            Debug.LogWarning("Lightning prefab non assigné.");
            return;
        }

        Vector3 spawnPosition = transform.position + Vector3.up * heightOffset;

        Vector3 direction = targetPosition - spawnPosition;
        float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, angleY, 0f);

        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, rotation);

        if (lightningTimeline != null)
        {
            lightningTimeline.Play();
        }
    }
}