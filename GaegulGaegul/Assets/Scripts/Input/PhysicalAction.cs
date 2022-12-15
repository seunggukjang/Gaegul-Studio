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
        //UnityEngine.Debug.Log("PRESSED KEY IS "+Input.anyKey);

        float input_x = Input.GetAxis("Horizontal");
        move.x = input_x;

        if (Input.GetButtonDown("Jump") && Input.GetAxis("Vertical") >= 0)
        {
            isJump = true;
        }
        if(Input.GetButton("Jump") && Input.GetAxis("Vertical") < 0)
        {
            isJumpDown = true;
        }
        if (Input.GetMouseButtonDown(0) && !isGrab)
        {
            isGrab = true;
            Grab();
        }
        if(Input.GetMouseButtonDown(1) && !isAttack)
        {
            isAttack = true;
            
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        Jump();
        JumpDown();
        Move(move);
        TongueAttack();
    }
    private void LateUpdate()
    {

    }
}
