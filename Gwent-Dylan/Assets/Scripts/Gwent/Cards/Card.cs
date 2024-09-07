using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
     public CardType Type;
     public string name;
     public string faction;
     public int power;
     public string[] range = new string[3];
     public int owner;
     public OnActivationExpression onActivation;
}
public enum CardType
{
     Gold,Silver,Weather,Boost,Leader,Clear,Lure
}