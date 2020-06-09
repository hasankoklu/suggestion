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
    public GameObject currentTeamBuffRect;
    public GameObject askWinLoseRect;
    public GameObject addItemRect;
    public Text teamBuffText;
    public Text PlayerLevelText;

    public GameObject HeroButtonPrefab;
    public GameObject HeroCardButtonPrefab;

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
        for (int t = 0; t < currentTeamBuffRect.transform.childCount; t++)
        {
            currentTeamBuffRect.transform.GetChild(t).gameObject.SetActive(false);
        }

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
        RefreshListes();
    }

    public void SetSelectedHeroIndex(int i)
    {
        GameManager.instance.selectedHeroIndex = i;
    }

    public void PlayerLevelIncreaseButtonClick()
    {
        GameManager.instance.playerLevel++;
        PlayerLevelText.text = GameManager.instance.playerLevel.ToString();
    }

    public void PlayerLevelDecreaseButtonClick()
    {
        if (GameManager.instance.playerLevel > 1)
        {
            GameManager.instance.playerLevel--;
            PlayerLevelText.text = GameManager.instance.playerLevel.ToString();
        }
    }

    List<BestHeroTeam> tempBestHeroTeamList = new List<BestHeroTeam>();

    public void TeamWon()
    {

        LoadBestTeamList();
        tempBestHeroTeamList = GameManager.instance.bestHeroTeamList;
        foreach (Hero hero in GameManager.instance.myHeroList)
        {
            tempBestHeroTeamList = GameManager.instance.bestHeroTeamList.Where(x => x.heroList.Contains(hero)).ToList();
        }

        if (tempBestHeroTeamList.Count > 0)
        {
            GameManager.instance.bestHeroTeamList.Where(x => x == tempBestHeroTeamList.FirstOrDefault()).FirstOrDefault().winCount++;
        }
        else
        {
            BestHeroTeam tempBestHeroTeam = new BestHeroTeam();
            tempBestHeroTeam.winCount = 1;
            tempBestHeroTeam.heroList = GameManager.instance.myHeroList;

            GameManager.instance.theBestTeamList.Add(tempBestHeroTeam);

        }
        SaveBestTeamList();
    }

    public void TeamLost()
    {
        LoadBestTeamList();
        tempBestHeroTeamList = GameManager.instance.bestHeroTeamList;
        foreach (Hero hero in GameManager.instance.myHeroList)
        {
            tempBestHeroTeamList = GameManager.instance.bestHeroTeamList.Where(x => x.heroList.Contains(hero)).ToList();
        }

        if (tempBestHeroTeamList.Count > 0)
        {
            GameManager.instance.bestHeroTeamList.Where(x => x == tempBestHeroTeamList.FirstOrDefault()).FirstOrDefault().winCount--;
        }
        else
        {
            BestHeroTeam tempBestHeroTeam = new BestHeroTeam();
            tempBestHeroTeam.loseCount = 1;
            tempBestHeroTeam.heroList = GameManager.instance.myHeroList;

            GameManager.instance.theBestTeamList.Add(tempBestHeroTeam);
        }
        SaveBestTeamList();
    }

    public void SaveBestTeamList()
    {
        Debug.Log("GameManager.instance.bestTeamList Kayıt edilecek.");
    }

    public void LoadBestTeamList()
    {
        Debug.Log("GameManager.instance.bestTeamList Geri Yüklenecek.");
    }

    public void IsHoldButtonClick()
    {
        isHold = true;
    }

    bool isHold = false;
    public IEnumerator AskWinLoseSystem()
    {
        while (GameManager.instance.myHeroList.Count > 1)
        {
            if (isHold)
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(1000, 1200));
                isHold = false;
            }
            else
            {
                yield return new WaitForSeconds(UnityEngine.Random.Range(150, 210));
            }
            askWinLoseRect.SetActive(true);
        }
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
        GameManager.instance.heroList = GameManager.instance.heroList.OrderBy(x => x.level).ThenBy(x => x.HeroGenericType).ToList();

        for (int i = 0; i < heroMenuRect.transform.childCount; i++)
        {
            Destroy(heroMenuRect.transform.GetChild(i).gameObject);
        }

        if (GameManager.instance.myHeroList.Count > 0)
        {

            for (int i = 0; i < GameManager.instance.suggestionHeroList.Count(); i++)
            {
                GameObject go = new GameObject();
                go = Instantiate(HeroButtonPrefab);
                go.transform.SetParent(heroMenuRect.transform);
                //go.transform.position = Vector3.zero;

                heroMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.suggestionHeroList[i].image;
                heroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.instance.suggestionHeroList[i].name;
                heroMenuRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[GameManager.instance.suggestionHeroList[i].HeroGenericType].image;
                heroMenuRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[GameManager.instance.suggestionHeroList[i].HeroFightStyleList[0]].image;

                heroMenuRect.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = "%" + GameManager.instance.heroList[i].winRate.ToString();


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
                GameObject go = Instantiate(HeroButtonPrefab);
                go.transform.SetParent(heroMenuRect.transform);
                go.transform.position = Vector3.zero;

                heroMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.heroList[i].image;
                heroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroList[i].name;
                heroMenuRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[GameManager.instance.heroList[i].HeroGenericType].image;

                heroMenuRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[GameManager.instance.heroList[i].HeroFightStyleList[0]].image;

                heroMenuRect.transform.GetChild(i).GetChild(3).GetComponent<Text>().text = "%" + GameManager.instance.heroList[i].winRate.ToString();


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

        //suggestionRect.SetActive(true);
        //extraItemSuggestionRect.SetActive(false);

        //GameManager.instance.selectedHeroIndex = index;

        //GameManager.instance.MakeSuggestionforSelectedHero();
    }

    public void SetMyHeroList()
    {
        for (int i = 0; i < myHeroMenuRect.transform.childCount; i++)
        {
            Destroy(myHeroMenuRect.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < GameManager.instance.myHeroList.Count; i++)
        {
            Debug.Log("MyHeroList Count : " + GameManager.instance.myHeroList.Count);
            GameObject go = new GameObject();
            go = Instantiate(HeroCardButtonPrefab);
            go.transform.SetParent(myHeroMenuRect.transform);

            myHeroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
            int temp = i;
            myHeroMenuRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => RemoveHeroButtonOnClick(temp));

            myHeroMenuRect.transform.GetChild(i).GetComponent<Image>().sprite = GameManager.instance.myHeroList[i].cardImage;
            myHeroMenuRect.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.myHeroList[i].name;

            myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[GameManager.instance.myHeroList[i].HeroGenericType].image;
            myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroGenericTypeList[GameManager.instance.myHeroList[i].HeroGenericType].name;

            myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[GameManager.instance.myHeroList[i].HeroFightStyleList[0]].image;
            myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroFightStyleList[GameManager.instance.myHeroList[i].HeroFightStyleList[0]].name;

            if (GameManager.instance.myHeroList[i].HeroFightStyleList[1] != 0)
            {
                myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[GameManager.instance.myHeroList[i].HeroFightStyleList[1]].image;
                myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroFightStyleList[GameManager.instance.myHeroList[i].HeroFightStyleList[1]].name;

                myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                myHeroMenuRect.transform.GetChild(i).GetChild(2).GetChild(1).GetChild(2).gameObject.SetActive(false);
            }

            GameManager.instance.currentBuffNameList.Clear();
            for (int t = 0; t < currentTeamBuffRect.transform.childCount; t++)
            {
                currentTeamBuffRect.transform.GetChild(t).gameObject.SetActive(false);
            }

            for (int t = 0; t < GameManager.instance.heroGenericTypeList.Count; t++)
            {
                if (GameManager.instance.myHeroList.Where(x => x.HeroGenericType == t).Count() / GameManager.instance.heroGenericTypeList[t].exponent >= 1)
                {
                    for (int j = 0; j < (int)(GameManager.instance.myHeroList.Where(x => x.HeroGenericType == t).Count() / GameManager.instance.heroGenericTypeList[t].exponent); j++)
                    {
                        GameManager.instance.currentBuffNameList.Add(GameManager.instance.heroGenericTypeList[t].name);
                    }
                }
            }

            for (int t = 0; t < GameManager.instance.heroFightStyleList.Count; t++)
            {
                if ((GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[0] == t).Count() + GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[1] == t).Count()) / GameManager.instance.heroFightStyleList[t].exponent >= 1)
                {

                    for (int j = 0; j < (int)((GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[0] == t).Count() + GameManager.instance.myHeroList.Where(x => x.HeroFightStyleList[1] == t).Count()) / GameManager.instance.heroFightStyleList[t].exponent); j++)
                    {
                        GameManager.instance.currentBuffNameList.Add(GameManager.instance.heroFightStyleList[t].name);
                    }
                }
            }


            for (int t = 0; t < GameManager.instance.currentBuffNameList.Count; t++)
            {
                if (GameManager.instance.heroGenericTypeList.Where(x => x.name == GameManager.instance.currentBuffNameList[t]).Count() != 0)
                {
                    currentTeamBuffRect.transform.GetChild(t).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList.Where(x => x.name == GameManager.instance.currentBuffNameList[t]).FirstOrDefault().image;
                }
                else
                {
                    currentTeamBuffRect.transform.GetChild(t).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList.Where(x => x.name == GameManager.instance.currentBuffNameList[t]).FirstOrDefault().image; ;
                }

                currentTeamBuffRect.transform.GetChild(t).gameObject.SetActive(true);
            }



            GameManager.instance.SetHeroItems(i);
            myHeroMenuRect.transform.GetChild(i).gameObject.SetActive(true);
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
