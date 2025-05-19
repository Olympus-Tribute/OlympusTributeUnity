using UnityEngine;
using UnityEngine.Playables;

public class Anim_ZeusVertical : MonoBehaviour
{
    public GameObject lightningPrefab;       
    public float heightOffset = 1f;          

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

        GameObject lightning = Instantiate(lightningPrefab, spawnPosition, Quaternion.identity);
        
        PlayableDirector director = lightning.GetComponent<PlayableDirector>();
        if (director != null)
        {
            director.Play();
        }
    }
}