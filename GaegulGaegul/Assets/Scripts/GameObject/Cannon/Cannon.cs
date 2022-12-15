using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Cannon : MonoBehaviour
{
    Vector2 halfSize = new Vector2(1,1);
    Vector2 position;
    [SerializeField] Vector3 offsetPosition;
    [SerializeField] Weight weight;
    LayerMask frogMask;
    bool canFrogGet = false;
    bool isFrogIn = false;
    GameObject frogObject;
    [SerializeField] private float fireForceX = 180;
    [SerializeField] private float fireForceY = 20;
    [SerializeField] private float angularSpeed = 0.1f;
    [SerializeField] private bool isWeight = false;
    [SerializeField] private GameObject chargedIcon;
    float angle = 0f;
    Vector3 previousRotation = new Vector3();
    Vector3 rotation = new Vector3();
    Vector2 fire_direction = new Vector2();
    AudioManager audioManager;
    bool canCannonBGM = true;
    void Start()
    {
        halfSize = halfSize * 1.5f;
        position = transform.position + offsetPosition;
        frogMask = 1 << LayerMask.NameToLayer("Frog");
        canFrogGet = false;
        audioManager = AudioManager.instance;
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

        if (isFrogIn)
        {
            if(audioManager && canCannonBGM)
            {
                audioManager.Stop("bgm");
                audioManager.Play("cannonbgm");
                canCannonBGM = false;
            }
            if(!isWeight)
            angle += Input.GetAxis("Vertical") * 30 * Time.deltaTime * angularSpeed;
            else
            {
                if(weight.GetGrabNumber() == 0)
                {
                    angle -= Time.deltaTime * angularSpeed * 30;
                    if (angle < 0)
                        angle = 0;
                }
                else if (angle < weight.GetGrabNumber() * 30)
                    angle += weight.GetGrabNumber() * 30 * Time.deltaTime * angularSpeed;
                else
                    angle = weight.GetGrabNumber() * 30;
            }
        }
        else if(angle != 0)
        {
            angle = 0;
        }
        if(previous_angle != angle)
        {
            if (audioManager)
                audioManager.Play("cannondrag");
            if (!isWeight)
                transform.Rotate(0, 0, angle - previous_angle);
            else
            {
                previousRotation.z = previous_angle;
                rotation.z = angle;
                transform.localRotation = Quaternion.Euler(rotation);
            }
                
        }
        
    }
    private void LateUpdate()
    {
        if(canFrogGet && Input.GetKeyDown(KeyCode.F))
        {
            if (audioManager)
                audioManager.Play("cannonammo");
            isFrogIn = true;
            frogObject.GetComponent<Grab>().CancelPulling();
            frogObject.SetActive(false);
            chargedIcon.SetActive(true);
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
        if (audioManager && canCannonBGM == false)
        {
            audioManager.Play("cannonfire");
            audioManager.Stop("cannonbgm");
            audioManager.Play("bgm");
            canCannonBGM = true;
        }
            
        frogObject.SetActive(true);
        chargedIcon.SetActive(false);
        frogObject.transform.position = new Vector3(transform.position.x, transform.position.y, frogObject.transform.position.z);
        frogObject.GetComponent<Player>().ReviveSkin();
        float radian = angle * Mathf.PI / 180;
        fire_direction.x = Mathf.Cos(radian) * fireForceX;
        fire_direction.y = Mathf.Sin(radian) * fireForceY;
        frogObject.GetComponent<Rigidbody2D>().velocity = fire_direction;
    }
}
