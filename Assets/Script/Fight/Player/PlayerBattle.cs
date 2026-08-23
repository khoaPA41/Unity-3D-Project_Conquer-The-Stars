using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerBattle : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }




    private void Attack(string attackName)
    {
        animator.CrossFadeInFixedTime(attackName, .1f);
    }


}
