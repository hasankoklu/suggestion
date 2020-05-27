using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentGenericType;

    public List<Hero> heroList = new List<Hero>();
    public List<Item> itemList = new List<Item>();
    public List<ItemPiece> itemPieceList = new List<ItemPiece>();
    public List<HeroGenericType> heroGenericTypeList = new List<HeroGenericType>();
    public List<HeroFightStyle> heroFightStyleList = new List<HeroFightStyle>();

    public List<BestHeroTeam> bestHeroTeamList = new List<BestHeroTeam>();
    public List<Hero> myHeroList = new List<Hero>();
    public List<Item> myItemList = new List<Item>();
    public List<ItemPiece> myItemPieceList = new List<ItemPiece>();

    public int selectedHeroIndex;

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

        StartCoroutine(ListAllHeroTeam());
        //for (int i = 0; i < heroList.Count; i++)
        //{
        //    Debug.Log(i + " : " + heroList[i].name);
        //}
    }

    public void AddHeroToMyHeroList(Hero hero)
    {
        if (myHeroList.Count < 9)
        {
            Hero newhero = new Hero();
            newhero.name = hero.name;
            newhero.image = hero.image;
            newhero.currentItemList = hero.currentItemList;
            newhero.betterItemIdList = hero.betterItemIdList;
            newhero.removeIndex = myHeroList.Count;
            myHeroList.Add(newhero);

            CanvasManager.instance.SetMyHeroList();
            if (CanvasManager.instance.suggestionRect.activeSelf)
                MakeSuggestionforSelectedHero();
            else if (CanvasManager.instance.heroTeamSuggestionRect.activeSelf)
                SuggestHeroTeamButtonOnClick();
            else if (CanvasManager.instance.extraItemSuggestionRect.activeSelf)
                ItemSuggestButtonOnClick();
        }

        //if (!CanvasManager.instance.suggestionRect.activeSelf)
        //    CanvasManager.instance.suggestionRect.SetActive(true);

        //if (!CanvasManager.instance.suggestionRect.activeSelf)
        //    CanvasManager.instance.suggestionRect.SetActive(true);
    }

    public void MakeSuggestionforSelectedHero()
    {
        Hero myHero;

        #region ClearImages

        for (int k = 1; k < CanvasManager.instance.suggestionRect.transform.childCount; k++)
        {
            CanvasManager.instance.suggestionRect.transform.GetChild(k).gameObject.SetActive(false);
            for (int j = 0; j < CanvasManager.instance.suggestionRect.transform.GetChild(k).transform.childCount; j++)
            {
                CanvasManager.instance.suggestionRect.transform.GetChild(k).GetChild(j).GetComponent<Image>().sprite = null;
            }
        }

        #endregion

        if (myHeroList.Count != 0)
        {
            myHero = myHeroList[selectedHeroIndex];

            CanvasManager.instance.suggestionRect.transform.GetChild(0).GetComponent<Image>().sprite = myHero.image;
            //CanvasManager.instance.removeHeroButton.onClick.RemoveAllListeners();
            //CanvasManager.instance.removeHeroButton.onClick.AddListener(() => CanvasManager.instance.RemoveHeroButtonOnClick(selectedHeroIndex));

            if (myHero.betterItemIdList == null) // Look here
                return;


            //for (int q = 0; q < CanvasManager.instance.myHeroItemsRect.transform.childCount - 1; q++)
            //{
            //    CanvasManager.instance.myHeroItemsRect.transform.GetChild(q).GetComponent<Image>().sprite = null;
            //}

            for (int p = 0; p < myHero.currentItemList.Count; p++)
            {
                Debug.Log("curren item");
                //CanvasManager.instance.myHeroItemsRect.transform.GetChild(p).GetComponent<Image>().sprite = myHero.currentItemList[p].image;
            }


            //for (int t = 0; t < CanvasManager.instance.bestItemsRect.transform.childCount - 1; t++)
            //{
            //    CanvasManager.instance.bestItemsRect.transform.GetChild(t).GetComponent<Image>().sprite = itemList[myHero.betterItemIdList[t]].image;
            //    CanvasManager.instance.bestItemsRect.transform.GetChild(t).GetChild(0).GetComponent<Text>().text = itemList[myHero.betterItemIdList[t]].description;
            //}

            #region ItemSuggest


            int i = 1;
            foreach (int betterItemId in myHero.betterItemIdList)
            {
                if (i > 4)
                    return;

                if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 1)
                {
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = selectedHeroIndex;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().color =
                        new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.r,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.g,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.b, 1f);

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = Color.white;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;

                }
                else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name != itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 0 && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() > 0)
                {
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = selectedHeroIndex;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().color =
                        new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.r,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.g,
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.b, 1f);

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color = Color.white;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color = Color.white;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).gameObject.SetActive(true);


                    i++;
                }
                else
                {
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetComponent<Image>().color.b, 0.5f);


                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = itemList[betterItemId].description;

                    #region Color
                    if (myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() == 0)
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.b, 0.5f);
                    }
                    else
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().color.b, 1f);
                    }

                    if (myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() == 0)
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.b, 0.5f);
                    }
                    else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() == 1)
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.b, 0.5f);
                    }
                    else
                    {
                        CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color =
                         new Color(CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.r,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.g,
                         CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().color.b, 1f);
                    }

                    #endregion

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;

                    CanvasManager.instance.suggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;


                    CanvasManager.instance.suggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;
                }
            }

            #endregion

        }
        else
        {
            //CanvasManager.instance.selectedInfoRect.SetActive(false);
            CanvasManager.instance.suggestionRect.SetActive(false);
        }
    }

    List<Item> itemsuggestionList = new List<Item>();
    public void ItemSuggestButtonOnClick()
    {
        CanvasManager.instance.heroTeamSuggestionRect.SetActive(false);
        CanvasManager.instance.extraItemSuggestionRect.SetActive(true);
        CanvasManager.instance.suggestionRect.SetActive(false);


        for (int f = 0; f < CanvasManager.instance.extraItemSuggestionRect.transform.childCount; f++)
        {
            CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).GetComponent<Image>().sprite = null;
            CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).gameObject.SetActive(false);
            for (int s = 0; s < CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).childCount; s++)
            {
                CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(f).GetChild(s).GetComponent<Image>().sprite = null;
            }
        }

        #region ItemSuggest

        int i = 0;
        int heroIndex = 0;
        foreach (Hero myHero in myHeroList)
        {
            if (i > 8)
                return;

            if (myHero.currentItemList.Count > 2)
                return;

            foreach (int betterItemId in myHero.betterItemIdList)
            {


                if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 1)
                {
                    if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 4) //look here
                    {
                        return;
                    }

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();

                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = heroIndex;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = myHero.image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;

                }
                else if (itemPieceList[itemList[betterItemId].requiredPieceList[0]].name != itemPieceList[itemList[betterItemId].requiredPieceList[1]].name && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[0]].name).Count() > 0 && myItemPieceList.Where(x => x.name == itemPieceList[itemList[betterItemId].requiredPieceList[1]].name).Count() > 0)
                {
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
                    Item item = new Item();
                    item.name = itemList[betterItemId].name;
                    item.image = itemList[betterItemId].image;
                    item.description = itemList[betterItemId].description;
                    item.requiredPieceList = itemList[betterItemId].requiredPieceList;
                    item.Type = itemList[betterItemId].Type;
                    item.selectedHeroIndex = heroIndex;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Button>().onClick.AddListener(() => TakeItem(item));

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = myHero.image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = itemList[betterItemId].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[0]].image;
                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = itemPieceList[itemList[betterItemId].requiredPieceList[1]].image;

                    CanvasManager.instance.extraItemSuggestionRect.transform.GetChild(i).gameObject.SetActive(true);
                    i++;
                }
            }

            heroIndex++;
        }


        #endregion

    }

    public void SuggestHeroTeamButtonOnClick() // sal
    {
        CanvasManager.instance.heroTeamSuggestionRect.SetActive(true);
        CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(0).gameObject.SetActive(true);
        CanvasManager.instance.extraItemSuggestionRect.SetActive(false);
        CanvasManager.instance.suggestionRect.SetActive(false);

        //List<BestTeam> bestTeamList = new List<BestTeam>();

        //for (int i = 0; i < 16; i++)
        //{
        //    BestTeam bt = new BestTeam();
        //    bt.counter = 0;
        //    bt.bestTeamIndex = -1;
        //    bestTeamList.Add(bt);
        //}

        //if (myHeroList.Count == 0)
        //    return;

        //for (int i = 0; i < bestHeroTeamList.Count; i++) // increase here when add new team
        //{
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId00].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId01].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId02].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId03].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId04].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId05].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId06].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    if (myHeroList.Where(x => x.name == heroList[bestHeroTeamList[i].heroId07].name).Count() > 0)
        //    {
        //        bestTeamList[i].counter++;
        //    }
        //    bestTeamList[i].bestTeamIndex = i;
        //}
        //bestTeamList = bestTeamList.OrderByDescending(x => x.counter).ToList();

        //for (int i = 0; i < 3; i++) // suggegst count
        //{
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(0).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId00].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId00].name;

        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(1).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId01].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(1).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId01].name;

        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(2).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId02].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(2).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId02].name;

        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(3).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId03].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(3).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId03].name;

        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId04].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(4).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId04].name;

        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(5).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId05].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(5).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId05].name;

        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(6).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId06].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(6).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId06].name;

        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(7).GetComponent<Image>().sprite = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId07].image;
        //    CanvasManager.instance.heroTeamSuggestionRect.transform.GetChild(i).GetChild(7).GetChild(0).GetComponent<Text>().text = heroList[bestHeroTeamList[bestTeamList[i].bestTeamIndex].heroId07].name;

        //}
    }

    void TakeItem(Item item)
    {
        if (myHeroList[item.selectedHeroIndex].currentItemList.Count > 2)
            return;

        foreach (int itemId in item.requiredPieceList)
        {
            myItemPieceList.Remove(myItemPieceList.Where(x => x.name == itemPieceList[itemId].name).FirstOrDefault());
        }

        myHeroList[item.selectedHeroIndex].currentItemList.Add(item);

        myItemList.Add(item);

        MakeSuggestionforSelectedHero();
        CanvasManager.instance.RefreshListes();
    }


    List<Hero> tempHeroList = new List<Hero>();
    BestHeroTeam tempBestHeroTeam = new BestHeroTeam();
    public IEnumerator ListAllHeroTeam()
    {

        for (int i = 0; i < heroList.Count; i++)
        {
            yield return new WaitForFixedUpdate();
            for (int j = 1; j < heroList.Count; j++)
            {
                yield return new WaitForFixedUpdate();
                for (int k = 2; k < heroList.Count; k++)
                {
                    tempHeroList.Clear();
                    tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k] });
                    ArrangeBestTeamList(tempHeroList);

                    for (int l = 3; l < heroList.Count; l++)
                    {
                        tempHeroList.Clear();
                        tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k], heroList[l] });
                        ArrangeBestTeamList(tempHeroList);

                        for (int m = 4; m < heroList.Count; m++)
                        {
                            tempHeroList.Clear();
                            tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k], heroList[l], heroList[m] });
                            ArrangeBestTeamList(tempHeroList);

                            for (int n = 5; n < heroList.Count; n++)
                            {
                                tempHeroList.Clear();
                                tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k], heroList[l], heroList[m], heroList[n] });
                                ArrangeBestTeamList(tempHeroList);

                                for (int o = 6; o < heroList.Count; o++)
                                {
                                    tempHeroList.Clear();
                                    tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k], heroList[l], heroList[m], heroList[n], heroList[o] });
                                    ArrangeBestTeamList(tempHeroList);

                                    for (int p = 7; p < heroList.Count; p++)
                                    {
                                        tempHeroList.Clear();
                                        tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k], heroList[l], heroList[m], heroList[n], heroList[o], heroList[p] });
                                        ArrangeBestTeamList(tempHeroList);

                                        for (int r = 8; r < heroList.Count; r++)
                                        {
                                            tempHeroList.Clear();
                                            tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k], heroList[l], heroList[m], heroList[n], heroList[o], heroList[p], heroList[r] });
                                            ArrangeBestTeamList(tempHeroList);
                                            

                                            for (int s = 9; s < heroList.Count; s++)
                                            {
                                                tempHeroList.Clear();
                                                tempHeroList.AddRange(new List<Hero>() { heroList[i], heroList[j], heroList[k], heroList[l], heroList[m], heroList[n], heroList[o], heroList[p], heroList[r], heroList[s] });
                                                ArrangeBestTeamList(tempHeroList);


                                                yield return new WaitForFixedUpdate();
                                            }
                                            yield return new WaitForFixedUpdate();
                                        }
                                        yield return new WaitForFixedUpdate();
                                    }
                                    yield return new WaitForFixedUpdate();
                                }
                                yield return new WaitForFixedUpdate();
                            }
                            yield return new WaitForFixedUpdate();
                        }
                        yield return new WaitForFixedUpdate();
                    }
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForFixedUpdate();
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void ArrangeBestTeamList(List<Hero> heroes)
    {
        tempBestHeroTeam.heroList = new List<Hero>();

        string heronames = "";
        foreach (Hero hero in heroes)
        {
            heronames += " : \n" + hero.name;
            tempBestHeroTeam.heroList.Add(hero);
        }

        tempBestHeroTeam.buffCount = 0;

        for (int i = 0; i < heroGenericTypeList.Count; i++)
        {
            if (tempBestHeroTeam.heroList.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent >= 1)
            {
                tempBestHeroTeam.buffCount += (int)(tempBestHeroTeam.heroList.Where(x => x.HeroGenericType == i).Count() / heroGenericTypeList[i].exponent);
            }
        }

        for (int i = 0; i < heroFightStyleList.Count; i++)
        {

            if (tempBestHeroTeam.heroList.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent >= 1)
            {
                tempBestHeroTeam.buffCount += (int)(tempBestHeroTeam.heroList.Where(x => heroFightStyleList[x.HeroFightStyleList[0]].name == heroFightStyleList[i].name || heroFightStyleList[x.HeroFightStyleList[1]].name == heroFightStyleList[i].name).Count() / heroFightStyleList[i].exponent);
            }
        }

        if (tempBestHeroTeam.buffCount > 7)
        {
            Debug.Log("Buff Count : " + tempBestHeroTeam.buffCount + "\nHeroes " + heronames);
            tempHeroList.Clear();
            tempBestHeroTeam = new BestHeroTeam();
        }
    }

    #region Classes




    [Serializable]
    public class Hero
    {
        public string name;
        public Sprite image;
        public int level;
        public List<int> betterItemIdList;
        public int HeroGenericType;
        public List<int> HeroFightStyleList;
        public int removeIndex;
        public List<Item> currentItemList;
        public string description;
    }

    [Serializable]
    public class Item
    {
        public string name;
        public Sprite image;
        public int selectedHeroIndex;
        public int Type;
        public List<int> requiredPieceList;
        public string description;
    }

    [Serializable]
    public class ItemPiece
    {
        public string name;
        public Sprite image;
        public string description;
    }

    [Serializable]
    public class HeroGenericType
    {
        public string name;
        public int exponent;
        public string description;
    }

    [Serializable]
    public class HeroFightStyle
    {
        public string name;
        public int exponent;
        public string description;
    }

    [Serializable]
    public class BestHeroTeam
    {
        public List<Hero> heroList;
        public int winCount;
        public int loseCount;
        public int buffCount;
    }

    #endregion

}