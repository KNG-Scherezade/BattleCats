using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageManager : MonoBehaviour {

    public GameObject[] Pages;

    public GameObject left;

    public GameObject right;

    private int pageCounter;

    private int PageMax = 1;

    void Start()
    {
        pageCounter = 0;
        PageMax = Pages.Length;
        left.SetActive(false);
    }

    void Update()
    {
        foreach (GameObject gameObject in Pages)
        {
            gameObject.SetActive(false);
        }
        if (Pages[pageCounter])
        {
            Pages[pageCounter].SetActive(true);
        }
    }

    public void LeftPage()
    {
        pageCounter = (pageCounter - 1) % PageMax;
        print(pageCounter);
        if (pageCounter < 0)
        {
            pageCounter += PageMax;
        }
    }

    public void RightPage()
    {
        pageCounter = (pageCounter + 1) % PageMax;
        print(pageCounter);
        if (left.activeSelf == false)
        {
            left.SetActive(true);
        }
    }
          
}
