using System.Diagnostics;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PhysicalAction2 : MonoBehaviour
{
    private CharacterController2D controller;
    private Grab g;
    private TongueAttack tongueAttack;
    private bool isGrab = false;
    private bool isAttack = false;
    private bool isJump = false;
    private Vector2 move = new Vector2(0, 0);
    Animator animator;
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
            if (g.Grip())
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

        if (TryGetComponent<TongueAttack>(out tongueAttack))
        {
            if (tongueAttack.CanAttack() && !isGrab)
            {
                tongueAttack.Attack();
            }

        }
        isAttack = false;
    }
    void Move(Vector2 m)
    {
        controller.Move(m.x, false);
    }
    void Jump()
    {
        if (!isJump)
            return;
        controller.Jump();
        isJump = false;
    }

    void Update()
    {
        
        bool input_x_positive = Input.GetKey(KeyCode.M);
        bool input_x_negative = Input.GetKey(KeyCode.K);
        if(input_x_positive)
            move.x = 1;
        else if(input_x_negative)
            move.x = -1;
        else
            move.x = 0;
        if (Input.GetKey(KeyCode.O))
        {
            isJump = true;
        }
        if (Input.GetMouseButtonDown(0) && !isGrab)
        {
            isGrab = true;
            Grab();
        }
        if (Input.GetMouseButtonDown(1) && !isAttack)
        {
            isAttack = true;

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        Jump();
        Move(move);
        TongueAttack();
    }
    private void LateUpdate()
    {

    }
}
