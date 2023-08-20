using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("StarBounce", true);

            Debug.Log("GIL");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("StarBounce", false);

            Debug.Log("GIL2");
        }
    }
}
