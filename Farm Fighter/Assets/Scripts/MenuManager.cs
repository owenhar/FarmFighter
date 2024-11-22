using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI scoreboard;
    // Start is called before the first frame update
    void Start()
    {
        RetriveLeaderboard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RetriveLeaderboard()
    {
        StartCoroutine(GetLeaderboard());
    }

    IEnumerator GetLeaderboard()
    {
        UnityWebRequest uwr = UnityWebRequest.Get("https://scoreboard.harrisowe.me/leaderboard/");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
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
                scoreText += entry.username + "-" + entry.score + "-" + TimeSurvivedToString(entry.timeSurvived) + "\n";
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
}
