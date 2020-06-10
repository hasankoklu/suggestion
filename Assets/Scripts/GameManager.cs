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
    #region Variables

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


        #region ItemSuggest

        foreach (Item betterItem in hero.betterItemList)
        {
            betterItem.gameObject = Instantiate(PrefabManager.instance.ItemSuggestion_Button);
            betterItem.gameObject.transform.SetParent(hero.gameObject.transform.GetChild(2).GetChild(0).GetChild(0).GetChild(0).transform);

            if (myComplateItemList.Where(x => x.name == betterItem.name).Count() > 0)
            {
                Item item = new Item();
                item.name = betterItem.name;
                item.image = betterItem.image;
                item.description = betterItem.description;
                item.requiredPieceList = betterItem.requiredPieceList;

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
        CanvasManager.instance.RefreshListes();
    }

    public void TakePieceItem(ItemPiece itemPiece)
    {
        if (itemPiece.selectedHero == null)
        {
            myPieceItemList.Add(itemPiece);
        }
        else if (itemPiece.selectedHero.currentItemList.Count != 3)
        {
            itemPiece.selectedHero.currentItemPieceList.Add(itemPiece);

            if (itemPiece.selectedHero.currentItemPieceList.Count > 1)
            {
                foreach (Item item in itemList)
                {

                    if (itemPiece.selectedHero.currentItemPieceList.Count > 0)
                        if (itemPieceList[item.requiredPieceList[0]].name == itemPiece.selectedHero.currentItemPieceList[0].name && itemPieceList[item.requiredPieceList[1]].name == itemPiece.selectedHero.currentItemPieceList[1].name)
                        {
                            itemPiece.selectedHero.currentItemPieceList.Clear();
                            Item tempItem = new Item();
                            tempItem.name = item.name;
                            tempItem.image = item.image;
                            tempItem.description = item.description;
                            tempItem.requiredPieceList = item.requiredPieceList;

                            itemPiece.selectedHero.currentItemList.Add(tempItem);

                        }
                        else if (itemPieceList[item.requiredPieceList[0]].name == itemPiece.selectedHero.currentItemPieceList[1].name && itemPieceList[item.requiredPieceList[1]].name == itemPiece.selectedHero.currentItemPieceList[0].name)

                        {
                            itemPiece.selectedHero.currentItemPieceList.Clear();
                            Item tempItem = new Item();
                            tempItem.name = item.name;
                            tempItem.image = item.image;
                            tempItem.description = item.description;
                            tempItem.requiredPieceList = item.requiredPieceList;

                            itemPiece.selectedHero.currentItemList.Add(tempItem);
                        }
                }
            }
        }
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
                myPieceItemList.Remove(myPieceItemList.Where(x => x.name == itemPieceList[itemId].name).FirstOrDefault());
            }
        }
        item.selectedHero.currentItemList.Add(item);

        CanvasManager.instance.RefreshListes();
    }

    public void BuyPieceItem(ItemPiece itemPiece)
    {

        if (itemPiece.selectedHero.currentItemList.Count == 3)
            return;

        myPieceItemList.Remove(myPieceItemList.Where(x => x.name == itemPiece.name).FirstOrDefault());

        itemPiece.selectedHero.currentItemPieceList.Add(itemPiece);

        if (itemPiece.selectedHero.currentItemPieceList.Count > 1)
        {
            foreach (Item item in itemList)
            {
                if (itemPiece.selectedHero.currentItemPieceList.Count > 0)
                    if (itemPieceList[item.requiredPieceList[0]].name == itemPiece.selectedHero.currentItemPieceList[0].name && itemPieceList[item.requiredPieceList[1]].name == itemPiece.selectedHero.currentItemPieceList[1].name)
                    {
                        itemPiece.selectedHero.currentItemPieceList.Clear();
                        Item tempItem = new Item();
                        tempItem.name = item.name;
                        tempItem.image = item.image;
                        tempItem.description = item.description;
                        tempItem.requiredPieceList = item.requiredPieceList;

                        itemPiece.selectedHero.currentItemList.Add(tempItem);

                    }
                    else if (itemPieceList[item.requiredPieceList[0]].name == itemPiece.selectedHero.currentItemPieceList[1].name && itemPieceList[item.requiredPieceList[1]].name == itemPiece.selectedHero.currentItemPieceList[0].name)
                    {
                        itemPiece.selectedHero.currentItemPieceList.Clear();
                        Item tempItem = new Item();
                        tempItem.name = item.name;
                        tempItem.image = item.image;
                        tempItem.description = item.description;
                        tempItem.requiredPieceList = item.requiredPieceList;

                        itemPiece.selectedHero.currentItemList.Add(tempItem);
                    }
            }
        }

        CanvasManager.instance.RefreshListes();
    }

    void RemoveItemPiece(ItemPiece itemPiece)
    {
        itemPiece.selectedHero.currentItemPieceList.Remove(itemPiece);
        Destroy(itemPiece.gameObject);
        CanvasManager.instance.RefreshListes();
    }

    void RemoveComplateItem(Item item)
    {
        item.selectedHero.currentItemList.Remove(item);
        Destroy(item.gameObject);
        CanvasManager.instance.RefreshListes();
    }

    List<Hero> tempHeroList = new List<Hero>();

    public List<string> currentBuffNameList;
    public void ArrangeSuggestionHeroList(List<Hero> suggestionHeroList)
    {
        //bestherolist de current buff count olmalı
        heroList.LastOrDefault().winRate = 0;
        currentBuffNameList.Clear();

        BestHeroTeam tempBestHeroTeam = new BestHeroTeam();
        foreach (Hero hero in suggestionHeroList)
        {
            tempBestHeroTeam = bestHeroTeamList.Where(x => x.heroList.Contains(hero) && x.heroList.Count == suggestionHeroList.Count).FirstOrDefault();
        }

        if (tempBestHeroTeam != null && (tempBestHeroTeam.winCount + tempBestHeroTeam.loseCount) > 5)
        {
            tempBestHeroTeam.winRate = (int)(tempBestHeroTeam.winCount / (tempBestHeroTeam.winCount + tempBestHeroTeam.loseCount) * 100);
            Debug.Log("Win Rate : " + heroList.LastOrDefault().winRate);
        }
        else
        {
            for (int i = 0; i < heroGenericTypeList.Count; i++)
            {
                if (heroList.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent >= 0)
                {
                    tempBestHeroTeam.winRate += (int)(heroList.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent);
                }
            }

            for (int i = 0; i < heroFightStyleList.Count; i++)
            {

                if (heroList.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent >= 0)
                {
                    tempBestHeroTeam.winRate += (int)(heroList.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent);
                }
            }

            tempBestHeroTeam.winRate = tempBestHeroTeam.winRate * 10;
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
    }

    #region Classes

    [Serializable]
    public class BestHeroTeam
    {
        public List<Hero> heroList;
        public int winCount;
        public int loseCount;
        public string buffName;
        public int buffCount;
        public float winRate;
        public GameObject gameObject;
    }

    #endregion

}
