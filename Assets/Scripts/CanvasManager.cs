using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject heroMenuRect;
    public GameObject myHeroMenuRect;
    public GameObject myItemPieceMenuRect;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetMyHeroList();
    }

    #region Dropdown OnValueChange

    public void GenericTypeDropdownOnValueChange(int genericType)
    {
        GameManager.instance.currentGenericType = genericType;
        SetHeroList();
    }

    public void MyAreasDropdownOnValueChange(int index)
    {
        if (index == 0)
        {
            myHeroMenuRect.SetActive(true);
            myItemPieceMenuRect.SetActive(false);
        }
        else
        {
            myHeroMenuRect.SetActive(false);
            myItemPieceMenuRect.SetActive(true);
        }
    }

    #endregion

    #region GeneralListsArrangement

    void SetHeroList()
    {
        for (int i = 0; i < heroMenuRect.transform.childCount; i++)
        {
            heroMenuRect.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.instance.heroList.Where(x => x.HeroGenericType.GetHashCode() == GameManager.instance.currentGenericType).Count(); i++)
        {
            heroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    #endregion

    #region MyListsArrangement

    public void SetMyHeroList()
    {
        for (int i = 0; i < myHeroMenuRect.transform.childCount; i++)
        {
            myHeroMenuRect.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.instance.myHeroList.Count; i++)
        {
            myHeroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    //public void SetMyItemPieceList()
    //{
    //    for (int i = 0; i < myHeroMenuRect.transform.childCount; i++)
    //    {
    //        myItemPieceMenuRect.transform.GetChild(i).gameObject.SetActive(false);
    //    }

    //    for (int i = 0; i < GameManager.instance.myItemList.Count; i++)
    //    {
    //        myItemPieceMenuRect.transform.GetChild(i).gameObject.SetActive(true);
    //    }
    //}

    #endregion

    //Vector2 origin;


    //public float verticalAutosize;
    //public float horizontalAutosize;
    //public float topPanelHeight;
    //public float topPanelWidth;
    //public float bottomPanelHeight;
    //public float bottomPanelWidth;
    //public float rightPanelHeight;
    //public float rightPanelWidth;
    //public float leftPanelHeight;
    //public float leftPanelWidth;
    //private void Awake()
    //{
    //    origin = new Vector2(Screen.width / 2, Screen.height / 2);
    //}

    //void Start()
    //{
    //    //heroMenuRect.GetComponent<RectTransform>().localScale = new Vector2(0.8f, 0.3f);
    //    //heroMenuRect.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,
    //    //    Screen.height / 2 - heroMenuRect.GetComponent<RectTransform>().localScale.y / 2 * Screen.height, 0f);
    //    //PanelSpawner();
    //}

    ////bir sonraki full değilse düşür


    //void PanelSpawner()
    //{
    //    for (int i = 0; i < 4; i++)
    //    {
    //        if (panelInfoList.Where(x => x.panelPosition.GetHashCode() == i).Count() > 1)
    //        {
    //            Debug.Log("Max panel count is 1 for each side. Please make sure is the panel list has 1 panel position for each side!");
    //            return;
    //        }
    //    }

    //    foreach (PanelInfo panel in panelInfoList.OrderBy(x => x.panelPosition.GetHashCode()).ToList())
    //    {
    //        GetComponent<RectTransform>().localScale = new Vector2(0.8f, 0.3f);

    //        GameObject go = Instantiate(defaultPanelGO);
    //        //go.transform.position = new Vector3(Screen.width / 2, Screen.height / 2 +, 0f);
    //        go.GetComponent<RectTransform>().localScale = new Vector2(1f, panel.sizePersantace / 100f);


    //        Debug.Log(panel.panelPosition.GetHashCode());
    //    }
    //}



    //void converter()
    //{

    //}


    //[Serializable]
    //public class PanelInfo
    //{
    //    public bool isActive;
    //    public float sizePersantace;
    //    public PanelPosition panelPosition;
    //    public ScaleType scaleType;
    //}

    //public enum PanelPosition { Top, Right, Bottom, Left };
    //public enum ScaleType { FullScale, AutoScale };
}
