using UnityEngine;

public class TestZeusLightning : MonoBehaviour
{
    public GameObject lightningPrefab;
    public float angle = 30f;

    void Start()
    {
        Quaternion rotation = Quaternion.Euler(90, angle, 0);
        Instantiate(lightningPrefab, transform.position, rotation);
    }
}

