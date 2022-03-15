using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, RUN, JUMP, FALL, ATTACK, DEATH }
public enum PLAYER_ANIMATION { IDLE, RUN, JUMP, FALL, LAND, ATTACK, DEATH, STOP, BLOCK, HIT, LAST_NO_USE }

public class Player : MonoBehaviour
{
    Animator m_animator;
    int m_animationCurrentState;

    string m_idleAnimationName = "idle";
    string m_runAnimationName = "run";
    string m_jumpAnimationName = "jump";
    string m_fallAnimationName = "fall";
    string m_attackAnimationName = "attack";
    string m_deathAnimationName = "death";
    string m_stopAnimationName = "stop";
    string m_blockAnimationName = "block";
    string m_hitAnimationName = "hit";

    int[] m_animationHash = new int[(int)PLAYER_ANIMATION.LAST_NO_USE];

    PLAYER_STATE m_state;

    Rigidbody2D m_rb2D;
    [SerializeField] float m_speed = 10.0f;
    [SerializeField] float m_maxHeight = 10.0f;
    [SerializeField] float m_timeToPeak1 = 1f;
    [SerializeField] float m_timeToPeak2 = 1f;

    bool m_isGrounded;

    SpriteRenderer m_spriteRenderer;
    bool m_isFacingRight;

    float m_gravity1;
    float m_gravity2;
    float m_initialVelocityY;
    float m_direction;
    bool m_canIMove = true;

    

    private void Awake()
    {
        m_animationHash[(int)PLAYER_ANIMATION.IDLE] = Animator.StringToHash(m_idleAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.RUN] = Animator.StringToHash(m_runAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.JUMP] = Animator.StringToHash(m_jumpAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.FALL] = Animator.StringToHash(m_fallAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.ATTACK] = Animator.StringToHash(m_attackAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.DEATH] = Animator.StringToHash(m_deathAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.STOP] = Animator.StringToHash(m_stopAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.BLOCK] = Animator.StringToHash(m_blockAnimationName);
        m_animationHash[(int)PLAYER_ANIMATION.HIT] = Animator.StringToHash(m_hitAnimationName);

        m_rb2D = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_isGrounded = true;
        m_isFacingRight = true;
    }

    private void Start()
    {
        m_gravity1 = -2 * m_maxHeight / (m_timeToPeak1 * m_timeToPeak1);
        m_gravity2 = -2 * m_maxHeight / (m_timeToPeak2 * m_timeToPeak2);
        m_initialVelocityY = 2 * m_maxHeight / m_timeToPeak1;
    }

    void ChangeAnimationState(int p_newState)
    {
        if (m_animationCurrentState == p_newState) return;   // stop the same animation from interrupting itself
        m_animator.Play(p_newState);                // play the animation
        m_animationCurrentState = p_newState;                // reassigning the new state
    }


    private void Update()
    {
         switch (m_state)
        {
            case PLAYER_STATE.IDLE:
                Move(PLAYER_STATE.IDLE);
                Jump();
                break;
            case PLAYER_STATE.RUN:
                Move(PLAYER_STATE.IDLE);
                Jump();
                break;
            case PLAYER_STATE.JUMP:
                Move(PLAYER_STATE.JUMP);
                break;
            case PLAYER_STATE.FALL:
                Move(PLAYER_STATE.FALL);
                break;
        }
    }

    void Move(PLAYER_STATE p_defaultState)
    {
        float horizontalAxisValue = Input.GetAxisRaw("Horizontal");
        if (horizontalAxisValue != 0 && m_canIMove)
        {
            if (!m_isFacingRight && horizontalAxisValue > 0) { FlipX(); }
            if (m_isFacingRight && horizontalAxisValue < 0) { FlipX(); }

            if (m_isGrounded && /*m_state != PLAYER_STATE.LAND &&*/ m_state != PLAYER_STATE.ATTACK)
            {
                m_state = PLAYER_STATE.RUN;
                SetPlayerAnimation(PLAYER_ANIMATION.RUN);
                //ChangeAnimationState(m_animationHash[(int)PLAYER_ANIMATION.RUN]);
            }
        }
        else
        {
            m_state = p_defaultState;
            if (m_state != PLAYER_STATE.ATTACK && m_animationCurrentState != (int)PLAYER_ANIMATION.LAND)
            {
                SetPlayerAnimation((PLAYER_ANIMATION)p_defaultState);
                //ChangeAnimationState(m_animationHash[(int)p_defaultState]);
            }
        }

        if (m_canIMove)
        {
            m_direction = horizontalAxisValue;
            m_rb2D.velocity = new Vector2(m_direction * m_speed, m_rb2D.velocity.y);
        }

        if (m_rb2D.velocity.y < 0 && m_state != PLAYER_STATE.ATTACK)
        {
            m_rb2D.gravityScale = m_gravity2 / Physics2D.gravity.y;
            m_state = PLAYER_STATE.FALL;
            SetPlayerAnimation(PLAYER_ANIMATION.FALL);
        }

        if (m_rb2D.velocity.y < -200) { m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, -200.0f); }
    }

    void Jump()
    {
        if (InputManager.Instance.JumpButtonPressed && m_isGrounded)
        {
            m_rb2D.gravityScale = m_gravity1 / Physics2D.gravity.y;
            m_rb2D.velocity = new Vector2(m_rb2D.velocity.x, m_initialVelocityY);

            m_state = PLAYER_STATE.JUMP;
            SetPlayerAnimation(PLAYER_ANIMATION.JUMP);
            m_isGrounded = false;
        }
    }

    void FlipX()
    {
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        m_isFacingRight = !m_isFacingRight;
    }

    public void SetPlayerAnimation(PLAYER_ANIMATION p_animation)
    {
        ChangeAnimationState(m_animationHash[(int)p_animation]);
    }

    public PLAYER_ANIMATION GetPlayerAnimation() { return (PLAYER_ANIMATION)m_animationCurrentState; }
    public PLAYER_STATE State
    {
        get { return m_state; }
        set
        {
            m_state = value;
            if (m_state == PLAYER_STATE.ATTACK) { m_rb2D.velocity = new Vector2(m_speed, m_rb2D.velocity.y); }
        }
    }

    public float Gravity1
    {
        get { return m_gravity1; }
    }

    public float Gravity2
    {
        get { return m_gravity2; }
    }
    public bool IsGrounded
    {
        set { m_isGrounded = value; }
        get { return m_isGrounded; }
    }

    public Vector3 Speed { get { return m_rb2D.velocity; } }

}
