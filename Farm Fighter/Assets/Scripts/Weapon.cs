using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{

    float timer = 0;
    bool attackDisabled = false;
    [SerializeField] float attackDelay = 2.0f;
    [SerializeField] Sprite upgradeSprite;

    CircleCollider2D cd;
    SpriteRenderer sr;
    Player p;
    Animator animator;
    private float weaponDamageMultipler = 1f;
    bool isUpgraded = false;

    // Start is called before the first frame update
    void Start()
    {
        cd = gameObject.GetComponent<CircleCollider2D>();
        p = gameObject.GetComponentInParent<Player>();
        animator = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();

        MyEvents.upgradeWeapon.AddListener(UpgradeWeapon);
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
            if (!isUpgraded)
            {
                animator.SetTrigger("Swing");
            } else
            {
                animator.SetTrigger("SwingSword");
            }
            List<GameObject> toKill = new List<GameObject>();
            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, cd.radius);
            foreach (Collider2D collision in collisions)
            {
                if (collision.tag == "Enemy")
                {
                    collision.GetComponent<Enemy>().DamageEnemy(weaponDamageMultipler * p.GetDamageMultiplier());
                }
            }
            p.UseStamina(10);
        }
    }

    private void UpgradeWeapon()
    {
        sr.sprite = upgradeSprite;
        transform.rotation = Quaternion.identity;
        sr.flipX = false;
        weaponDamageMultipler = 1.5f;
        isUpgraded = true;
    }
}
