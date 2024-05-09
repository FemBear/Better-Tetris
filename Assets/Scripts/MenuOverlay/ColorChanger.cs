using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public TextMeshProUGUI text;
    public float duration = 2f;
    private Color[] rainbowColors;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        rainbowColors = new Color[]
{
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta
};
        StartCoroutine(ChangeColor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator ChangeColor()
    {
        while (true)
        {
            Color currentColor = rainbowColors[currentIndex];
            Color nextColor = rainbowColors[(currentIndex + 1) % rainbowColors.Length];
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = elapsedTime / duration;
                text.color = Color.Lerp(currentColor, nextColor, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            currentIndex = (currentIndex + 1) % rainbowColors.Length;
        }
    }
}
