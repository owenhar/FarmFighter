using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{

    float timer = 0;
    bool attackDisabled = false;
    [SerializeField] float attackDelay = 2.0f;
    CircleCollider2D cd;
    Player p;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        cd = gameObject.GetComponent<CircleCollider2D>();
        p = gameObject.GetComponentInParent<Player>();
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackDisabled)
        {
            timer += Time.deltaTime;
            if (timer > 0.25)
            {
                cd.enabled = false;
            }
            if (timer >= attackDelay)
            {
                timer = 0;
                attackDisabled = false;
            }
        }
        if (Input.GetButtonDown("Fire1") && !attackDisabled && p.GetStamina() > 10)
        {
            animator.SetTrigger("Swing");
            List<GameObject> toKill = new List<GameObject>();
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, cd.radius);
            foreach (Collider2D collision in collisions)
            {
                Debug.Log("found a " + collision.tag);
                if (collision.tag == "Enemy")
                {
                    collision.GetComponent<Enemy>().DamageEnemy(1);
                }
            }
            p.UseStamina(10);
            
        }
    }
}
