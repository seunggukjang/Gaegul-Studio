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
    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController2D>();
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
    }
}
