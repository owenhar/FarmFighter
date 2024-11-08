using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    Transform player;
    Collider2D cd;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float attackDelay = 2.0f;
    [SerializeField] int damage = 10;
    [SerializeField] float maxHealth = 2f;
    [SerializeField] List<GameObject> drops;
    float health;
    float timer = 0;
    bool attackDisabled = true;

    Slider healthBar;
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cd = gameObject.GetComponent<Collider2D>();
        healthBar = gameObject.GetComponentInChildren<Slider>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackDisabled)
        {
            timer += Time.deltaTime;
            if (timer >= attackDelay)
            {
                timer = 0;
                Attack();
            }
        }
        TrackPlayer();
    }

    float TrackPlayer()
    {
        if (player == null) return 1000;
        Vector3 playerVec = (player.position - transform.position);
        Vector3 playerDir = playerVec.normalized;

        transform.Translate(playerDir * moveSpeed * Time.deltaTime);
        return playerVec.magnitude;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().DamagePlayer(damage);
            //cd.enabled = false;
            attackDisabled = true;
        }
    }

    public void DamageEnemy(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0 )
        {

            MyEvents.xpGain.Invoke(15);
            DropItem();
            Destroy(gameObject);
        }
    }

    void UpdateHealthBar()
    {
        healthBar.value = health / maxHealth;
    }

    void DropItem()
    {
        int randChoice = Random.Range(0, 3);
        if (randChoice == 0) {
            Instantiate(drops[0], transform.position, Quaternion.identity);
        } else if (randChoice == 1)
        {

        } else
        {
            int randIndex = Random.Range(0, drops.Count);
            Instantiate(drops[randIndex], transform.position, Quaternion.identity);
        }
    }

    public void Attack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);
        foreach (Collider2D c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                c.gameObject.GetComponent<Player>().DamagePlayer(10);
            }
        }
    }

}
