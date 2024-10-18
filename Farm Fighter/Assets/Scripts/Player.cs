using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] int maxHealth = 100;
    [SerializeField] float attackDelay = 2f;
    private int health = 100;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        health = maxHealth;
        MyEvents.playerHealthUpdate.Invoke(100);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, moveY);
        move.Normalize();

        transform.Translate(move * moveSpeed * Time.deltaTime);
    }

    public int DamagePlayer(int damage)
    {
        health -= damage;
        MyEvents.playerHealthUpdate.Invoke(health);
        return health;
    }


}
