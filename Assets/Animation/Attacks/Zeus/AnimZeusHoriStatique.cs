using UnityEngine;

public class AnimZeusHoriStatique : MonoBehaviour
{
    public GameObject zeusHoriPrefab;
    void Start()
    {
        Debug.Log("Zeus Horizontal a été lancé");
        var anim = Instantiate(zeusHoriPrefab, transform.position, transform.rotation);
        Destroy(anim,2f);
    }

}
