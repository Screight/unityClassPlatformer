using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    enum ENEMY_STATE { PATROL, CHASE, ATTACK, IDLE, RETURN}

    [SerializeField] float m_speed = 40;
    Rigidbody2D m_rb2D;
    
    [SerializeField] Transform leftLimit;
    [SerializeField] Transform rightLimit;
    [SerializeField] float m_attackRange = 20;
    [SerializeField] float m_visionRange = 40;
    [SerializeField] float m_maxChaseRange = 40;

    bool m_isFacingRight;
    ENEMY_STATE m_state;
    [SerializeField] GameObject m_player;

    private void Awake()
    {
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_state = ENEMY_STATE.PATROL;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case ENEMY_STATE.PATROL:
                {
                    if (IsPlayerInVisionRange())
                    {
                        Debug.Log("Player in range");   
                    }
                    else { Patrol(); }
                }
                break;
        }
    }

    void Patrol()
    {
        m_rb2D.velocity = new Vector2(FacingDirection() * m_speed, 0);
        if(transform.position.x < leftLimit.position.x)
        {
            transform.position = new Vector3(leftLimit.position.x, leftLimit.position.y, leftLimit.position.z);
            m_isFacingRight = !m_isFacingRight;
        }
        else if (transform.position.x > rightLimit.position.x)
        {
            transform.position = new Vector3(rightLimit.position.x, rightLimit.position.y, rightLimit.position.z);
            m_isFacingRight = !m_isFacingRight;
        }
    }

    bool IsPlayerInVisionRange()
    {
        if(Mathf.Abs((transform.position.x - m_player.transform.position.x)) < m_visionRange) {
            if (Mathf.Abs((transform.position.y - m_player.transform.position.y)) < 8)
            {
                return true;
            }
        }
        return false;
    }

    int FacingDirection()
    {
        if (m_isFacingRight) return 1;
        else return -1;
    }

}
