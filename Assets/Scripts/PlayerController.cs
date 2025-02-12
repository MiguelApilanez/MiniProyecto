using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("PlayerConfiguración")]
    private Rigidbody2D rb;
    public float speed = 5f;
    public float jumpForce = 6f;
    //public int health = 10;
    //public int maxHealth = 10;
    //public int minHealth = 0;
    //public int currentHealth;
    public bool isDeath = false;
    public bool isOnGround = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    public void Movement()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isOnGround = false;
            Debug.Log("Salto realizado");
        }
    }
}
