using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : SpecialCard
{
    public void Start()
    {
      player = transform.parent.GetComponent<Player>();//Toma como refrencia al jugador que sea su padre
    }
    public void OnMouseDown()
    {
        if(destroyed)Debug.Log("Ya esta carta fue destruida");
        else if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
          if(player.EffectLureIsActive) Debug.Log("Debe seleccionar una carta plata en el campo");
         else if(!invoked)
         {//Invoca la carta y activa su efecto
          if(this.name == "Pildoras Ninjas")
          {
            player.ShowMenuSummonBoost(this,1);
            invoked = true;
            player.playedCards++;
            player.ChangedCards = true;
          }
          else if(this.name == "Jutsu de la Alianza Shinobi")
          {
            player.ShowMenuSummonBoost(this,2);
            invoked = true;
            player.playedCards++;
            player.ChangedCards = true;
          }
         }
         else
         {
            Debug.Log("Ya esta carta esta invocada");
         }
        }
        else if(!player.isMyTurn)Debug.Log("No es tu turno");
        else if(player.playedCards != 0 && !player.ICanStillSummoning)Debug.Log("Ya jugaste una carta en este turno");
    }
}