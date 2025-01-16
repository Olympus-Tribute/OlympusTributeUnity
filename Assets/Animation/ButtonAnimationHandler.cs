using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimationHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Animator animator;

    void Start()
    {
        // Récupérer l'Animator attaché au bouton
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger("Hover");
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger("Idle");
    }
}