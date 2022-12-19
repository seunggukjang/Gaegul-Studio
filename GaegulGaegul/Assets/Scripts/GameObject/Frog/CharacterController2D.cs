using UnityEngine.Events;
using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_SwingSpeed = 4.8f;
	[SerializeField] private float m_MaxSwingSpeed = 120f;
	[SerializeField] private float m_maxSpeedX = 1.08f;
	[SerializeField] private float m_MoveSpeed = 10f;
	[SerializeField] private float m_SmallJumpSpeed = 5f;
	[SerializeField] private float m_JumpForce = 400f;                          
	[SerializeField] private bool m_AirControl = false;                         
	[SerializeField] private LayerMask m_WhatIsGround;                          
	[SerializeField] private Transform m_GroundCheck;                           
	[SerializeField] private Transform m_TopFrogCheck;                          
	[SerializeField] private Collider2D m_CrouchDisableCollider;
	[SerializeField] private ParticleSystem particleSystem;
	[SerializeField] private Animator m_Animator;
	[SerializeField] private Transform m_GroundCheckForBigJump;
	[SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] FrogHead head;
	
    bool jumpOffCoroutineIsRunning = false;
	private bool m_Grounded;
	private bool m_FrogBigJumpGround = false;
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;
	private Vector3 m_Velocity = Vector3.zero;

	private AudioManager audioManager;
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private Vector3 ground_halfSize = new Vector3();
	private Vector3 groundForBigJump_halfSize = new Vector3();
	private Vector3 halfSize = new Vector3();
	private bool isJump = false;
	private LayerMask frogMask;
	private LayerMask jumpOffPlatformMask;
	private bool canJumpDown = false;

	//private bool airControlinSmallJump = false;
	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		frogMask = LayerMask.NameToLayer("Frog");
		jumpOffPlatformMask = LayerMask.NameToLayer("JumpOffPlatform");

        if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		ground_halfSize.x = m_GroundCheck.lossyScale.x * 0.5f;
        ground_halfSize.y = m_GroundCheck.lossyScale.y * 0.5f;
		groundForBigJump_halfSize.x = m_GroundCheckForBigJump.lossyScale.x * 0.5f;
        groundForBigJump_halfSize.y = m_GroundCheckForBigJump.lossyScale.y * 0.5f;

        halfSize.x = transform.lossyScale.x * 0.5f;
		halfSize.y = transform.lossyScale.y * 0.5f;
		audioManager = FindObjectOfType<AudioManager>();
	}

    
    private void FixedUpdate()
	{
		m_FrogBigJumpGround = false;
        m_Grounded = false;
		
        Collider2D[] colliders = Physics2D.OverlapAreaAll(m_GroundCheck.position - ground_halfSize, m_GroundCheck.position + ground_halfSize, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				isJump = false;
				
            }

		colliders = Physics2D.OverlapAreaAll(m_GroundCheckForBigJump.position - groundForBigJump_halfSize, m_GroundCheckForBigJump.position + groundForBigJump_halfSize, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
			if (colliders[i].gameObject != gameObject)
                m_FrogBigJumpGround = true;
	}

    public void SetAirControl(bool isGrab)
	{
		m_AirControl = isGrab;
	}
    IEnumerator JumpOff()
    {
        
        jumpOffCoroutineIsRunning = true;
        Physics2D.IgnoreLayerCollision(frogMask, jumpOffPlatformMask, true);
		head.CollideOff();
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(frogMask, jumpOffPlatformMask, false);
		head.CollideOn();
        jumpOffCoroutineIsRunning = false;
    }
    public void JumpUp()
	{
		UnityEngine.Debug.Log("jump stp");	
        if (m_Grounded)
        {
            if (particleSystem != null)
                particleSystem.Emit(1);
            if (audioManager != null)
                audioManager.Play("bigjump");
            m_Rigidbody2D.velocity = (new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce));
            isJump = true;
            m_Animator.SetTrigger("bigJump");
        }
        StartCoroutine(JumpOff());
    }
	public void JumpDown()
	{
		if (m_Grounded)
		{
            if (particleSystem != null)
				particleSystem.Emit(1);
            if (audioManager != null)
                audioManager.Play("bigjump");
            m_Rigidbody2D.velocity = (new Vector2(m_Rigidbody2D.velocity.x, -m_JumpForce));
            isJump = true;
            m_Animator.SetTrigger("bigJump");
		}
        StartCoroutine(JumpOff());
    }
	public void Jump()
	{
		if (m_FrogBigJumpGround)
		{
			if(particleSystem != null)
				particleSystem.Emit(1);
			if(audioManager != null)
				audioManager.Play("bigjump");
			m_Rigidbody2D.velocity = (new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce));
			isJump = true;
			m_Animator.SetTrigger("bigJump");
        }
	}
	bool wasGrab = false;
	float previousMove = 0;
	public void Move(float move, bool crouch)
	{
            	UnityEngine.Debug.Log("AIR CONTROL STATUS : "+m_AirControl);
		
		if (move == 0)
		{
			m_Animator.SetTrigger("idle");
			previousMove = move;
			return;
		}
		if (!m_Grounded)
		{
			if (m_AirControl)
			{
            	UnityEngine.Debug.Log("Haha ^^ Your move is " + move + "so your velocity must be " + move*m_SwingSpeed);

				m_Velocity.y = m_Rigidbody2D.velocity.y;
                m_Velocity.x = move * m_SwingSpeed;
				wasGrab = true;

                float swingSpeed = m_Velocity.sqrMagnitude;
				if(swingSpeed > m_MaxSwingSpeed)
				{
					m_Velocity.x = 0;
					m_Velocity.y = 0;                    
                }
                m_Rigidbody2D.AddForce(m_Velocity);
            }
			else if (!wasGrab && previousMove * move <= 0)
            {
				m_Velocity.x = 0;
				m_Velocity.y = m_Rigidbody2D.velocity.y;
				m_Rigidbody2D.velocity = m_Velocity;
			}
			return;
		}

		if (m_Grounded)
		{
			wasGrab = false;

            if (!m_Animator.GetCurrentAnimatorStateInfo(0).IsName("smallJump"))
				m_Animator.SetTrigger("smallJump");
			if (particleSystem != null)
				particleSystem.Emit(1);
			if (audioManager != null)
				audioManager.Play("jump");
			m_Velocity.y = m_Rigidbody2D.velocity.y;
			if (move > 0)
				move = 1;
			else if (move < 0)
				move = -1;

			m_Velocity.x = move * m_MoveSpeed;
			if (!isJump)
			 m_Velocity.y = m_SmallJumpSpeed * (move != 0 ? 1 : 0);

			m_Rigidbody2D.velocity = (m_Velocity);

			if (m_Rigidbody2D.velocity.x > m_maxSpeedX)
				m_Velocity.x = m_maxSpeedX;
			else if (m_Rigidbody2D.velocity.x < -m_maxSpeedX)
				m_Velocity.x = -m_maxSpeedX;
			if (!isJump && m_Rigidbody2D.velocity.y > m_SmallJumpSpeed)
				m_Velocity.y = m_SmallJumpSpeed;

			if ((move > 0 && !m_FacingRight) || (move < 0 && m_FacingRight))
				Flip();
		}
		else if (m_AirControl)
		{
            UnityEngine.Debug.Log("Haha ^^ Your move is " + move + "so your velocity must be " + move*m_SwingSpeed);

			// m_Velocity.x = 0;
			m_Velocity.y = m_Rigidbody2D.velocity.y;
			m_Velocity.x = move * m_SwingSpeed;
			m_Rigidbody2D.AddForce(m_Velocity);
		}
		previousMove = move;
	}
	
	private void Flip()
	{
		m_FacingRight = !m_FacingRight;
		//spriteRenderer.flipX = !m_FacingRight;
		transform.right *= -1;

	}
}
