using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hero Fight Style", menuName = "Heroes/Hero Fight Style")]
public class HeroFightStyle : ScriptableObject
{
    public Sprite image;
    public int exponent;
    public string description;
    public GameObject gameObject;
}
