using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
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
    [SerializeField] TextMeshProUGUI scoreboard;
    [SerializeField] TMP_InputField input;





    bool displayed = false;
    int xp = 0;
    float time = 0;
    int kills = 0;
    int plants = 0;
    float timeSurvived = 0;
    bool gameRunning = true;
    bool hasSubmittedScore = false;

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
        if (gameRunning) // Stop player from accuring more xp after death
        {
            xp += xpGain;
            xpText.text = "XP: " + xp;
        }
    }

    void UpdateInventoryLabel(int index, int value)
    {
        inventoryLabels[index].text = value.ToString();
    }

    // Displays alert message on the top of the screen.
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
        if (gameRunning == false) {
            return; // Don't want this function running multiple times
        }
        panel.SetActive(true);
        gameRunning = false;
        timeText.text = TimeSurvivedToString(timeSurvived);
        killsText.text = kills.ToString();
        finalXPText.text = xp.ToString();
        plantText.text = plants.ToString();
        panel.SetActive(true);

        StartCoroutine(GetLeaderboard());
    }

    // The UnityWebRequest logic was taken mostly from https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Get.html and https://stackoverflow.com/questions/46003824/sending-http-requests-in-c-sharp-with-unity
    IEnumerator GetLeaderboard()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("https://scoreboard.harrisowe.me/leaderboard/");
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Failed to retrieve leaderboard");
        }
        else
        {
            Scores scores = JsonUtility.FromJson<Scores>(uwr.downloadHandler.text);
            Debug.Log(scores);

            string scoreText = "Scoreboard (Username - Score - Time Survived)\n";

            foreach (ScoreBoardEntry entry in scores.scores)
            {
                scoreText += entry.username + " - " + entry.score + " - " + TimeSurvivedToString(entry.timeSurvived) + "\n";
            }

            scoreboard.text = scoreText;

        }
    }

    string TimeSurvivedToString(float timeSurvived)
    {
        if (timeSurvived > 60)
        {
            return Mathf.Floor(timeSurvived / 60).ToString() + "m " + Mathf.Floor(timeSurvived % 60).ToString() + "s";

        }
        else
        {
            return Mathf.Floor(timeSurvived % 60).ToString() + "s";
        }
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SubmitScore()
    {
        if (hasSubmittedScore)
        {
            return;
        }
        hasSubmittedScore = true;
        StartCoroutine(PushScore(input.text));
    }

    // The UnityWebRequest logic was taken mostly from https://docs.unity3d.com/ScriptReference/Networking.UnityWebRequest.Get.html and https://stackoverflow.com/questions/46003824/sending-http-requests-in-c-sharp-with-unity
    IEnumerator PushScore(string username)
    {
        ScoreBoardEntry score = new ScoreBoardEntry(username, xp, timeSurvived);
        UnityWebRequest wr = UnityWebRequest.Post("https://scoreboard.harrisowe.me/leaderboard/", JsonUtility.ToJson(score), "application/json");
        yield return wr.SendWebRequest();

        if (wr.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(wr.error);
        }
        else
        {
            StartCoroutine(GetLeaderboard());
        }

    }
}


// I got some of this [Serializable] from in lab but also looked at https://docs.unity3d.com/2020.1/Documentation/Manual/JSONSerialization.html for information on JSON serilization for sending web requests.
[Serializable]
public class ScoreBoardEntry
{
    public ScoreBoardEntry(string username, int score, float timeSurvived)
    {
        this.username = username;
        this.score = score;
        this.timeSurvived = timeSurvived;
    }

    public string username;
    public int score;
    public float timeSurvived;
}

[Serializable]
public class Scores
{
    public ScoreBoardEntry[] scores;
}

