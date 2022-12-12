using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combat : MonoBehaviour
{

	[SerializeField] private Animator m_Animator;

    private int playerskin;

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

    //TRANSFORMATIONS 
    private int BeeBullet = 0;
    private bool isBee = false;
    public TextMeshProUGUI Beetext;
    public GameObject BeeCrown;
    public Transform BeeAttack_pos;
    public GameObject HoneyBulletPrefab;
    public float BeeAttack_cooldown = 0.8f;
    private float next_BeeAttack;

    private int LadybugBullet = 0;
    private bool isLadybug = false;
    // public Transform LadybugAttack_pos;
    // public GameObject HoneyBulletPrefab;
    public TextMeshProUGUI Ladybugtext;
    public GameObject LadybugCrown;
    public GameObject RedBulletPrefab;
    public float LadybugAttack_cooldown = 0.8f;
    private float next_LadybugAttack;

    private int BeetleBullet = 0;
    private bool isBeetle = false;
    // public Transform BeetleAttack_pos;
    // public GameObject HoneyBulletPrefab;
    public TextMeshProUGUI Beetletext;
    public GameObject BeetleCrown;
    public float BeetleAttack_cooldown = 0.8f;
    private float next_BeetleAttack;

    public float damageTaken;

    void Start()
    {
        playerskin = GetComponent<Player>().GetSkin();
        Debug.Log("playerskin = " + playerskin);
        damageTaken = 0;
    }

    void Update()
    {
        // BASIC ATTACKS
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Shield();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("bee : " + isBee + " ladybug : " + isLadybug + " beetle : " + isBeetle);
            if (isBee == true || isLadybug == true || isBeetle == true) {
                m_Animator.SetTrigger("shoot");
                Debug.Log("shoot");
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            m_Animator.SetTrigger("legBottom");
        }

        if (Input.GetKeyDown(KeyCode.Q) && Time.time > next_headAttack)
        {
            next_headAttack = Time.time + headAttack_cooldown;
            headAttack();
        }

        if (Input.GetKeyDown(KeyCode.E) && Time.time > next_legAttack)
        {
            next_legAttack = Time.time + legAttack_cooldown;
            legAttack();
        }
    
        // TRANSFORMATION
        if (Input.GetKeyDown(KeyCode.N))
        {
            BeeBullet = 3;
            GetComponentInParent<Player>().ChangeSkin(5);
            BeeCrown.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            LadybugBullet = 3;
            GetComponentInParent<Player>().ChangeSkin(6);
            LadybugCrown.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            BeetleBullet = 3;
            GetComponentInParent<Player>().ChangeSkin(7);
            BeetleCrown.SetActive(true);
        }

        // TRANSFORMATION ATTACK
        if (Input.GetKeyDown(KeyCode.T) && Time.time > next_BeeAttack && BeeBullet > 0)
        {
            if (isLadybug == true || isBeetle == true)
                return;
            isBee = true;
            Beetext.text = " x " + BeeBullet.ToString();
            next_BeeAttack = Time.time + BeeAttack_cooldown;
            BeeBullet--;
            Beetext.text = "x " + BeeBullet.ToString();
            Debug.Log("BeeBullet: " + BeeBullet);
            BeeAttack();
            if (BeeBullet < 1) {
                isBee = false;
                GetComponentInParent<Player>().ChangeSkin(playerskin);
                BeeCrown.SetActive(false);
                // back to basic skin
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && Time.time > next_LadybugAttack && LadybugBullet > 0)
        {
            if (isBee == true || isBeetle == true)
                return;
            isLadybug = true;
            next_LadybugAttack = Time.time + LadybugAttack_cooldown;
            LadybugAttack();
            LadybugBullet--;
            Debug.Log("LadybugBullet: " + LadybugBullet);
            if (LadybugBullet < 1) {
                GetComponentInParent<Player>().ChangeSkin(playerskin);
                LadybugCrown.SetActive(false);
                isLadybug = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && Time.time > next_BeetleAttack && BeetleBullet > 0)
        {
            if (isBee == true || isLadybug == true)
                return;
            isBeetle = true;
            next_BeetleAttack = Time.time + BeetleAttack_cooldown;
            BeetleAttack();
            BeetleBullet--;
            Debug.Log("BeetleBullet: " + BeetleBullet);
            if (BeetleBullet < 1) {
                GetComponentInParent<Player>().ChangeSkin(playerskin);
                BeetleCrown.SetActive(false);
                isBeetle = false;
                // back to basic skin
            }
        } 
    }

    void headAttack()
    {
        m_Animator.SetTrigger("headAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(headAttack_pos.position, headAttack_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                Combat enemyCombat = enemy.GetComponent<Combat>();
                enemyCombat.TakeDamage(headAttack_dmg);
                enemy.GetComponent<KnockBack>().Activate(transform.right, enemyCombat.damageTaken);
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
                Combat enemyCombat = enemy.GetComponent<Combat>();
                enemyCombat.TakeDamage(headAttack_dmg);
                enemy.GetComponent<KnockBack>().Activate(transform.up, enemyCombat.damageTaken);
            }
        }
    }

    void BeeAttack()
    {
        Instantiate(HoneyBulletPrefab, BeeAttack_pos.position, BeeAttack_pos.rotation);
    }

    void LadybugAttack()
    {
        Instantiate(RedBulletPrefab, BeeAttack_pos.position, BeeAttack_pos.rotation);
    }

    void BeetleAttack()
    {
        Instantiate(HoneyBulletPrefab, BeeAttack_pos.position, BeeAttack_pos.rotation);
    }

    void Shield()
    {
        Debug.Log("SIELD");
        m_Animator.SetTrigger("shield");
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
