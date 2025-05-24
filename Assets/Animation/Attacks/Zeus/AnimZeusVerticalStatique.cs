using UnityEngine;

namespace Animation.Attacks.Zeus
{
    public class AnimZeusVerticalStatique : MonoBehaviour
    {
        public GameObject zeusVerticalPrefab;
    
        void Start()
        {
            Debug.Log("Zeus vertical a été lancé");
            var anim = Instantiate(zeusVerticalPrefab, transform.position, transform.rotation);
            Destroy(anim, 0.3f);
        
        }
    }
}
