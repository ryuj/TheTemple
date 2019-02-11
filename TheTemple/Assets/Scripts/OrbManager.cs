using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using DG.Tweening;

public class OrbManager : MonoBehaviour
{
    private GameObject gameManager;

    public Sprite[] orbPicture = new Sprite[3];

    public enum ORB_KIND
    {
        BLUE,
        GREEN,
        PURPLE,
    }

    private ORB_KIND orbKind;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchOrb()
    {
        if (Input.GetMouseButton(0) == false)
        {
            return;
        }

        var rect = GetComponent<RectTransform>();

        Vector3[] path = {
            new Vector3(rect.localPosition.x * 1.5f, 300f, 0f),
            new Vector3(0f, 150f, 0f),
        };

        rect.DOLocalPath(path, .5f, PathType.CatmullRom)
            .SetEase(Ease.OutQuad)
            .OnComplete(AddOrbPoint);

        rect.DOScale(
            new Vector3(.5f, .5f, 0f),
            .5f
        );
    }

    private void AddOrbPoint()
    {
        switch (orbKind)
        {
            case ORB_KIND.BLUE:
                gameManager.GetComponent<GameManager>().GetOrb(1);
                break;
            case ORB_KIND.GREEN:
                gameManager.GetComponent<GameManager>().GetOrb(5);
                break;
            case ORB_KIND.PURPLE:
                gameManager.GetComponent<GameManager>().GetOrb(10);
                break;
        }

        Destroy(this.gameObject);
    }

    public void SetKind(ORB_KIND kind)
    {
        orbKind = kind;

        switch (orbKind)
        {
            case ORB_KIND.BLUE:
                GetComponent<Image>().sprite = orbPicture[0];
                break;
            case ORB_KIND.GREEN:
                GetComponent<Image>().sprite = orbPicture[1];
                break;
            case ORB_KIND.PURPLE:
                GetComponent<Image>().sprite = orbPicture[2];
                break;
        }
    }
}
