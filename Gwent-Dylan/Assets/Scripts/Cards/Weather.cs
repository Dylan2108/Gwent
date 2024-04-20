using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
   public string Name;
   public Hand hand;
   public bool invoked;
   public void Start()
   {
      hand = transform.parent.GetComponent<Hand>();
   }
   public void OnMouseDown()
   {
     if(hand.isMyTurn && hand.playedCards==0)
        {
         if(!invoked)
         {
           hand.SummonWeatherCard(this);
           invoked = true;
           hand.playedCards++;
         }
         else
         {
            Debug.Log("Ya jugaste esta carta");
         }
        }
        else if(!hand.isMyTurn)
        {
            Debug.Log("No es tu turno");
        }
        else if(hand.playedCards!=0)
        {
            Debug.Log("Ya invocaste una carta este turno");
        }
   }
}