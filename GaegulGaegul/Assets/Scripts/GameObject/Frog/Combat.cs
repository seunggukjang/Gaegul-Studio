using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

	[SerializeField] private Animator m_Animator;

    public LayerMask enemyLayers;
    
    public Transform headAttack_pos;
    public float headAttack_range = 0.3f;
    public int headAttack_dmg = 10;

    public Transform legAttack_pos;
    public float legAttack_range = 0.3f;
    public int legAttack_dmg = 18;

    public int damageTaken;

    void Start()
    {
        damageTaken = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("headAttack");
            headAttack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("legAttack");
            legAttack();
        }
    }

    void headAttack()
    {
        // add: player Attack Animation
        m_Animator.SetTrigger("headAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(headAttack_pos.position, headAttack_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                Debug.Log("hit" + enemy.name + " headAttack");
                enemy.GetComponent<Combat>().TakeDamage(headAttack_dmg);
            }

            // damage enemey
        }
    }

    void legAttack()
    {
        // add: player Attack Animation
        m_Animator.SetTrigger("legAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(legAttack_pos.position, legAttack_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                Debug.Log("hit" + enemy.name + " leg attack");
                enemy.GetComponent<Combat>().TakeDamage(headAttack_dmg);
            }

            // damage enemey
        }
    }

    void OnDrawGizmosSelected()
    {
        if (headAttack_pos == null || legAttack_pos == null)
            return;

        Gizmos.DrawWireSphere(headAttack_pos.position, headAttack_range);
        Gizmos.DrawWireSphere(legAttack_pos.position, legAttack_range);
    }

    public void TakeDamage(int dmg)
    {
        damageTaken += dmg;

        // play hurt animation
    }
}
