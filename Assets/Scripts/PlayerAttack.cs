using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] Transform attackPosition;
    [SerializeField] LayerMask enemyMask;

    float timeSinceLastAttack = 0f;

    // Update is called once per frame
    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    void OnFire(InputValue value)
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            ScareAttack();
            timeSinceLastAttack = 0f;
        }
    }

    void ScareAttack()
    {
        Debug.Log("BOO!");

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemyMask);
        foreach (var enemy in enemiesInRange)
        {
            Debug.Log($"{enemy.name} is scared");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPosition)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        }
    }
}
