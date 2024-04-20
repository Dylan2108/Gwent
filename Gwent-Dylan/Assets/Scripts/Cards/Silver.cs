using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : MonoBehaviour
{//Cartas Plata
    public string name;
    public int atk;
    public string atkType;
    public Hand hand;
    public bool invoked;
    public void Start()
    {
        hand = transform.parent.GetComponent<Hand>();//Toma como referencia a la mano que sea su padre
    }
    public void OnMouseDown()
    {
      if(hand.isMyTurn && hand.playedCards == 0)
        {
         if(!invoked)
         {
           hand.SummonSilverCard(this);
           invoked = true;
           hand.playedCards++;
         }
         else
         {
            Debug.Log("Ya esta carta esta invocada");
         }
        }
        else if(!hand.isMyTurn)
        {
            Debug.Log("No es tu turno");
        }
        else if(hand.playedCards!=0)
        {
            Debug.Log("Ya jugaste una carta este turno");
        }
    }
}