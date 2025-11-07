using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IExplodable
{
    private Animator animator;

    private Collider2D enemyCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
    }

    public void Explode()
    {
        if (enemyCollider != null)
            enemyCollider.enabled = false;

        if (animator != null)
        {
            StartCoroutine(WaitAndDestroy());
            animator.SetTrigger("Explode");
        }
    }

    public float Adjust(float health) => health -= 20;

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
