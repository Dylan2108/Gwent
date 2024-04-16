using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : MonoBehaviour
{//Cartas Plata
    public string name;
    public int atk;
    public string atkType;
    public Hand hand;
    public void Start()
    {
        hand = transform.parent.GetComponent<Hand>();//Toma como referencia a la mano que sea su padre
    }
    public void OnMouseDown()
    {
      hand.SummonSilverCard(this);//Al hacer click se invoca la carta
    }
}