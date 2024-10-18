using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI healthText;
    // Start is called before the first frame update
    void Start()
    {
        MyEvents.playerHealthUpdate.AddListener(UpdateHealthText);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealthText(int health)
    {
        healthText.text = "Health: " + health + "/100";
    }

    
}
