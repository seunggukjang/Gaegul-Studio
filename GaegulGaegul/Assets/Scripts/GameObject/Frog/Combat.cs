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
    public float headAttack_cooldown = 0.33f;
    private float next_headAttack;

    public Transform legAttack_pos;
    public float legAttack_range = 0.3f;
    public int legAttack_dmg = 18;
    public float legAttack_cooldown = 0.8f;
    private float next_legAttack;

    public int damageTaken;

    void Start()
    {
        damageTaken = 0;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > next_headAttack)
        {
            Debug.Log("headAttack");
            next_headAttack = Time.time + headAttack_cooldown;
            headAttack();
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > next_legAttack)
        {
            Debug.Log("legAttack");
            next_legAttack = Time.time + legAttack_cooldown;
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
                Debug.Log("hit" + enemy.name + " headAttack + flash");
                Combat enemyComBat = enemy.GetComponent<Combat>();
                enemyComBat.TakeDamage(headAttack_dmg);
                enemy.GetComponent<KnockBack>().Activate(transform.right, enemyComBat.damageTaken);
                enemy.GetComponentInChildren<takeDmg>().Flash();
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
                Debug.Log("hit" + enemy.name + " leg attack + flash");
                Combat enemyComBat = enemy.GetComponent<Combat>();
                enemyComBat.TakeDamage(headAttack_dmg);
                enemy.GetComponent<KnockBack>().Activate(transform.right, enemyComBat.damageTaken);
                
                enemy.GetComponentInChildren<takeDmg>().Flash();

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
