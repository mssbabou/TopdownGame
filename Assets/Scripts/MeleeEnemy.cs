using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;
using System;



public class MeleeEnemy : MonoBehaviour
{
    private Health health;

    public Color NormalColor = Color.white;
    public Color DamagedColor = Color.red;
    public float DamagedColorTime = 0.1f;

    // A* pathfinding interface
    public IAstarAI Navigation;

    // Reference to player
    public Transform player;

    // Detection layers
    public LayerMask whatIsGround, whatIsPlayer;

    // Movement/Patrol
    public Vector2 walkPoint;
    private bool walkPointSet;
    public float walkPointRange = 5f;

    // Attack variables
    public float timeBetweenAttacks = 1f;
    private bool alreadyAttacked;

    public UnityEvent OnAttack;
    public Animator animator;


    // The damage the enemy deals
    public float damageAmount = 10f;

    // We�ll use an overlap circle for the �hitbox�
    // 1) Create a child object called �AttackPoint� under your enemy and
    //    position it roughly where the weapon or contact point should be.
    // 2) Assign it in the Inspector.
    public Transform attackPoint;
    // Radius of the overlap circle used for attacking
    public float attackHitboxRadius = 0.5f;

    // Ranges
    public float sightRange = 5f;
    public float attackRange = 1f;
    public bool playerInSightRange, playerInAttackRange;

    private SpriteRenderer spriteRenderer;



    private void Awake()
    {
        // Health & onDeath
        health = GetComponent<Health>();
        health.onDeath.AddListener(Die);
        health.onTakeDamage.AddListener(TakeDamage);
 

        // A* Pathfinding
        Navigation = GetComponent<IAstarAI>();
        if (Navigation != null)
        {
            Navigation.onSearchPath += Update;
        }

        // Find player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        // Basic 2D overlap checks for player
        playerInSightRange = Physics2D.OverlapCircle(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics2D.OverlapCircle(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

       
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (walkPointSet)
        {
            //SetDestination(walkPoint);
            Vector2 distanceToWalkPoint = (Vector2)transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 0.5f) walkPointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomY = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        Vector2 candidatePoint = new Vector2(transform.position.x + randomX,
                                             transform.position.y + randomY);

        RaycastHit2D hit = Physics2D.Raycast(candidatePoint, Vector2.down, 2f, whatIsGround);
        if (hit.collider != null)
        {
            walkPoint = candidatePoint;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        if (player != null)
        {
            SetDestination(player.position);
            Vector2 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        }
    }

    private void AttackPlayer()
    {
        // Face the player in 2D
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        // Only attack if we haven�t attacked recently
        if (!alreadyAttacked)
        {
            Debug.Log("Enemy is Attacking!");

            // 1) Check everything in your attack hitbox
            //    (a circle around the AttackPoint)
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(
                attackPoint.position,
                attackHitboxRadius,
                whatIsPlayer
            );


            // 2) If we hit the player, call TakeDamage on their Health script
            foreach (Collider2D collider in hitObjects)
            {

                AttackAnimation();
                // Make sure the object has a Health component
                Health playerHealth = collider.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                }

            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }


    private void OnDestroy()
    {

    }

    public void AttackAnimation()
    {
        animator.SetBool("Attacking", true);   
    }

    private void StopAttackAnimation()
    {
        animator.SetBool("Attacking", false);
    }




    private void ResetAttack()
    {
        alreadyAttacked = false;
        StopAttackAnimation();
    }

    private void Die()
    {
        health.onDeath.RemoveAllListeners();
        Destroy(gameObject);
    }

    private void TakeDamage()
    {
        StartCoroutine(TakeDamageColorChange());
    }

    private IEnumerator TakeDamageColorChange()
    {
        spriteRenderer.color = DamagedColor;
        yield return new WaitForSeconds(DamagedColorTime);
        spriteRenderer.color = NormalColor;
    }

    private void SetDestination(Vector2 destination)
    {
        if (Navigation != null)
        {
            Navigation.destination = destination;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Attack range sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Sight range sphere
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        // The �hitbox� for the actual melee attack
        if (attackPoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, attackHitboxRadius);
        }
    }
}
