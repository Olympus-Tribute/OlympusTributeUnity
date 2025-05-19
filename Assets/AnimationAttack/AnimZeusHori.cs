using UnityEngine;
using UnityEngine.Playables;

public class Anim_ZeusHorizontal : MonoBehaviour
{
    public GameObject lightningPrefab;          
    public PlayableDirector lightningTimeline; 
    public float heightOffset = 1f;            

    [HideInInspector]
    public Vector3 targetPosition; // fournie dynamiquement à l’instanciation

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

        // Calcul de la direction vers la cible
        Vector3 direction = targetPosition - spawnPosition;
        float angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, angleY, 0f);

        // Instanciation orientée de la foudre
        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, rotation);

        if (lightningTimeline != null)
        {
            lightningTimeline.Play();
        }
    }
}