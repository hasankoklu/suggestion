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
    public GameObject ItemMenuRect;
    public GameObject myHeroMenuRect;
    public GameObject myItemPieceMenuRect;
    public GameObject selectedInfoRect;
    public GameObject suggestionRect;
    public GameObject extraItemSuggestionRect;
    //public GameObject generalSuggestionRect;
    public GameObject heroTeamSuggestionRect;
    public GameObject itemSuggestionRect;

    public Button removeHeroButton;

    public Text selectedHeroNameText;
    //public GameObject bestItemsRect;
    public GameObject myHeroItemsRect;


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
        RefreshListes();
    }

    public void RefreshListes()
    {
        SetMyItemPiece();
        SetHeroList();
        SetMyHeroList();

        if (extraItemSuggestionRect.activeInHierarchy)
            GameManager.instance.ItemSuggestButtonOnClick();
    }

    public void RemoveHeroButtonOnClick(int index)
    {
        GameManager.instance.myHeroList.Remove(GameManager.instance.myHeroList[index]);
        //selectedInfoRect.SetActive(false);
        SetMyHeroList();
        if (GameManager.instance.myHeroList != null)
        {
            SetSelectedInfoList(0);
        }
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

    #region ItemPieceAddRemove

    public void AddItemPieceToMyItemPieceList(int index)
    {
        GameManager.instance.myItemPieceList.Add(GameManager.instance.itemPieceList[index]);
        SetMyItemPiece();

        if (extraItemSuggestionRect.activeInHierarchy)
            GameManager.instance.ItemSuggestButtonOnClick();
    }

    public void RemoveItemPieceToMyItemPieceList(int index)
    {
        GameManager.instance.myItemPieceList.Remove(
            GameManager.instance.myItemPieceList.Where(x => x.name == GameManager.instance.itemPieceList[index].name).FirstOrDefault()
            );
        SetMyItemPiece();

        if (extraItemSuggestionRect.activeInHierarchy)
            GameManager.instance.ItemSuggestButtonOnClick();
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
            heroMenuRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroList.Where(x => x.HeroGenericType.GetHashCode() == GameManager.instance.currentGenericType).ToList()[i].image;
            heroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroList.Where(x => x.HeroGenericType.GetHashCode() == GameManager.instance.currentGenericType).ToList()[i].name;


            GameManager.Hero hero = GameManager.instance.heroList.Where(x => x.HeroGenericType.GetHashCode() == GameManager.instance.currentGenericType).ToList()[i];

            heroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            heroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => GameManager.instance.AddHeroToMyHeroList(hero));

            heroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    #endregion

    #region MyListsArrangement

    public void SetSelectedInfoList(int index)
    {
        extraItemSuggestionRect.SetActive(false);
        heroTeamSuggestionRect.SetActive(false);

        suggestionRect.SetActive(true);
        //extraItemSuggestionRect.SetActive(false);

        GameManager.instance.selectedHeroIndex = index;

        GameManager.instance.MakeSuggestionforSelectedHero();
    }

    public void SetMyHeroList()
    {
        for (int i = 0; i < myHeroMenuRect.transform.childCount; i++)
        {
            myHeroMenuRect.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.instance.myHeroList.Count; i++)
        {
            //Debug.Log("i : " + i);
            myHeroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            int temp = i;
            myHeroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => SetSelectedInfoList(temp));
            myHeroMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.myHeroList[i].image;
            myHeroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.instance.myHeroList[i].name;
            myHeroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (GameManager.instance.myHeroList.Count == 1)
        {
            SetSelectedInfoList(0);
        }
    }

    void SetMyItemPiece()
    {
        for (int i = 0; i < myItemPieceMenuRect.transform.childCount; i++) // Look Here
        {
            myItemPieceMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.itemPieceList[i].image;

            if (GameManager.instance.myItemPieceList.Where(x => x.name == GameManager.instance.itemPieceList[i].name).FirstOrDefault() != null)
                myItemPieceMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text =
                    GameManager.instance.myItemPieceList.Where(x => x.name == GameManager.instance.itemPieceList[i].name).Count().ToString();
            else
            {
                myItemPieceMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "0";
            }
        }

        GameManager.instance.MakeSuggestionforSelectedHero();
    }


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
}
