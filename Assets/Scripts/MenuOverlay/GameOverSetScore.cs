using UnityEngine;
using TMPro;

public class GameOverSetScore : MonoBehaviour
{
    TextMeshProUGUI scoreText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        if (PlayerPrefs.HasKey("lastScore"))
        {
            scoreText.text = "Score: " + PlayerPrefs.GetInt("lastScore");
        }
        else
        {
            scoreText.text = "Score: N/A";
        }
    }
}
