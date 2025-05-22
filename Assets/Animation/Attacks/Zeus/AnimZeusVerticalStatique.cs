using UnityEngine;

public class AnimZeusVerticalStatique : MonoBehaviour
{
    public GameObject zeusVerticalPrefab;
    
    void Start()
    {
        Debug.Log("Zeus vertical a été lancé");
        var anim = Instantiate(zeusVerticalPrefab, transform.position, Quaternion.identity);
        Destroy(anim, 0.3f);
        
    }
}
