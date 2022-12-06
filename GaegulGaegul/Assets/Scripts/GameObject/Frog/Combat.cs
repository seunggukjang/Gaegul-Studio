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

    private int BeeBullet = 0;
    public Transform BeeAttack_pos;
    public GameObject HoneyBulletPrefab;
    public float BeeAttack_cooldown = 0.8f;
    private float next_BeeAttack;

    public float damageTaken;

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

        if (Input.GetKeyDown(KeyCode.T) && Time.time > next_BeeAttack && BeeBullet > 0)
        {
            Debug.Log("Bee Bullet nbr = " + BeeBullet);
            next_BeeAttack = Time.time + BeeAttack_cooldown;
            BeeAttack();
            BeeBullet--;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("changeSkin / adding 3 BeeBullets");
            BeeBullet += 3;
            GetComponentInParent<Player>().ChangeSkin(5);
        }
    }

    void headAttack()
    {
        m_Animator.SetTrigger("headAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(headAttack_pos.position, headAttack_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                enemy.GetComponent<Combat>().TakeDamage(headAttack_dmg);
            }
        }
    }

    void legAttack()
    {
        m_Animator.SetTrigger("legAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(legAttack_pos.position, legAttack_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                enemy.GetComponent<Combat>().TakeDamage(legAttack_dmg);
            }
        }
    }

    void BeeAttack()
    {
        Instantiate(HoneyBulletPrefab, BeeAttack_pos.position, BeeAttack_pos.rotation);
    }

    void OnDrawGizmosSelected()
    {
        if (headAttack_pos == null || legAttack_pos == null)
            return;

        Gizmos.DrawWireSphere(headAttack_pos.position, headAttack_range);
        Gizmos.DrawWireSphere(legAttack_pos.position, legAttack_range);
    }

    public void TakeDamage(float dmg)
    {
        damageTaken += dmg;
        GetComponentInChildren<takeDmg>().Flash();
        GetComponentInChildren<percentDamage>().UpdateDmgTaken(damageTaken);
    }
}
