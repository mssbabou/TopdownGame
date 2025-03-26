using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    public string weaponName;
    public int damage;
    private Animator animator;
    public GameObject weaponPositionObject;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Hit()
    {
        if (weaponPositionObject != null)
        {
            // Check if the "Hit" animation is already playing
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName("Hit") || stateInfo.normalizedTime >= 1.0f)
            {
                Transform weaponPosition = weaponPositionObject.transform;
                transform.position = weaponPosition.position;
                transform.rotation = weaponPosition.rotation;
                Debug.Log("Hit with " + weaponName);
                animator.SetTrigger("Hit");
            }
            else
            {
                Debug.Log("Hit animation is still playing.");
            }
        }
    }
}