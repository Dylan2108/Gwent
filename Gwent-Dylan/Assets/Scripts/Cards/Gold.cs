using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{//Carta de Oro
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
        hand.SummonGoldCard(this);//Invoca la carta al hacer click
    }
}