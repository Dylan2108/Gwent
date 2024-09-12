using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
     public string Type;
     public string name;
     public string faction;
     public int power;
     public string[] range = new string[3];
     public int owner;
     public Player player;
     public bool invoked;
     public bool destroyed;
     public OnActivationExpression onActivation;
}