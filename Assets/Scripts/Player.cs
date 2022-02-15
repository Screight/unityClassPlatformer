using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_STATE { IDLE, RUN, JUMP, FALL, ATTACK, DEATH }
public enum PLAYER_ANIMATION { IDLE, RUN, JUMP, FALL, ATTACK, DEATH, STOP, BLOCK, HIT, LAST_NO_USE }

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

    Rigidbody2D m_rb2d;
    [SerializeField] float m_movementSpeed = 10.0f;
    bool m_isGrounded;

    SpriteRenderer m_spriteRenderer;
    bool m_isFacingRight;

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

        m_rb2d = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();

        m_isGrounded = true;
        m_isFacingRight = true;
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
                break;
            case PLAYER_STATE.RUN:
                Move(PLAYER_STATE.IDLE);
                break;
        }
    }

    void Move(PLAYER_STATE p_defaultState)
    {
        float direction = Input.GetAxisRaw("Horizontal");

        if(direction != 0)
        {
            transform.position += new Vector3(direction * m_movementSpeed * Time.deltaTime, 0, 0);
            
            if (m_isGrounded)
            {
                m_state = PLAYER_STATE.RUN;
                ChangeAnimationState(m_animationHash[(int)PLAYER_STATE.RUN]);
            }

            if(direction < 0 && m_isFacingRight){ m_spriteRenderer.flipX = true; m_isFacingRight = !m_isFacingRight; }
            if(direction > 0 && !m_isFacingRight){ m_spriteRenderer.flipX = false; m_isFacingRight = !m_isFacingRight; }
        }
        else
        {
            m_state = p_defaultState;
            ChangeAnimationState(m_animationHash[(int)PLAYER_STATE.IDLE]);
        }

    }

}
