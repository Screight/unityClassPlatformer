using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float m_speed = 100.0f;
    Rigidbody2D m_rb2D;
    int m_direction = 1;



    void Awake() {
        m_rb2D = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_rb2D.velocity = new Vector2(m_direction * m_speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "floor")
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(int p_direction)
    {
        if (p_direction > 0) { m_direction = 1; }
        else { m_direction = -1; }
        
    }

}
