using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName;
    public int damage;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit()
    {
        Debug.Log("Hit with " + weaponName);
        animator.SetTrigger("Hit");
    }
}