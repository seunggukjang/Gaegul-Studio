using System.Diagnostics;
using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PhysicalAction : MonoBehaviour
{
    private int actOnFrog = 1;
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
        actOnFrog = 1;
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
    void Update()
    {
        UnityEngine.Debug.Log("PRESSED KEY IS "+Input.anyKey);

        // float input_x = Input.GetAxis("Horizontal");
        // move.x = input_x;

        if (Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.N) || Input.GetKey(KeyCode.Keypad3))//left
        {
            move.x = -2f;
            if (Input.GetKey(KeyCode.Y))
                actOnFrog = 1;
            if (Input.GetKey(KeyCode.H))
                actOnFrog = 2;
            if (Input.GetKey(KeyCode.N))
                actOnFrog = 3;
            if (Input.GetKey(KeyCode.Keypad3))
                actOnFrog = 4;
        }
        if (Input.GetKey(KeyCode.T) || Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.B) || Input.GetKey(KeyCode.Keypad4))//right
        {
            move.x = 2f;
            if (Input.GetKey(KeyCode.T))
                actOnFrog = 1;
            if (Input.GetKey(KeyCode.G))
                actOnFrog = 2;
            if (Input.GetKey(KeyCode.B))
                actOnFrog = 3;
            if (Input.GetKey(KeyCode.Keypad4))
                actOnFrog = 4;

        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            isJump = true;
            if (Input.GetKey(KeyCode.Q))
                actOnFrog = 1;
            if (Input.GetKey(KeyCode.A))
                actOnFrog = 2;
            if (Input.GetKey(KeyCode.W))
                actOnFrog = 3;
            if (Input.GetKey(KeyCode.Keypad5))
                actOnFrog = 4;
        }
        if(Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            UnityEngine.Debug.Log("jump ?");
            isJumpDown = true;
            if (Input.GetKey(KeyCode.Q))
                actOnFrog = 1;
            if (Input.GetKey(KeyCode.A))
                actOnFrog = 2;
            if (Input.GetKey(KeyCode.W))
                actOnFrog = 3;
            if (Input.GetKey(KeyCode.Keypad5))
                actOnFrog = 4;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Alpha8) && !isGrab)
        {
            isGrab = true;
            if (Input.GetKey(KeyCode.Space))
                actOnFrog = 1;
            if (Input.GetKey(KeyCode.Alpha4))
                actOnFrog = 2;
            if (Input.GetKey(KeyCode.Alpha6))
                actOnFrog = 3;
            if (Input.GetKey(KeyCode.Alpha8))
                actOnFrog = 4;
            
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
