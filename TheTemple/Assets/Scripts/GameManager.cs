using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    private const int MAX_ORB = 10;
    private const int RESPAWN_TIME = 1;
    private const int MAX_LEVEL = 2;

    public GameObject orbPrefab;
    public GameObject smokePrefab;
    public GameObject kusudamaPrefab;
    public GameObject canvasGame;
    public GameObject textScore;
    public GameObject imageTemple;

    private int score = 0;
    private int nextScore = 100;

    private int currentOrb = 0;

    private int templeLevel = 0;

    private DateTime lastDateTime;

    private int[] nextScoreTable = new int[] { 10, 10, 10 };

    void Start()
    {
        currentOrb = 10;

        for (int i = 0; i < currentOrb; ++i)
        {
            CreateOrb();
        }

        lastDateTime = DateTime.UtcNow;
        nextScore = nextScoreTable[templeLevel];

        imageTemple.GetComponent<TempleManager>().SetTemplePicture(templeLevel);
        imageTemple.GetComponent<TempleManager>().SetTempleScale(templeLevel, nextScore);

        RefreshScoreText();
    }

    void Update()
    {
        if (currentOrb < MAX_ORB)
        {
            TimeSpan timeSpan = DateTime.UtcNow - lastDateTime;

            if (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME))
            {
                while (timeSpan >= TimeSpan.FromSeconds(RESPAWN_TIME))
                {
                    CreateNewOrb();
                    timeSpan -= TimeSpan.FromSeconds(RESPAWN_TIME);
                }
            }
        }
    }

    public void CreateNewOrb()
    {
        lastDateTime = DateTime.UtcNow;
        if (currentOrb >= MAX_ORB)
        {
            return;
        }

        CreateOrb();
        currentOrb++;
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

        if (score >= nextScore)
        {
            score = nextScore;
        }

        TempleLevelUp();
        RefreshScoreText();
        imageTemple.GetComponent<TempleManager>().SetTempleScale(score, nextScore);

        if ((score == nextScore) && (templeLevel == MAX_LEVEL))
        {
            ClearEffect();
        }

        currentOrb--;
    }
    
    void RefreshScoreText()
    {
        textScore.GetComponent<Text>().text = "徳：" + score + " / " + nextScore;
    }

    void TempleLevelUp()
    {
        if (score >= nextScore)
        {
            if (templeLevel < MAX_LEVEL)
            {
                templeLevel++;
                score = 0;

                TempleLevelUpEffect();

                nextScore = nextScoreTable[templeLevel];
                imageTemple.GetComponent<TempleManager>().SetTemplePicture(templeLevel);
            }
        }
    }

    void TempleLevelUpEffect()
    {
        var smoke = Instantiate(smokePrefab);
        smoke.transform.SetParent(canvasGame.transform, false);
        smoke.transform.SetSiblingIndex(2);

        Destroy(smoke, .5f);
    }

    void ClearEffect()
    {
        var kusudama = Instantiate(kusudamaPrefab);
        kusudama.transform.SetParent(canvasGame.transform, false);
    }
}
