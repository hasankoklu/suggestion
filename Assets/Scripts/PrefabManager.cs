using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public GameObject Hero_Button;
    public GameObject CurrentTeamBuff_Image;
    public GameObject HeroCard_Button;
    public GameObject ItemPiece_Button;
    public GameObject ItemSuggestion_Button;

    public static PrefabManager instance;

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
}