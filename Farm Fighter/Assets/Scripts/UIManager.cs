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

    // Start is called before the first frame update
    void Start()
    {
        MyEvents.playerHealthUpdate.AddListener(UpdateHealthSlider);
        MyEvents.playerStaminaUpdate.AddListener(UpdateStaminaSlider);
        MyEvents.playerWaterUpdate.AddListener(UpdateWaterSlider);

    }

    // Update is called once per frame
    void Update()
    {
        
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



    
}
