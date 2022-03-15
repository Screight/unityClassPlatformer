using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Groundcheck : MonoBehaviour
{
    Player playerScript;
    Rigidbody2D playerRigidBody;

    void Start()
    {
        playerScript = GetComponentInParent<Player>();
        playerRigidBody = GetComponentInParent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Idle si estás en suelo || DENTRO de plataforma NO saltando/cayendo
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "enemy"))
        {
            playerScript.IsGrounded = true;
            playerRigidBody.gravityScale = playerScript.Gravity1 / Physics2D.gravity.y;
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);

            if (playerScript.State != PLAYER_STATE.RUN && playerScript.State != PLAYER_STATE.ATTACK)
            {
                if (playerScript.State == PLAYER_STATE.FALL)
                {
                    playerScript.State = PLAYER_STATE.IDLE;
                    playerScript.SetPlayerAnimation(PLAYER_ANIMATION.LAND);
                }

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "floor" || collision.gameObject.tag == "platform"))
        {
            playerScript.IsGrounded = false;

            playerRigidBody.gravityScale = playerScript.Gravity1 / Physics2D.gravity.y;
        } 
    }
}


