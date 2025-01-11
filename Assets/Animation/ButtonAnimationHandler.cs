using UnityEngine;

public class ButtonAnimationHandler : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        // Récupérer l'Animator attaché au bouton
        animator = GetComponent<Animator>();
    }

    public void OnMouseEnter()
    {
        // Déclencher l'animation de survol
        animator.SetTrigger("Hover");
    }

    public void OnMouseExit()
    {
        // Revenir à l'état normal
        animator.SetTrigger("Idle");
    }
}