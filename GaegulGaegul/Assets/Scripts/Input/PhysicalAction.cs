using System.Diagnostics;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PhysicalAction : MonoBehaviour
{
    [SerializeField] private int actOnFrog = 1;
    private CharacterController2D controller;
    private Grab g;
    private TongueAttack tongueAttack;
    private bool isGrab = false;
    private bool isAttack = false;
    private bool isJump = false;
    private Vector2 move = new Vector2 (0, 0);
    Animator animator;
    [SerializeField] bool isPVPmode = false;
    private bool isJumpDown = false;

    public List<CharacterController2D> FrogsPlayer;

    private void Start()
    {
        SetKeyMap();
        // controller = GetComponent<CharacterController2D>();
    }
    void Restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
    void Grab()
    {
        if (!isGrab)
            return;
        isGrab = false;
        if (TryGetComponent<Grab>(out g))
        {
            if (TryGetComponent<TongueAttack>(out tongueAttack))
            {
                if (!tongueAttack.IsOverAttackCoolTime())
                {
                    return;
                }
            }
            if(g.Grip())
            {
                controller.SetAirControl(true);
                return;
            }
            controller.SetAirControl(false);
            return;
        }
        isGrab = false;
    }
    void TongueAttack() 
    {
        if (!isAttack)
            return;
        
        if(TryGetComponent<TongueAttack>(out tongueAttack))
        {
            if(tongueAttack.CanAttack() && !isGrab)
            {
                tongueAttack.Attack();
            }
            
        }
        isAttack = false;
    }
    void Move(Vector2 m) {
        controller.Move(m.x, false);
    }
    void Jump() {
        if (!isJump)
            return;
        if (isPVPmode)
            controller.JumpUp();
        else
            controller.Jump();
        isJump = false;
    }
   void JumpDown()
    {
        if (!isJumpDown)
            return;
        if (isPVPmode)
        controller.JumpDown();
        isJumpDown = false;
    }
    void GetFrogControl()
    {
        controller = FrogsPlayer[actOnFrog-1].GetComponent<CharacterController2D>();
    }
    KeyCode leftMove;
    KeyCode rightMove;
    KeyCode jumpUp;
    KeyCode jumpDown;
    KeyCode grab;
    void SetKeyMap()
    {
        switch(actOnFrog)
        {
            case 1:
                leftMove = KeyCode.Y;
                rightMove = KeyCode.T;
                jumpUp = KeyCode.Q;
                jumpDown = KeyCode.U;
                grab = KeyCode.Space;
                break;
            case 2:
                leftMove = KeyCode.H;
                rightMove = KeyCode.G;
                jumpUp = KeyCode.A;
                jumpDown = KeyCode.J;
                grab = KeyCode.Alpha4;
                break;
            case 3:
                leftMove = KeyCode.N;
                rightMove = KeyCode.B;
                jumpUp = KeyCode.W;
                jumpDown = KeyCode.Question;
                grab = KeyCode.Alpha6;
                break;
            case 4:
                leftMove = KeyCode.Keypad3;
                rightMove = KeyCode.Keypad4;
                jumpUp = KeyCode.Keypad5;
                jumpDown = KeyCode.Alpha7;
                grab = KeyCode.Alpha8;
                break;
        }
    }
    void Update()
    {
        //UnityEngine.Debug.Log("PRESSED KEY IS "+Input.anyKey);

        // float input_x = Input.GetAxis("Horizontal");
        // move.x = input_x;

        if (Input.GetKey(leftMove))
        {
            move.x = -2f;
           
        }
        if (Input.GetKey(rightMove))//right
        {
            move.x = 2f;
            
        }
        if (Input.GetKeyDown(jumpUp))
        {
            isJump = true;
            
        }
        if(Input.GetKeyDown(jumpDown))
        {
            UnityEngine.Debug.Log("jump ?");
            isJumpDown = true;
            
        }
        if (Input.GetKeyDown(grab)&& !isGrab)
        {
            isGrab = true;
            
        }
        // if(Input.GetKey(KeyCode.Z) && !isAttack)
        // {
        //     isAttack = true;
            
        // }
        // if(Input.GetKeyDown(KeyCode.R))
        // {
        //     Restart();
        // }
        GetFrogControl();
        Grab();
        Jump();
        JumpDown();
        Move(move);
        TongueAttack();
        move.x = 0;
    }
    private void LateUpdate()
    {

    }
}
