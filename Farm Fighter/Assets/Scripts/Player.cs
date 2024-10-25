using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] int maxHealth = 100;
    [SerializeField] float attackDelay = 2f;
    private int health = 100;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float staminaRegenPerSecond = 20f;
    [SerializeField] List<SpriteRenderer> slots;
    [SerializeField] List<GameObject> items;
    [SerializeField] Color nonSelectedColor;
    [SerializeField] Color selectedColor;
    private int selectedItem = 0;
    private float stamina;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        health = maxHealth;
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
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
        if (changed) {
            ChangeSelectedItem();
        }
    }

    public int DamagePlayer(int damage)
    {
        health -= damage;
        MyEvents.playerHealthUpdate.Invoke((float) health / maxHealth);
        if (health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    public void ChangeSelectedItem()
    {
        foreach (SpriteRenderer sr in slots)
        {
            sr.color = nonSelectedColor;
        }
        Debug.Log(selectedItem);
        slots[selectedItem].color = selectedColor;

        foreach (GameObject go in items)
        {
            go.SetActive(false);
        }

        items[selectedItem].SetActive(true);
    }


}
