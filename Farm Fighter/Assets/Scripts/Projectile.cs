using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float shootingForce = 10f;
    [SerializeField] float deSpawnTimer = 5f;
    [SerializeField] int playerDamage = 10;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        deSpawnTimer -= Time.deltaTime;
        if (deSpawnTimer <= 0)
        {
            MyEvents.xpGain.Invoke(5);
            Destroy(gameObject);
        }
    }

    public void SetColor(Color c)
    {
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }

    public void ShootInDirection(Vector3 dir)
    {
        rb.AddForce(dir.normalized * shootingForce);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().DamagePlayer(playerDamage);
            Destroy(gameObject);
        }
    }
}
