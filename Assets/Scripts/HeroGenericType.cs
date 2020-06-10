using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Generic Type", menuName = "Heroes/Hero Generic Type")]
public class HeroGenericType : ScriptableObject
{
    public Sprite image;
    public int exponent;
    public string description;
    public GameObject gameObject;
}