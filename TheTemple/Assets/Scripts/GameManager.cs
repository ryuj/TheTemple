using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    private const int MAX_ORB = 10;
    private const int RESPAWN_TIME = 5;
    private const int MAX_LEVEL = 2;

    private const string KEY_SCORE = "SCORE";
    private const string KEY_LEVEL = "LEVEL";
    private const string KEY_ORB = "ORB";
    private const string KEY_TIME = "TIME";

    public GameObject orbPrefab;
    public GameObject smokePrefab;
    public GameObject kusudamaPrefab;
    public GameObject canvasGame;
    public GameObject textScore;
    public GameObject imageTemple;
    public GameObject imageMokugyo;

    public AudioClip getScoreSE;
    public AudioClip levelUpSE;
    public AudioClip clearSE;

    private int score = 0;
    private int nextScore = 100;

    private int currentOrb = 0;

    private int templeLevel = 0;

    private DateTime lastDateTime;

    private int[] nextScoreTable = new int[] { 10, 20, 30 };

    private AudioSource audioSource;

    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();

        score = PlayerPrefs.GetInt(KEY_SCORE, 0);
        templeLevel = PlayerPrefs.GetInt(KEY_LEVEL, 0);
        currentOrb = PlayerPrefs.GetInt(KEY_ORB, 10);

        for (int i = 0; i < currentOrb; ++i)
        {
            CreateOrb();
        }
        
        var time = PlayerPrefs.GetString(KEY_TIME, "");
        if (time == "")
        {
            lastDateTime = DateTime.UtcNow;
        }
        else
        {
            long temp = Convert.ToInt64(time);
            lastDateTime = DateTime.FromBinary(temp);
        }

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

        SaveGameData();
    }

    public void CreateOrb()
    {
        var orb = Instantiate(orbPrefab);
        orb.transform.SetParent(canvasGame.transform, false);
        orb.transform.localPosition = new Vector3(
            UnityEngine.Random.Range(-300, 300),
            UnityEngine.Random.Range(-140, -500));
        
        int kind = UnityEngine.Random.Range(0, templeLevel + 1);
        switch (kind)
        {
            case 0:
                orb.GetComponent<OrbManager>().SetKind(OrbManager.ORB_KIND.BLUE);
                break;
            case 1:
                orb.GetComponent<OrbManager>().SetKind(OrbManager.ORB_KIND.GREEN);
                break;
            case 2:
                orb.GetComponent<OrbManager>().SetKind(OrbManager.ORB_KIND.PURPLE);
                break;
        }
    }

    public void GetOrb(int getScore)
    {
        audioSource.PlayOneShot(getScoreSE);

        var stateInfo = imageMokugyo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
        if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.get@ImageMokugyo"))
        {
            imageMokugyo.GetComponent<Animator>().Play(stateInfo.fullPathHash, 0, 0.0f);
        }
        else
        {
            imageMokugyo.GetComponent<Animator>().SetTrigger("isGetScore");
        }

        if (score < nextScore)
        {
            score += getScore;

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
        }

        currentOrb--;

        SaveGameData();
    }
    
    private void RefreshScoreText()
    {
        textScore.GetComponent<Text>().text = "徳：" + score + " / " + nextScore;
    }

    private void TempleLevelUp()
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

    private void TempleLevelUpEffect()
    {
        var smoke = Instantiate(smokePrefab);
        smoke.transform.SetParent(canvasGame.transform, false);
        smoke.transform.SetSiblingIndex(2);

        audioSource.PlayOneShot(levelUpSE);

        Destroy(smoke, .5f);
    }

    private void ClearEffect()
    {
        var kusudama = Instantiate(kusudamaPrefab);
        kusudama.transform.SetParent(canvasGame.transform, false);

        audioSource.PlayOneShot(clearSE);
    }

    private void SaveGameData()
    {
        PlayerPrefs.SetInt(KEY_SCORE, score);
        PlayerPrefs.SetInt(KEY_LEVEL, templeLevel);
        PlayerPrefs.SetInt(KEY_ORB, currentOrb);
        PlayerPrefs.SetString(KEY_TIME, lastDateTime.ToBinary().ToString());

        PlayerPrefs.Save();
    }
}
