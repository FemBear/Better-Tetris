using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int ScoreOneLine = 40;
    public int ScoreTwoLine = 100;
    public int ScoreThreeLine = 300;
    public int ScoreFourLine = 1200;

    public TextMeshProUGUI hud_score;

    public int currentScore = 0;
    public int NumberOfRowsThisTurn = 0;


    AudioSource audioSource;
    public AudioClip rowDeleteSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        UpdateScore();
        UpdateUI();
    }

    public void UpdateScore()
    {
        if (NumberOfRowsThisTurn > 0)
        {
            if (NumberOfRowsThisTurn == 1)
            {
                ClearedOneLine();
            }
            else if (NumberOfRowsThisTurn == 2)
            {
                ClearedTwoLines();
            }
            else if (NumberOfRowsThisTurn == 3)
            {
                ClearedThreeLines();
            }
            else if (NumberOfRowsThisTurn == 4)
            {
                ClearedFourLines();
            }
            NumberOfRowsThisTurn = 0;
        }
    }

    void UpdateUI()
    {
        hud_score.text = currentScore.ToString();
    }

    void ClearedOneLine()
    { 
        PlayRowDeleteSound();
        currentScore += ScoreOneLine;
    }

    void ClearedTwoLines()
    {
        PlayRowDeleteSound();
        currentScore += ScoreTwoLine;
    }

    void ClearedThreeLines()
    {
        PlayRowDeleteSound();
        currentScore += ScoreThreeLine;
    }

    void ClearedFourLines()
    {
        PlayRowDeleteSound();
        currentScore += ScoreFourLine;
    }

    public void PlayRowDeleteSound()
    {
        audioSource.PlayOneShot(rowDeleteSound);
    }

    public void GameOver()
    {
        PlayerPrefs.SetInt("lastScore", currentScore);
        SceneManager.LoadScene("GameOver");
    }
}
