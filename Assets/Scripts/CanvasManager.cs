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

    public GameObject ItemSuggestionItemPrefab;
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
        SetMyHeroList(GameManager.instance.myHeroList);

    }

    public void RemoveHeroButtonOnClick(Hero hero)
    {
        GameManager.instance.myHeroList.Remove(hero);
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

    }

    public void RemoveItemPieceToMyItemPieceList(int index)
    {
        GameManager.instance.myPieceItemList.Remove(
            GameManager.instance.myPieceItemList.Where(x => x.name == GameManager.instance.itemPieceList[index].name).FirstOrDefault()
            );
        //SetMyItemPiece();
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

            foreach (Hero hero in GameManager.instance.suggestionHeroList)
            {
                Hero myHero = new Hero();
                myHero = hero;
                myHero.gameObject = Instantiate(HeroButtonPrefab);
                myHero.gameObject.transform.SetParent(heroMenuRect.transform);
                myHero.gameObject.GetComponent<Image>().sprite = hero.image;
                myHero.gameObject.transform.GetChild(0).GetComponent<Text>().text = hero.name;
                myHero.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].image;
                myHero.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].image;
                myHero.gameObject.transform.GetChild(3).GetComponent<Text>().text = "%" + hero.winRate.ToString();

                myHero.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                myHero.gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.AddHeroToMyHeroList(myHero));
            }
        }
        else
        {
            foreach (Hero hero in GameManager.instance.heroList)
            {
                Hero myHero = new Hero();
                myHero = hero;
                myHero.gameObject = Instantiate(HeroButtonPrefab);
                myHero.gameObject.transform.SetParent(heroMenuRect.transform);
                myHero.gameObject.GetComponent<Image>().sprite = hero.image;
                myHero.gameObject.transform.GetChild(0).GetComponent<Text>().text = hero.name;
                myHero.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].image;
                myHero.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].image;
                myHero.gameObject.transform.GetChild(3).GetComponent<Text>().text = ""; //"%" + GameManager.instance.heroList[i].winRate.ToString();

                myHero.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                myHero.gameObject.GetComponent<Button>().onClick.AddListener(() => GameManager.instance.AddHeroToMyHeroList(myHero));
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

    public void SetMyHeroList(List<Hero> heroList)
    {
        for (int i = 0; i < myHeroMenuRect.transform.childCount; i++)
        {
            Destroy(myHeroMenuRect.transform.GetChild(i).gameObject);
        }


        foreach (Hero hero in GameManager.instance.myHeroList)
        {
            hero.gameObject = Instantiate(HeroCardButtonPrefab);
            hero.gameObject.transform.SetParent(myHeroMenuRect.transform);

            hero.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

            hero.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => RemoveHeroButtonOnClick(hero));

            hero.gameObject.transform.GetComponent<Image>().sprite = hero.cardImage;
            hero.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = hero.name;

            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(0).GetComponent<Image>().sprite = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].image;
            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroGenericTypeList[hero.HeroGenericType].name;

            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(1).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].image;
            hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[0]].name;

            if (hero.HeroFightStyleList[1] != 0)
            {
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<Image>().sprite = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[1]].image;
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>().text = GameManager.instance.heroFightStyleList[hero.HeroFightStyleList[1]].name;
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                hero.gameObject.transform.GetChild(2).GetChild(1).GetChild(2).gameObject.SetActive(false);
            }


            GameManager.instance.SetHeroItems(hero);
            #region Current Buff System

            GameManager.instance.currentBuffNameList.Clear();

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

            for (int i = 0; i < currentTeamBuffRect.transform.childCount; i++)
            {
                currentTeamBuffRect.transform.GetChild(i).gameObject.SetActive(false);
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

            #endregion

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

}
