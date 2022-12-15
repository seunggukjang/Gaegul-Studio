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

    public Transform legAttack2_pos;
    public float legAttack2_range = 0.3f;
    public int legAttack2_dmg = 18;
    public float legAttack2_cooldown = 0.8f;
    private float next_legAttack2;

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
    public TextMeshProUGUI Ladybugtext;
    public GameObject LadybugCrown;
    public Transform LadybugAttack_pos;
    public GameObject RedBulletPrefab;
    public float LadybugAttack_cooldown = 0.8f;
    private float next_LadybugAttack;

    private int BeetleBullet = 0;
    private bool isBeetle = false;
    public Transform BeetleAttack_pos;
    public TextMeshProUGUI Beetletext;
    public GameObject BeetleCrown;
    public int BeetleAttack_dmg = 48;
    public float BeetleAttack_range = 0.8f;
    public float BeetleAttack_cooldown = 0.8f;
    private float next_BeetleAttack;
    private AudioManager audio;
    public float damageTaken;

    void Start()
    {
        audio = AudioManager.instance;
        if (!audio)
            audio = FindObjectOfType<AudioManager>();
        playerskin = GetComponent<Player>().GetSkin();
        damageTaken = 0;
    }

    void Update()
    {

        if (m_Animator.GetBool("isShield"))
        {
            Debug.Log("il ce shield");
        }

        // BASIC ATTACKS
        if (Input.GetKeyDown(KeyCode.Y))
        {
            m_Animator.SetTrigger("shield");
            m_Animator.SetBool("isShield", true);
        }

        if (Input.GetKeyDown(KeyCode.O) && Time.time > next_legAttack2)
        {
            next_legAttack2 = Time.time + legAttack2_cooldown;
            legAttack2();
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
            if (isLadybug == true || isBeetle == true)
                return;
            isBee = true;
            BeeBullet = 3;
            GetComponentInParent<Player>().ChangeSkin(5);
            BeeCrown.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isBeetle == true || isBee == true)
                return;
            isLadybug = true;
            LadybugBullet = 3;
            GetComponentInParent<Player>().ChangeSkin(6);
            LadybugCrown.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (isLadybug == true || isBee == true)
                return;
            isBeetle = true;
            BeetleBullet = 3;
            GetComponentInParent<Player>().ChangeSkin(7);
            BeetleCrown.SetActive(true);
        }

        // TRANSFORMATION ATTACK
        if (Input.GetKeyDown(KeyCode.T) && Time.time > next_BeeAttack && BeeBullet > 0)
        {
            if (isLadybug == true || isBeetle == true)
                return;
            Beetext.text = " x " + BeeBullet.ToString();
            next_BeeAttack = Time.time + BeeAttack_cooldown;
            BeeBullet--;
            Beetext.text = "x " + BeeBullet.ToString();
            BeeAttack();
            if (BeeBullet < 1) {
                StartCoroutine(DelayChangeSkin(playerskin));
                BeeCrown.SetActive(false);
                isBee = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && Time.time > next_LadybugAttack && LadybugBullet > 0)
        {
            if (isBee == true || isBeetle == true)
                return;
            next_LadybugAttack = Time.time + LadybugAttack_cooldown;
            LadybugAttack();
            LadybugBullet--;
            Ladybugtext.text = "x " + LadybugBullet.ToString();
            if (LadybugBullet < 1) {
                StartCoroutine(DelayChangeSkin(playerskin));
                LadybugCrown.SetActive(false);
                isLadybug = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && Time.time > next_BeetleAttack && BeetleBullet > 0)
        {
            if (isBee == true || isLadybug == true)
                return;
            next_BeetleAttack = Time.time + BeetleAttack_cooldown;
            BeetleAttack();
            BeetleBullet--;
            Beetletext.text = "x " + BeetleBullet.ToString();
            if (BeetleBullet < 1) {
                StartCoroutine(DelayChangeSkin(playerskin));
                BeetleCrown.SetActive(false);
                isBeetle = false;
            }
        } 
    }
    public void ChangeToForm(int i)
    {
        if(audio)
        audio.Play("changeform");
       switch (i)
        {
            case 0:
                BeeBullet = 3;
                GetComponentInParent<Player>().ChangeSkin(5);
                BeeCrown.SetActive(true);
                break;
            case 1:
                LadybugBullet = 3;
                GetComponentInParent<Player>().ChangeSkin(6);
                LadybugCrown.SetActive(true);
                break;
            case 2:
                BeetleBullet = 3;
                GetComponentInParent<Player>().ChangeSkin(7);
                BeetleCrown.SetActive(true);
                break;
        }
    }
    void headAttack()
    {
        m_Animator.SetTrigger("headAttack");
        if (audio)
        {
            audio.Play("headattack");
        }
            
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
        if (audio)
            audio.Play("legattack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(legAttack_pos.position, legAttack_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                Combat enemyCombat = enemy.GetComponent<Combat>();
                enemyCombat.TakeDamage(legAttack_dmg);
                enemy.GetComponent<KnockBack>().Activate(transform.up, enemyCombat.damageTaken);
            }
        }
    }

    void legAttack2()
    {
        m_Animator.SetTrigger("legBottom");
        if (audio)
            audio.Play("legattack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(legAttack2_pos.position, legAttack2_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                Combat enemyCombat = enemy.GetComponent<Combat>();
                enemyCombat.TakeDamage(legAttack2_dmg);
                enemy.GetComponent<KnockBack>().Activate(-transform.up, enemyCombat.damageTaken);
            }
        }
    }

    void BeeAttack()
    {
        Instantiate(HoneyBulletPrefab, BeeAttack_pos.position, BeeAttack_pos.rotation);
        m_Animator.SetTrigger("specialAttack");
        if (audio)
            audio.Play("beeattack");
    }

    void LadybugAttack()
    {
        Instantiate(RedBulletPrefab, LadybugAttack_pos.position, LadybugAttack_pos.rotation);
        m_Animator.SetTrigger("specialAttack");
        if (audio)
            audio.Play("ladybugattack");
    }

    void BeetleAttack()
    {
        m_Animator.SetTrigger("specialAttack");
        if (audio)
            audio.Play("beetlesattack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(BeetleAttack_pos.position, BeetleAttack_range, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject != gameObject) {
                Combat enemyCombat = enemy.GetComponent<Combat>();
                enemyCombat.TakeDamage(BeetleAttack_dmg);
                enemy.GetComponent<KnockBack>().Activate(transform.right, enemyCombat.damageTaken);
            }
        }
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
        if (m_Animator.GetBool("isShield"))
        {
            Debug.Log("il ce shield");
        }
        damageTaken += dmg;
        GetComponentInChildren<takeDmg>().Flash();
        GetComponentInChildren<percentDamage>().UpdateDmgTaken(damageTaken);
        if (audio)
            audio.Play("damage2");
    }

    
    IEnumerator DelayChangeSkin(int skinName)
    {
        yield return new WaitForSeconds(0.4f);
        GetComponentInParent<Player>().ChangeSkin(skinName);
    }
}
