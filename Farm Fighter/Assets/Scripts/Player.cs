using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] int maxHealth = 100;
    private int health = 100;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float staminaRegenPerSecond = 20f;
    [SerializeField] List<GameObject> slots;
    [SerializeField] List<GameObject> items;
    List<int> inventoryCount = new List<int>();
    [SerializeField] Color nonSelectedColor;
    [SerializeField] Color selectedColor;
    [SerializeField] List<Sprite> walkingSprites;
    SpriteRenderer sr;
    Rigidbody2D rb;
    private int selectedItem = 0;
    private float stamina;
    private int xp = 0;
    private int lvl = 1;
    private float damageMultiplier = 1.0f;
    private bool dead = false;
    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

  

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        health = maxHealth;
        stamina = maxStamina;
        int i = 0;
        foreach (GameObject _ in slots)
        {
            inventoryCount.Add(1);
            MyEvents.inventoryCountUpdate.Invoke(i, 1);
            i++;
        }
        MyEvents.updateInventoryCount.AddListener(ChangeValueOfItem);
        MyEvents.xpGain.AddListener(IncreaseXP);
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead)
        {
            Move();
        }
        if (stamina < maxStamina)
        {
            stamina = Mathf.Clamp(stamina + (Time.deltaTime * staminaRegenPerSecond), 0, maxStamina);
            MyEvents.playerStaminaUpdate.Invoke(stamina / maxStamina);
        }
        HandleInventorySwap();
    }

    private void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX, moveY);
        move.Normalize();

        sr.sprite = walkingSprites[0];
        if (moveX > 0)
        {
            sr.sprite = walkingSprites[1];
        } else if (moveX < 0)
        {
            sr.sprite = walkingSprites[3];
        } else if (moveY > 0)
        {
            sr.sprite = walkingSprites[2];
        }

        transform.Translate(move * moveSpeed * Time.deltaTime);
    }

    private void HandleInventorySwap()
    {
        bool changed = false;
        float mouseScrollDelta = Input.mouseScrollDelta.y;
        if (mouseScrollDelta < 0)
        {
            selectedItem = (selectedItem - 1) % slots.Count;
            if (selectedItem < 0) selectedItem += slots.Count;
            changed = true;
        }
        else if (mouseScrollDelta > 0)
        {
            selectedItem = (selectedItem + 1) % slots.Count;
            changed = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedItem = 0;
            changed = true;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            changed = true;
            selectedItem = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            changed = true;
            selectedItem = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            changed = true;
            selectedItem = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            changed = true;
            selectedItem = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            changed = true;
            selectedItem = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            changed = true;
            selectedItem = 6;
        }
        if (changed) {
            ChangeSelectedItem();
        }
    }

    public int DamagePlayer(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        MyEvents.playerHealthUpdate.Invoke((float) health / maxHealth);
        if (health <= 0)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            MyEvents.endGame.Invoke();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            dead = true;
            foreach (SpriteRenderer sr in gameObject.GetComponentsInChildren<SpriteRenderer>())
            {
                sr.enabled = false;
            }
        }
        return health;
    }

    public float GetStamina()
    {
        return stamina;
    }

    public void UseStamina(float used)
    {
        stamina = Mathf.Clamp(stamina - used, 0, maxStamina);
        MyEvents.playerStaminaUpdate.Invoke(stamina / maxStamina);
    }

    private void IncreaseXP(int amount)
    {
        xp += amount;
        if (xp / 100 >= lvl) {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        lvl += 1;
        if (lvl == 10)
        {
            MyEvents.displayAlertMessage.Invoke("LVL UP -- NEW WEAPON -- Located on the Upgrade Pedestal");
            MyEvents.spawnUpgradeWeapon.Invoke();
        }
        else if (lvl % 2 == 0)
        {
            damageMultiplier += 0.1f;
            MyEvents.displayAlertMessage.Invoke("LVL UP -- Damage Multiplier: " + damageMultiplier);
        } else
        {
            maxHealth += 20;
            MyEvents.displayAlertMessage.Invoke("LVL UP -- Max Health: " + maxHealth);
        }
    }

    public int GetAllItemCount(int index)
    {
        return inventoryCount[index];
    }

    public void ChangeValueOfItem(int index, int value)
    {
        inventoryCount[index] += value;
        MyEvents.inventoryCountUpdate.Invoke(index, inventoryCount[index]);
    }

    public void ChangeSelectedItem()
    {
        foreach (GameObject go in slots)
        {
            UnityEngine.UI.Image sr = go.GetComponent<UnityEngine.UI.Image>();
            sr.color = nonSelectedColor;
        }
        UnityEngine.UI.Image sr1 = slots[selectedItem].GetComponent<UnityEngine.UI.Image>();
        sr1.color = selectedColor;

        foreach (GameObject go in items)
        {
            go.SetActive(false);
        }

        items[selectedItem].SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pickup"))
        {
            IPickUp item = collision.gameObject.GetComponent<IPickUp>();
            item.PickUp(this);
        }
    }

}
