using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueAttack : MonoBehaviour
{
    private GameObject tongueAttack;
    [SerializeField] private float mouseGrabThicknessTongue = 0.3f;
    [SerializeField] private Transform mouseAttackRange;
    [SerializeField] private GameObject tongueToObject;
    [SerializeField] private Tongue tongue;
    private MousePosition mousePos;
    private Rigidbody2D targetRB;
    [SerializeField] private float attackCoolTime = 2.0f;
    private float Timer = 0;
    bool isAttack = false;
    private LayerMask enemyLayer;

    void Start()
    {
        mousePos = GetComponent<MousePosition>();
        Timer = attackCoolTime;
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
    }
    void Update()
    {
        if(isAttack)
        {
            Timer -= Time.deltaTime;
            if(!tongueToObject.activeSelf)
                tongueToObject.SetActive(true);
        }
        if(Timer < 0)
        {
            isAttack = false;
            Timer = attackCoolTime;
            if(tongueToObject.activeSelf)
                tongueToObject.SetActive(false);
        }
    }
    private Rigidbody2D SelectTarget()
    {
        RaycastHit2D targetHit2D = Physics2D.CircleCast(transform.position, mouseGrabThicknessTongue, mousePos.GetMousePosition() - transform.position, mouseAttackRange.lossyScale.x * 0.5f, enemyLayer);
        if(!targetHit2D)
            return targetRB = null;
        targetRB = targetHit2D.rigidbody;
        return targetRB;
    }
    public bool IsOverAttackCoolTime()
    {
        return Timer == attackCoolTime;
    }
    public bool CanAttack()
    {
        if(SelectTarget() && targetRB.CompareTag("EnemyBoss") && !isAttack)
            return true;
        return false;
    }
    public void Attack()
    {
        if(!targetRB)
        {
            this.isAttack = false;
            return;
        }
        if(!this.isAttack)
        {
            tongue.targetTransform = targetRB.transform;
            targetRB.transform.GetComponent<EnemyBoss>().Damage(1);
            this.isAttack = true;
        }
    }
    IEnumerator AttackCoolTime()
    {
        //yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(attackCoolTime);
        tongueToObject.SetActive(false);
        isAttack = false;
    }
}
