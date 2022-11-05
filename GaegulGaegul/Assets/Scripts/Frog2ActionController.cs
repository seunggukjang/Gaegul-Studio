using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Frog2ActionController : MonoBehaviour
{
    private CharacterController2D controller;
    private Grab g;
    Vector2 move = new Vector2();
    bool isGrab = false;
    //private PlayerInputSystem playerControls;
    void Awake()
    {
        //playerControls = new PlayerInputSystem();
        //playerControls.PlayerActions.Move.started += Move;
        
        //playerControls.PlayerActions.Move.canceled += Move;
        //playerControls.PlayerActions.Jump.started += Jump;
        
        //playerControls.PlayerActions.Jump.canceled += Jump;
    }
    
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController2D>();
    }
    
    void Grab()
    {
        if(TryGetComponent<Grab>(out g))
        {
            if(g.Grip())
            {
                isGrab = true;
                return;
            }
            isGrab = false;
            return;
        }
    }
    void Move(Vector2 move) {
        controller.Move(move.x, false);
    }
    void Jump() {
        controller.Jump();
    }
    // Physics
    private void FixedUpdate()
    {
        Move(move);
    }
    //Input events after Game logic
    void Update()
    {
        float move = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Grab();
        //}
    }
    //Scene rendering
    private void LateUpdate()
    {
        
    }
}
