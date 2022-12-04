using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{

    // public Animation animator;

    public Transform attack1;
    public float attack1_range = 0.5f;
    public LayerMask enemyLayers;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("attack1");
            Attack1();
        }
    }

    void Attack1()
    {
        // add: player Attack Animation
        // animator.SetTrigger("Attack1")

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attack1.position, attack1_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject)
                Debug.Log("hit" + enemy.name);
            // damage enemey
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attack1 == null)
            return;

        Gizmos.DrawWireSphere(attack1.position, attack1_range);
    }
}
