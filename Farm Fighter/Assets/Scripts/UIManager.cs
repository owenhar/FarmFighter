using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] Slider healthSlider;
    [SerializeField] Slider staminaSlider;
    [SerializeField] Slider waterSlider;
    [SerializeField] TextMeshProUGUI xpText;
    [SerializeField] List<TextMeshProUGUI> inventoryLabels;
    [SerializeField] TextMeshProUGUI alertText;
    [SerializeField] float alertTime = 2.0f;


    bool displayed = false;
    int xp = 0;
    float time = 0;

    private void Awake()
    {
        MyEvents.playerHealthUpdate.AddListener(UpdateHealthSlider);
        MyEvents.playerStaminaUpdate.AddListener(UpdateStaminaSlider);
        MyEvents.playerWaterUpdate.AddListener(UpdateWaterSlider);
        MyEvents.xpGain.AddListener(UpdateXpText);
        MyEvents.inventoryCountUpdate.AddListener(UpdateInventoryLabel);
        MyEvents.displayAlertMessage.AddListener(DisplayAlertText);
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (displayed)
        {
            time += Time.deltaTime;
            if (time >= alertTime)
            {
                alertText.gameObject.SetActive(false);
                displayed = false;
            }
        }
    }

    void UpdateHealthSlider(float health)
    {
        healthSlider.value = health;
    }

    void UpdateStaminaSlider(float stamina)
    {
        staminaSlider.value = stamina;
    }

    void UpdateWaterSlider(float water)
    {
        waterSlider.value = water;
    }

    void UpdateXpText(int xpGain)
    {
        xp += xpGain;
        xpText.text = "XP: " + xp;
    }

    void UpdateInventoryLabel(int index, int value)
    {
        inventoryLabels[index].text = value.ToString();
    }

    void DisplayAlertText(string message)
    {
        alertText.text = message;
        alertText.gameObject.SetActive(true);
        displayed = true;
        time = 0;
    }



    
}
