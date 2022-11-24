using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannon : MonoBehaviour
{
    Vector2 halfSize;
    Vector2 position;
    LayerMask frogMask;
    bool canFrogGet = false;
    bool isFrogIn = false;
    GameObject frogObject;
    [SerializeField] private float fireForceX = 20;
    [SerializeField] private float fireForceY = 20;
    [SerializeField] private float angularSpeed = 0.00001f;
    float angle = 0f;
    Vector2 fire_direction = new Vector2();
    void Start()
    {
        halfSize = transform.lossyScale * 0.5f;
        position = transform.position;
        frogMask = 1 << LayerMask.NameToLayer("Frog");
        canFrogGet = false;
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        
        canFrogGet = false;
        if(!isFrogIn)
        {
            frogObject = null;
        }
        Collider2D collider = Physics2D.OverlapArea(position - halfSize, position + halfSize, frogMask);
        if (collider && !isFrogIn)
        {
            canFrogGet = true;
            frogObject = collider.gameObject;
        }
    }
    private void Update()
    {
        float previous_angle = angle;
        if(isFrogIn)
        {
            angle += Input.GetAxis("Vertical");
            
        }
        else if(angle != 0)
        {
            angle = 0;
        }
        if(previous_angle != angle)
        {
            transform.Rotate(0, 0, angle - previous_angle);
        }
    }
    private void LateUpdate()
    {
        if(canFrogGet && Input.GetKeyDown(KeyCode.F))
        {
            isFrogIn = true;
            frogObject.SetActive(false);
        }
    }
    public bool GetFrogIn()
    {
        return isFrogIn;
    }
        public void SetFrogIn(bool isIn)
    {
        isFrogIn = isIn;
    }
    public void Fire()
    {
        frogObject.SetActive(true);
        float radian = angle * Mathf.PI / 180;
        fire_direction.x = Mathf.Cos(radian) * fireForceX;
        fire_direction.y = Mathf.Sin(radian) * fireForceY;
        frogObject.GetComponent<Rigidbody2D>().velocity = fire_direction;
    }
}
