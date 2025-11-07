using UnityEngine;

public class Explodes: MonoBehaviour
{
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Explode()
    {
        animator.SetTrigger("Explode");
    }
}
