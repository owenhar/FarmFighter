using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI killsText;
    [SerializeField] TextMeshProUGUI finalXPText;
    [SerializeField] TextMeshProUGUI plantText;
    [SerializeField] GameObject panel;





    bool displayed = false;
    int xp = 0;
    float time = 0;
    int kills = 0;
    int plants = 0;
    float timeSurvived = 0;
    bool gameRunning = true;

    private void Awake()
    {
        MyEvents.playerHealthUpdate.AddListener(UpdateHealthSlider);
        MyEvents.playerStaminaUpdate.AddListener(UpdateStaminaSlider);
        MyEvents.playerWaterUpdate.AddListener(UpdateWaterSlider);
        MyEvents.xpGain.AddListener(UpdateXpText);
        MyEvents.inventoryCountUpdate.AddListener(UpdateInventoryLabel);
        MyEvents.displayAlertMessage.AddListener(DisplayAlertText);
        MyEvents.plantPlanted.AddListener(PlantPlanted);
        MyEvents.enemyKilled.AddListener(EnemyKilled);
        MyEvents.endGame.AddListener(GameEnded);
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        if (gameRunning)
        {
            timeSurvived += Time.deltaTime;
        }
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

    

    void PlantPlanted()
    {
        plants += 1;
    }

    void EnemyKilled()
    {
        kills += 1;
    }

    void GameEnded()
    {
        panel.SetActive(true);
        gameRunning = false;
        if (timeSurvived > 60)
        {
            timeText.text = Mathf.Floor(timeSurvived / 60).ToString() + "m " + Mathf.Floor(timeSurvived % 60).ToString() +"s";
            
        } else
        {
            timeText.text = Mathf.Floor(timeSurvived % 60).ToString() + "s";
        }
        killsText.text = kills.ToString();
        finalXPText.text = xp.ToString();
        plantText.text = plants.ToString();
        panel.SetActive(true);
    }

    public void EndGame()
    {
        Application.Quit();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }




}
