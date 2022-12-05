using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float offSetPower = 1.5f;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Activate(Vector3 attackerFront, float power)
    {
        
        rb.AddForce(attackerFront * power * offSetPower);
    }
}
