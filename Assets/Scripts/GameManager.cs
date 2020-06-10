using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region Variablees

    public static GameManager instance;

    public int currentGenericType;

    public List<Hero> heroList = new List<Hero>();
    public List<Item> itemList = new List<Item>();
    public List<ItemPiece> itemPieceList = new List<ItemPiece>();
    public List<HeroGenericType> heroGenericTypeList = new List<HeroGenericType>();
    public List<HeroFightStyle> heroFightStyleList = new List<HeroFightStyle>();

    public List<BestHeroTeam> bestHeroTeamList = new List<BestHeroTeam>();
    public List<Hero> myHeroList = new List<Hero>();
    public List<Item> myComplateItemList = new List<Item>();
    public List<ItemPiece> myPieceItemList = new List<ItemPiece>();
    public List<Hero> suggestionHeroList = new List<Hero>();

    public List<BestHeroTeam> theBestTeamList;

    public int playerLevel = 1;

    public int selectedHeroIndex = -1;

    #endregion

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
        myPieceItemList.Add(itemPieceList[0]);
        myPieceItemList.Add(itemPieceList[0]);
        myPieceItemList.Add(itemPieceList[0]);
        myPieceItemList.Add(itemPieceList[0]);
    }

    public void AddHeroToMyHeroList(Hero hero)
    {
        if (myHeroList.Count < 9)
        {
            Hero newHero = new Hero();
            newHero.name = hero.name;
            newHero.image = hero.image;
            newHero.currentItemList = hero.currentItemList;
            newHero.currentItemPieceList = hero.currentItemPieceList;
            newHero.betterItemIdList = hero.betterItemIdList;
            newHero.removeIndex = myHeroList.Count;
            newHero.HeroFightStyleList = hero.HeroFightStyleList;
            newHero.HeroGenericType = hero.HeroGenericType;
            newHero.cardImage = hero.cardImage;
            newHero.level = hero.level;
            newHero.winRate = hero.winRate;
            myHeroList.Add(newHero);
            //myHeroList = myHeroList.OrderBy(x => x.HeroGenericType).ThenByDescending(x => x.level).ToList();

            BestHeroTeamSuggestion();

        }
    }

    public void SetHeroItems(Hero hero)
    {
        hero.betterItemList = new List<Item>();

        foreach (int betterItemId in hero.betterItemIdList)
        {
            hero.betterItemList.Add(itemList[betterItemId]);
        }

        #region ItemSuggest

        foreach (Item betterItem in hero.betterItemList)
        {
            betterItem.gameObject = Instantiate(CanvasManager.instance.ItemSuggestionItemPrefab);
            betterItem.gameObject.transform.SetParent(hero.gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).transform);

            if (myComplateItemList.Where(x => x.name == betterItem.name).Count() > 0)
            {
                Item item = new Item();
                item.name = betterItem.name;
                item.image = betterItem.image;
                item.description = betterItem.description;
                item.requiredPieceList = betterItem.requiredPieceList;
                item.Type = betterItem.Type;

                betterItem.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyItem(item));

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color =
                    new Color(betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.r,
                    betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.g,
                    betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.b, 1f);

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = betterItem.image;

                betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[0]].image;
                betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[1]].image;

                betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
                betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color = Color.white;

            }
            else if (itemPieceList[betterItem.requiredPieceList[0]].name == itemPieceList[betterItem.requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[betterItem.requiredPieceList[0]].name).Count() > 1)
            {
                Item item = new Item();
                item.gameObject = betterItem.gameObject;
                item.name = betterItem.name;
                item.image = betterItem.image;
                item.description = betterItem.description;
                item.requiredPieceList = betterItem.requiredPieceList;
                item.Type = betterItem.Type;

                betterItem.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyItem(item));

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color =
                    new Color(betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.r,
                    betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.g,
                    betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.b, 1f);

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = betterItem.image;

                betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[0]].image;
                betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[1]].image;

                betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
                betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color = Color.white;

                ItemPiece itemPiece = new ItemPiece();
                itemPiece.name = itemPieceList[betterItem.requiredPieceList[0]].name;
                itemPiece.description = itemPieceList[betterItem.requiredPieceList[0]].description;
                itemPiece.image = itemPieceList[betterItem.requiredPieceList[0]].image;


                betterItem.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                ItemPiece itemPiece2 = new ItemPiece();
                itemPiece2.name = itemPieceList[betterItem.requiredPieceList[1]].name;
                itemPiece2.description = itemPieceList[betterItem.requiredPieceList[1]].description;
                itemPiece2.image = itemPieceList[betterItem.requiredPieceList[1]].image;

                betterItem.gameObject.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece2));


            }
            else if (itemPieceList[betterItem.requiredPieceList[0]].name != itemPieceList[betterItem.requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[betterItem.requiredPieceList[0]].name).Count() > 0 && myPieceItemList.Where(x => x.name == itemPieceList[betterItem.requiredPieceList[1]].name).Count() > 0)
            {

                Item item = new Item();
                item.gameObject = betterItem.gameObject;
                item.name = betterItem.name;
                item.image = betterItem.image;
                item.description = betterItem.description;
                item.requiredPieceList = betterItem.requiredPieceList;
                item.Type = betterItem.Type;
                betterItem.gameObject.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => BuyItem(item));

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color =
                    new Color(betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.r,
                    betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.g,
                    betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.b, 1f);

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = betterItem.image;

                betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[0]].image;
                betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[1]].image;

                betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.white;
                betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color = Color.white;

                ItemPiece itemPiece = new ItemPiece();
                itemPiece.name = itemPieceList[betterItem.requiredPieceList[0]].name;
                itemPiece.description = itemPieceList[betterItem.requiredPieceList[0]].description;
                itemPiece.image = itemPieceList[betterItem.requiredPieceList[0]].image;

                betterItem.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                ItemPiece itemPiece2 = new ItemPiece();
                itemPiece2.name = itemPieceList[betterItem.requiredPieceList[1]].name;
                itemPiece2.description = itemPieceList[betterItem.requiredPieceList[1]].description;
                itemPiece2.image = itemPieceList[betterItem.requiredPieceList[1]].image;

                betterItem.gameObject.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece2));
            }
            else
            {

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color =
                     new Color(betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.r,
                    betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.g,
                     betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().color.b, 0.65f);

                betterItem.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = betterItem.image;

                #region Color
                if (myPieceItemList.Where(x => x.name == itemPieceList[betterItem.requiredPieceList[0]].name).Count() == 0)
                {
                    betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color =
                     new Color(
                     betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color.r,
                     betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color.g,
                     betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color.b, 0.65f);

                    betterItem.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
                }
                else
                {
                    betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color =
                     new Color(
                     betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color.r,
                     betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color.g,
                     betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().color.b, 1f);

                    ItemPiece itemPiece = new ItemPiece();
                    itemPiece.name = itemPieceList[betterItem.requiredPieceList[0]].name;
                    itemPiece.description = itemPieceList[betterItem.requiredPieceList[0]].description;
                    itemPiece.image = itemPieceList[betterItem.requiredPieceList[0]].image;

                    betterItem.gameObject.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                }

                if (myPieceItemList.Where(x => x.name == itemPieceList[betterItem.requiredPieceList[1]].name).Count() == 0)
                {
                    betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color =
                     new Color(
                     betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color.r,
                     betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color.g,
                     betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color.b, 0.65f);

                }
                else if (itemPieceList[betterItem.requiredPieceList[0]].name == itemPieceList[betterItem.requiredPieceList[1]].name && myPieceItemList.Where(x => x.name == itemPieceList[betterItem.requiredPieceList[1]].name).Count() == 1)
                {
                    betterItem.gameObject.transform.GetComponent<Image>().color =
                     new Color(
                     betterItem.gameObject.transform.GetComponent<Image>().color.r,
                     betterItem.gameObject.transform.GetComponent<Image>().color.g,
                     betterItem.gameObject.transform.GetComponent<Image>().color.b, 0.65f);
                }
                else
                {
                    betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color =
                     new Color(
                     betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color.r,
                     betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color.g,
                     betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().color.b, 0.1f);

                    ItemPiece itemPiece = new ItemPiece();
                    itemPiece.name = itemPieceList[betterItem.requiredPieceList[1]].name;
                    itemPiece.description = itemPieceList[betterItem.requiredPieceList[1]].description;
                    itemPiece.image = itemPieceList[betterItem.requiredPieceList[1]].image;

                    betterItem.gameObject.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => BuyPieceItem(itemPiece));

                }

                #endregion

                betterItem.gameObject.transform.GetChild(1).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[0]].image;

                betterItem.gameObject.transform.GetChild(2).GetComponent<Image>().sprite = itemPieceList[betterItem.requiredPieceList[1]].image;

            }
        }
        #endregion
    }

    public void SuggestHero()
    {
        CanvasManager.instance.heroTeamSuggestionRect.SetActive(true);
        CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(0).gameObject.SetActive(true);
        CanvasManager.instance.extraItemSuggestionRect.SetActive(false);
        CanvasManager.instance.suggestionRect.SetActive(false);


        for (int i = 0; i < CanvasManager.instance.heroTeamSuggestionRect.transform.childCount; i++)
        {
            for (int j = 0; j < CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).childCount; j++)
            {
                CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(j).GetComponent<Image>().gameObject.SetActive(false);
            }
        }

        CanvasManager.instance.teamBuffText.text = theBestTeamList[0].buffName;

        for (int i = 0; i < theBestTeamList.Count; i++)
        {
            for (int j = 0; j < theBestTeamList[i].heroList.Count; j++)
            {
                if (i < CanvasManager.instance.heroTeamSuggestionRect.transform.childCount)
                {
                    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(j).GetComponent<Image>().sprite = theBestTeamList[i].heroList[j].image;
                    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(j).gameObject.SetActive(true);
                }

            }
        }
    }

    public void TakeItem(Item item)
    {

        if (item.selectedHero == null)
        {
            myComplateItemList.Add(item);
        }
        else if (item.selectedHero.currentItemList.Count + item.selectedHero.currentItemPieceList.Count < 3)
        {
            item.selectedHero.currentItemList.Add(item);
        }

        CanvasManager.instance.addItemRect.SetActive(false);
        CanvasManager.instance.RefreshListes();
    }

    public void TakePieceItem(ItemPiece itemPiece)
    {
        itemPiece.selectedHeroIndex = selectedHeroIndex;

        if (itemPiece.selectedHeroIndex == -1)
        {
            myPieceItemList.Add(itemPiece);
        }
        else if (myHeroList[itemPiece.selectedHeroIndex].currentItemList.Count != 3)
        {
            myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Add(itemPiece);

            if (myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Count > 1)
            {
                foreach (Item item in itemList)
                {

                    if (myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Count > 0)
                        if (itemPieceList[item.requiredPieceList[0]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[0].name && itemPieceList[item.requiredPieceList[1]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[1].name)
                        {
                            myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Clear();
                            Item tempItem = new Item();
                            tempItem.name = item.name;
                            tempItem.image = item.image;
                            tempItem.description = item.description;
                            tempItem.requiredPieceList = item.requiredPieceList;
                            //tempItem.selectedHeroIndex = itemPiece.selectedHeroIndex;

                            myHeroList[itemPiece.selectedHeroIndex].currentItemList.Add(tempItem);

                        }
                        else if (itemPieceList[item.requiredPieceList[0]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[1].name && itemPieceList[item.requiredPieceList[1]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[0].name)

                        {
                            myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Clear();
                            Item tempItem = new Item();
                            tempItem.name = item.name;
                            tempItem.image = item.image;
                            tempItem.description = item.description;
                            tempItem.requiredPieceList = item.requiredPieceList;
                            //tempItem.selectedHeroIndex = itemPiece.selectedHeroIndex;

                            myHeroList[itemPiece.selectedHeroIndex].currentItemList.Add(tempItem);
                        }
                }
            }
        }


        CanvasManager.instance.addItemRect.SetActive(false);
        CanvasManager.instance.RefreshListes();
    }

    public void BuyItem(Item item)
    {
        if (item.selectedHero.currentItemList.Count + item.selectedHero.currentItemPieceList.Count > 2)
            return;

        if (myComplateItemList.Where(x => x.name == item.name).Count() > 0)
        {
            myComplateItemList.Remove(item);
        }
        else
        {

            foreach (int itemId in item.requiredPieceList)
            {
                myPieceItemList.Remove(myPieceItemList.Where(x => x.name == itemPieceList[itemId].name).FirstOrDefault()); // look here
            }
        }
        item.selectedHero.currentItemList.Add(item);

        CanvasManager.instance.RefreshListes();
    }

    public void BuyPieceItem(ItemPiece itemPiece)
    {

        if (myHeroList[itemPiece.selectedHeroIndex].currentItemList.Count == 3)
            return;

        myPieceItemList.Remove(myPieceItemList.Where(x => x.name == itemPiece.name).FirstOrDefault());

        myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Add(itemPiece);

        if (myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Count > 1)
        {
            foreach (Item item in itemList)
            {
                if (myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Count > 0)
                    if (itemPieceList[item.requiredPieceList[0]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[0].name && itemPieceList[item.requiredPieceList[1]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[1].name)
                    {
                        myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Clear();
                        Item tempItem = new Item();
                        tempItem.name = item.name;
                        tempItem.image = item.image;
                        tempItem.description = item.description;
                        tempItem.requiredPieceList = item.requiredPieceList;
                        //tempItem.selectedHero = itemPiece.selectedHeroIndex;

                        myHeroList[itemPiece.selectedHeroIndex].currentItemList.Add(tempItem);

                    }
                    else if (itemPieceList[item.requiredPieceList[0]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[1].name && itemPieceList[item.requiredPieceList[1]].name == myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList[0].name)
                    {
                        myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Clear();
                        Item tempItem = new Item();
                        tempItem.name = item.name;
                        tempItem.image = item.image;
                        tempItem.description = item.description;
                        tempItem.requiredPieceList = item.requiredPieceList;
                        //tempItem.selectedHeroIndex = itemPiece.selectedHeroIndex;

                        myHeroList[itemPiece.selectedHeroIndex].currentItemList.Add(tempItem);
                    }
            }
        }

        CanvasManager.instance.RefreshListes();
    }

    void RemoveItemPiece(ItemPiece itemPiece)
    {
        myHeroList[itemPiece.selectedHeroIndex].currentItemPieceList.Remove(itemPiece);
        CanvasManager.instance.RefreshListes();
    }

    void RemoveComplateItem(Item item)
    {
        item.selectedHero.currentItemList.Remove(item);
        CanvasManager.instance.RefreshListes();
    }

    List<Hero> tempHeroList = new List<Hero>();

    public List<string> currentBuffNameList;
    public void ArrangeSuggestionHeroList(List<Hero> heroList)
    {
        heroList.LastOrDefault().currentBuffCount = 0;
        heroList.LastOrDefault().winRate = 0;
        currentBuffNameList.Clear();

        List<BestHeroTeam> tempBestHeroTeamList = new List<BestHeroTeam>();
        foreach (Hero hero in heroList) // remember.
        {
            tempBestHeroTeamList = bestHeroTeamList.Where(x => x.heroList.Contains(hero)).ToList();
        }

        if (tempBestHeroTeamList.Count > 0 && (tempBestHeroTeamList.FirstOrDefault().winCount + tempBestHeroTeamList.FirstOrDefault().loseCount) > 5)
        {
            heroList.LastOrDefault().winRate = (int)(tempBestHeroTeamList.FirstOrDefault().winCount / (tempBestHeroTeamList.FirstOrDefault().winCount + tempBestHeroTeamList.FirstOrDefault().loseCount) * 100);
            Debug.Log("Win Rate : " + heroList.LastOrDefault().winRate);
        }
        else
        {
            for (int i = 0; i < heroGenericTypeList.Count; i++)
            {
                if (heroList.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent >= 0)
                {
                    heroList.LastOrDefault().currentBuffCount += (int)(heroList.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent);
                }
            }

            for (int i = 0; i < heroFightStyleList.Count; i++)
            {

                if (heroList.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent >= 0)
                {
                    heroList.LastOrDefault().currentBuffCount += (int)(heroList.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent);
                }
            }

            heroList.LastOrDefault().winRate = heroList.LastOrDefault().currentBuffCount * 10;
            //Debug.Log("Win Rate : " + heroList.LastOrDefault().winRate);
        }

        suggestionHeroList.Add(heroList.LastOrDefault());
        suggestionHeroList = suggestionHeroList.OrderByDescending(x => x.winRate).ToList();
    }

    public int currentBuffCount;
    public List<BestHeroTeam> TheBestHeroes;
    public void BestHeroTeamSuggestion()
    {
        suggestionHeroList.Clear();

        foreach (Hero hero in heroList)
        {
            tempHeroList = new List<Hero>();

            foreach (Hero myHero in myHeroList)
            {
                tempHeroList.Add(myHero);
            }


            if (tempHeroList.Where(x => x.name == hero.name).Count() == 0)
            {
                tempHeroList.Add(hero);
                ArrangeSuggestionHeroList(tempHeroList);
            }
        }
        CanvasManager.instance.RefreshListes();

    }

    public void Save_TheBestHeroes(List<Hero> TheBestHeroes)
    {
        string path = Application.persistentDataPath + "/TheBestHeroes.txt";
        if (!string.IsNullOrEmpty(path))
        {
            var jsonString = JsonConvert.SerializeObject(TheBestHeroes, Formatting.Indented, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore });
            System.IO.File.WriteAllText(path, jsonString);
            Debug.Log("TheBestHeroes Successfully Saved.");
        }
    }

    public IEnumerator Read_TheBestHeroes()
    {
        string path = Application.persistentDataPath + "/TheBestHeroes.txt";
        if (!string.IsNullOrEmpty(path))
        {
            UnityWebRequest uwr = UnityWebRequest.Get(path);
            yield return uwr.SendWebRequest();
            if (uwr.isNetworkError)
            {
                Debug.Log("TheBestHeroes Load Error");
            }
            else
            {
                var Result = JsonConvert.DeserializeObject<List<BestHeroTeam>>(uwr.downloadHandler.text, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });
                TheBestHeroes = new List<BestHeroTeam>();
                TheBestHeroes = Result;
                Debug.Log("TheBestHeroes Successfully Loaded.");
            }
        }
        //}
    }

    #region Classes

    [Serializable]
    public class Hero
    {
        public string name;
        public Sprite image;
        public Sprite cardImage;
        public int level;
        public int gameLevel;
        public List<int> betterItemIdList;

        public List<Item> betterItemList;

        public int HeroGenericType;
        public List<int> HeroFightStyleList;
        public int removeIndex;
        public List<Item> currentItemList;
        public List<ItemPiece> currentItemPieceList;
        public int currentBuffCount;
        public float winRate;
        public string description;
        public GameObject gameObject;
    }

    [Serializable]
    public class Item
    {
        public string name;
        public Sprite image;
        public Hero selectedHero;
        public int Type;
        public List<int> requiredPieceList;
        public string description;
        public GameObject gameObject;
    }

    [Serializable]
    public class ItemPiece
    {
        public string name;
        public Sprite image;
        public int selectedHeroIndex;
        public string description;
        public GameObject gameObject;
    }

    [Serializable]
    public class HeroGenericType
    {
        public string name;
        public Sprite image;
        public int exponent;
        public string description;
        public GameObject gameObject;
    }

    [Serializable]
    public class HeroFightStyle
    {
        public string name;
        public Sprite image;
        public int exponent;
        public string description;
        public GameObject gameObject;
    }

    [Serializable]
    public class BestHeroTeam
    {
        public List<Hero> heroList;
        public int winCount;
        public int loseCount;
        public string buffName;
        public int buffCount;
        public GameObject gameObject;
    }

    #endregion

}
