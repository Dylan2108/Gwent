using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
   public string Name;
   public Player player;
   public bool invoked;
   public string affectedZone;
   public void Start()
   {
      player = transform.parent.GetComponent<Player>();
   }
   public void OnMouseDown()
   {
     if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
         if(!invoked)
         {
           player.SummonWeatherCard(this);
           invoked = true;
           player.playedCards++;
            if(this.Name == "Kirin")
            {
               player.ShowMenuWeatherEffect(this,2);
            }
            else if(this.Name == "Jutsu Bola de Fuego")
            {
               player.ShowMenuWeatherEffect(this,1);
            }
         }
         else
         {
            Debug.Log("Ya el efecto de esta carta fue activado");
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