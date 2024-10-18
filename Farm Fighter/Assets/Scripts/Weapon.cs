using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    float timer = 0;
    bool attackDisabled = false;
    [SerializeField] float attackDelay = 2.0f;
    Collider2D cd;

    List<Enemy> inRadius = new List<Enemy>();

    // Start is called before the first frame update
    void Start()
    {
        cd = gameObject.GetComponent<Collider2D>();
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
        if (Input.GetButtonDown("Fire1") && !attackDisabled)
        {
            List<GameObject> toKill = new List<GameObject>();
            foreach (Enemy enemy in inRadius)
            {
                toKill.Add(enemy.gameObject);
            }

            foreach (GameObject go in toKill)
            {
                Destroy(go);
                MyEvents.enemyKilled.Invoke();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Added Enemy");
            inRadius.Add(collision.gameObject.GetComponent<Enemy>());
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Enemy")
    //    {
    //        inRadius.Add(collision.gameObject.GetComponent<Enemy>());
    //    }
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Removed Enemy");
            inRadius.Remove(collision.gameObject.GetComponent<Enemy>());
        }
    }
}
