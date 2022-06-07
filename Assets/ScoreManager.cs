using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    
    public Text scoreText;
    public Text highscroeText;

    int score = 0;
    int highscore = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt("highscore", 0);
        scoreText.text = score.ToString() + " Points";
        highscroeText.text = "Highscore: " + highscore.ToString();
    }

    public void AddPoint()
    {
        score += 10;
        scoreText.text = score.ToString() + " Points";
        if (highscore < score)
        {
            PlayerPrefs.SetInt("highscore", score);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
