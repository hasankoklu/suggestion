using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using static GameManager;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    public GameObject heroMenuRect;
    public GameObject ItemMenuRect;
    public GameObject myHeroMenuRect;
    public GameObject pieceItemMenuRect;
    public GameObject complateItemMenuRect;
    public GameObject selectedInfoRect;
    public GameObject suggestionRect;
    public GameObject extraItemSuggestionRect;
    //public GameObject generalSuggestionRect;
    public GameObject heroTeamSuggestionRect;
    public GameObject itemSuggestionRect;
    public GameObject addItemRect;
    public Text teamBuffText;

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
        //StartCoroutine(CanvasFixer());
    }

    IEnumerator CanvasFixer()
    {
        while (true)
        {
            RefreshListes();
            yield return new WaitForEndOfFrame();
        }
    }


    public void RefreshListes()
    {
        SetItemPiece();
        SetComplateItem();
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

    public void SetSelectedHeroIndex(int i)
    {
        GameManager.instance.selectedHeroIndex = i;
    }


    #region Dropdown OnValueChange

    //public void GenericTypeDropdownOnValueChange(int genericType)
    //{
    //    GameManager.instance.currentGenericType = genericType;
    //    SetHeroList();
    //}

    public void MyAreasDropdownOnValueChange(int index)
    {
        if (index == 0)
        {
            myHeroMenuRect.SetActive(true);
            pieceItemMenuRect.SetActive(false);
        }
        else
        {
            myHeroMenuRect.SetActive(false);
            pieceItemMenuRect.SetActive(true);
        }
    }

    #endregion

    #region ItemPieceAddRemove

    public void AddItemPieceToMyItemPieceList(int index)
    {
        GameManager.instance.myPieceItemList.Add(GameManager.instance.itemPieceList[index]);
        //SetMyItemPiece();

        if (extraItemSuggestionRect.activeInHierarchy)
            GameManager.instance.ItemSuggestButtonOnClick();
    }

    public void RemoveItemPieceToMyItemPieceList(int index)
    {
        GameManager.instance.myPieceItemList.Remove(
            GameManager.instance.myPieceItemList.Where(x => x.name == GameManager.instance.itemPieceList[index].name).FirstOrDefault()
            );
        //SetMyItemPiece();

        if (extraItemSuggestionRect.activeInHierarchy)
            GameManager.instance.ItemSuggestButtonOnClick();
    }

    #endregion

    #region GeneralListsArrangement

    void SetHeroList()
    {
        GameManager.instance.heroList = GameManager.instance.heroList.OrderBy(x => x.HeroGenericType).ThenByDescending(y => y.HeroFightStyleList[0]).ToList();

        for (int i = 0; i < heroMenuRect.transform.childCount; i++)
        {
            heroMenuRect.transform.GetChild(i).gameObject.SetActive(false);
        }
        
        if (GameManager.instance.myHeroList.Count > 0)
        {
            for (int i = 0; i < GameManager.instance.suggestionHeroList.Count(); i++)
            {
                heroMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.suggestionHeroList[i].image;
                heroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.instance.suggestionHeroList[i].name;


                Hero hero = GameManager.instance.suggestionHeroList[i];

                heroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                heroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => GameManager.instance.AddHeroToMyHeroList(hero));

                heroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < GameManager.instance.heroList.Count(); i++)
            {
                heroMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.heroList[i].image;
                heroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroList[i].name;


                Hero hero = GameManager.instance.heroList[i];

                heroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                heroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => GameManager.instance.AddHeroToMyHeroList(hero));

                heroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
            }
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

        //GameManager.instance.selectedHeroIndex = index;

        //GameManager.instance.MakeSuggestionforSelectedHero();
    }

    public void SetMyHeroList()
    {
        for (int i = 0; i < myHeroMenuRect.transform.childCount; i++)
        {
            myHeroMenuRect.transform.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.instance.myHeroList.Count; i++)
        {
            myHeroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            int temp = i;
            myHeroMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => SetSelectedInfoList(temp));
            myHeroMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.myHeroList[i].cardImage;
            myHeroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.instance.myHeroList[i].name;

            GameManager.instance.SetHeroItems(i);
            myHeroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (GameManager.instance.myHeroList.Count == 1)
        {
            SetSelectedInfoList(0);
        }
    }

    void SetItemPiece()
    {
        for (int i = 0; i < pieceItemMenuRect.transform.childCount; i++)
        {

            pieceItemMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();

            ItemPiece itemPiece = GameManager.instance.itemPieceList[i];
            pieceItemMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => GameManager.instance.TakePieceItem(itemPiece));

            pieceItemMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.itemPieceList[i].image;

            if (GameManager.instance.myPieceItemList.Where(x => x.name == GameManager.instance.itemPieceList[i].name).FirstOrDefault() != null)
                pieceItemMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text =
                    GameManager.instance.myPieceItemList.Where(x => x.name == GameManager.instance.itemPieceList[i].name).Count().ToString();
            else
            {
                pieceItemMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "0";
            }
        }

    }

    void SetComplateItem()
    {
        for (int i = 0; i < complateItemMenuRect.transform.childCount; i++)
        {

            complateItemMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            Item item = GameManager.instance.itemList[i];
            complateItemMenuRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => GameManager.instance.TakeItem(item));

            complateItemMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.itemList[i].image;

            if (GameManager.instance.myComplateItemList.Where(x => x.name == GameManager.instance.itemList[i].name).FirstOrDefault() != null)
                complateItemMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text =
                    GameManager.instance.myComplateItemList.Where(x => x.name == GameManager.instance.itemList[i].name).Count().ToString();
            else
            {
                complateItemMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "0";
            }
        }
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
