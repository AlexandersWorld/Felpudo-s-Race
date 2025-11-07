using System.Collections;
using UnityEngine;

public class Collectable : MonoBehaviour, IExplodable
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Explode()
    {
        if (animator != null)
        {
            animator.SetTrigger("Explode");
            StartCoroutine(WaitAndDestroy());
        }
    }
    public float Adjust(float health) => health += 40;

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}