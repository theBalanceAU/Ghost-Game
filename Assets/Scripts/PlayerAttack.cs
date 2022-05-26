using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] float attackCooldown = 1f;
    [SerializeField] float attackRange = 1f;
    [SerializeField] Transform attackPosition;
    [SerializeField] LayerMask attackLayerMask;
    [SerializeField] ParticleSystem attackParticleEffect;
    [SerializeField] AudioClip attackSoundEffect;

    float timeSinceLastAttack = 0f;

    // Unity events

    void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPosition)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPosition.position, attackRange);
        }
    }

    // Player Input events

    void OnFire(InputValue value)
    {
        if (timeSinceLastAttack >= attackCooldown)
        {
            ScareAttack();
            timeSinceLastAttack = 0f;
        }
    }


    // Private methods

    void ScareAttack()
    {
        Debug.Log("BOO!");
        attackParticleEffect?.Play();

        if (attackSoundEffect)
            AudioSource.PlayClipAtPoint(attackSoundEffect, Camera.main.transform.position);

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, attackLayerMask);
        foreach (var enemy in enemiesInRange)
        {
            Debug.Log($"{enemy.name} heard you say BOO!");
            EnemyBehavior e = enemy.GetComponent<EnemyBehavior>();
            e.Scare(transform);
        }
    }

}
