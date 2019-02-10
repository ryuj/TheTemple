using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbManager : MonoBehaviour
{
    private GameObject gameManager;

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
        Debug.Log("touch begin");
        if (Input.GetMouseButton(0) == false)
        {
            Debug.Log("return");
            return;
        }
        Debug.Log("touch");

        gameManager.GetComponent<GameManager>().GetOrb();
        Destroy(this.gameObject);
    }
}
