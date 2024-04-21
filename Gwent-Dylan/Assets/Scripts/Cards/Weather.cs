using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
   public string Name;
   public Player player;
   public bool invoked;
   public void Start()
   {
      player = transform.parent.GetComponent<Player>();
   }
   public void OnMouseDown()
   {
     if(player.isMyTurn && player.playedCards==0 || player.ICanStillSummoning)
        {
         if(!invoked)
         {
           player.SummonWeatherCard(this);
           invoked = true;
           player.playedCards++;
         }
         else
         {
            Debug.Log("Ya jugaste esta carta");
         }
        }
        else if(!player.isMyTurn)
        {
            Debug.Log("No es tu turno");
        }
        else if(player.playedCards!=0 && !player.ICanStillSummoning)
        {
            Debug.Log("Ya invocaste una carta este turno");
        }
   }
}