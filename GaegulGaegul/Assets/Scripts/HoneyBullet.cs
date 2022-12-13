using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBullet : MonoBehaviour
{
    public int BeeAttack_dmg = 48;
    public float speed = 20f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Combat enemy = hitInfo.GetComponentInParent<Combat>();
        if (enemy != null)
        {
            enemy.TakeDamage(BeeAttack_dmg);
        }
        Destroy(gameObject);
    }
}
