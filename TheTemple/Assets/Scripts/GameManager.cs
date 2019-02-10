using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int MAX_ORB = 10;

    public GameObject orbPrefab;
    public GameObject canvasGame;
    public GameObject textScore;

    private int score = 0;
    private int nextScore = 100;


    void Start()
    {
        for (int i = 0; i < MAX_ORB; ++i)
        {
            CreateOrb();
        }

        RefreshScoreText();
    }

    void Update()
    {
        
    }

    public void CreateOrb()
    {
        var orb = Instantiate(orbPrefab);
        orb.transform.SetParent(canvasGame.transform, false);
        orb.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(-300, 300),
            UnityEngine.Random.Range(-140, -500));
    }

    public void GetOrb()
    {
        score += 1;
        RefreshScoreText();
    }
    
    void RefreshScoreText()
    {
        textScore.GetComponent<Text>().text = "徳：" + score + " / " + nextScore;
    }
}
