using UnityEngine;
using UnityEngine.Playables;

public class Anim_ZeusHorizontal : MonoBehaviour
{
    public GameObject lightningPrefab;  
    public PlayableDirector lightningTimeline;  
    public Transform attackPosition;  
    public float heightOffset = 1f;  

    void Start()
    {
        TriggerLightning();  
    }

    void TriggerLightning()
    {
        if (lightningTimeline != null)
        {
            Vector3 spawnPosition = new Vector3(attackPosition.position.x, attackPosition.position.y + heightOffset, attackPosition.position.z);

            Debug.Log("Instantiating horizontal lightning at: " + spawnPosition);

            GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.Euler(90,0,0));

            lightningTimeline.Play();
        }
    }
}