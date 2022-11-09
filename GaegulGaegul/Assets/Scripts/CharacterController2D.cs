using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_SwingSpeed = 1f;
	[SerializeField] private float m_maxSpeedX = 1.08f;
	[SerializeField] private float m_MoveSpeed = 10f;
	[SerializeField] private float m_SmallJumpSpeed = 5f;
	[SerializeField] private float m_JumpForce = 400f;
	[SerializeField] private bool m_AirControl = false;
	[SerializeField] private LayerMask m_WhatIsGround;
	[SerializeField] private Transform m_GroundCheck;
	[SerializeField] private Transform m_TopFrogCheck;
	[SerializeField] private Collider2D m_CrouchDisableCollider;
	[SerializeField] private Animator m_Animator;

	[SerializeField] private SpriteRenderer spriteRenderer;
	private bool m_Grounded;
	private bool m_FrogTouchGround = false;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;
	

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private Vector3 ground_halfSize = new Vector3();
	private Vector3 halfSize = new Vector3();
	private bool isJump = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		ground_halfSize.x = m_GroundCheck.lossyScale.x * 0.5f;
        ground_halfSize.y = m_GroundCheck.lossyScale.y * 0.5f;
		halfSize.x = transform.lossyScale.x * 0.5f;
		halfSize.y = transform.lossyScale.y * 0.5f;
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		
        Collider2D[] colliders = Physics2D.OverlapAreaAll(m_GroundCheck.position - ground_halfSize, m_GroundCheck.position + ground_halfSize, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				isJump = false;
				m_Animator.SetTrigger("idle");
            }

        colliders = Physics2D.OverlapAreaAll(transform.position - halfSize, transform.position + halfSize, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
            if (colliders[i].gameObject != gameObject)
                m_FrogTouchGround = true;
    }

	public void SetAirControl(bool isGrab)
	{
		m_AirControl = isGrab;
	}

	public void Jump()
	{
		if (m_Grounded)
		{
			m_Rigidbody2D.velocity = (new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce));
			isJump = true;
			m_Animator.SetTrigger("bigJump");
        }
	}

	public void Move(float move, bool crouch)
	{
		if (move == 0)
            return;
        if (!m_Grounded) {
			m_FrogTouchGround = false;
			m_Animator.SetTrigger("smallJump");
		}

		if (m_Grounded && m_FrogTouchGround)
		{
			m_Velocity.y = m_Rigidbody2D.velocity.y;
			if (move > 0)
				move = 1;
			else if (move < 0)
				move = -1;
				
			m_Velocity.x = move * m_MoveSpeed;
			if(!isJump)
				m_Velocity.y = m_SmallJumpSpeed * (move != 0 ? 1 : 0);

			m_Rigidbody2D.velocity = (m_Velocity);
			
			if(m_Rigidbody2D.velocity.x > m_maxSpeedX)
				m_Velocity.x = m_maxSpeedX;
			else if(m_Rigidbody2D.velocity.x < -m_maxSpeedX)
				m_Velocity.x = -m_maxSpeedX;
			if(!isJump && m_Rigidbody2D.velocity.y > m_SmallJumpSpeed)
				m_Velocity.y = m_SmallJumpSpeed;

			if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
				Flip();
        }
		else if(m_AirControl)
		{
			m_Velocity.x = 0;
			m_Velocity.y = m_Rigidbody2D.velocity.y;
			m_Velocity.x = move * m_SwingSpeed;
			m_Rigidbody2D.AddForce(m_Velocity);
		}	
	}

	private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		spriteRenderer.flipX = !m_FacingRight;
	}
}
