using System.Diagnostics;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PhysicalAction : MonoBehaviour
{
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
    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
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
    void Update()
    {
        UnityEngine.Debug.Log("PRESSED KEY IS "+Input.anyKey);

        // float input_x = Input.GetAxis("Horizontal");
        // move.x = input_x;
        if (Input.GetKey(KeyCode.Y) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Keypad3))//left
        {
            move.x = -0.5f;
        }
        if (Input.GetKey(KeyCode.T) || Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Keypad4))//right
        {
            move.x = 0.5f;
        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            isJump = true;
        }
        if(Input.GetKey(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            isJumpDown = true;
        }
        if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Alpha5) || Input.GetKey(KeyCode.Alpha7) && !isGrab)
        {
            isGrab = true;
            Grab();
        }
        // if(Input.GetKey(KeyCode.Z) && !isAttack)
        // {
        //     isAttack = true;
            
        // }
        // if(Input.GetKeyDown(KeyCode.R))
        // {
        //     Restart();
        // }

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
