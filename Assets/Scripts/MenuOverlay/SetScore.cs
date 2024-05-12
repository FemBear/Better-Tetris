using UnityEngine;
using TMPro;

public class SetScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    void Start()
    { 
        if(scoreText != null)
        {
            if (PlayerPrefs.HasKey("lastScore"))
            {
                scoreText.text = "Score: " + PlayerPrefs.GetInt("lastScore");
            }
        }   
        if(highScoreText != null)
        {
            if (PlayerPrefs.HasKey("highScore"))
            {
                highScoreText.text = "HighScore: " + PlayerPrefs.GetInt("highScore", 25000);
            }
        }
    }
}
