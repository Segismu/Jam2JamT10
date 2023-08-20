using UnityEngine;

public class BounceAnimationHandler : MonoBehaviour
{
    [SerializeField] Animator animator;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
            animator.SetTrigger("start_bounce");
    }
}
