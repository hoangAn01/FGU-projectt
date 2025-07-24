using UnityEngine;

public class AquaSpiderController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Run(); // Mặc định chạy animation run khi bắt đầu
    }

    public void Run()
    {
        animator.Play("run");
    }

    public void Attack()
    {
        animator.Play("attack");
    }

    public void Hurt()
    {
        animator.Play("hurt");
    }

    public void Dead()
    {
        animator.Play("dead");
    }
}