using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public string Name;
    public Player player;
    public bool invoked;
    public bool destroyed;
    public void Start()
    {
      player = transform.parent.GetComponent<Player>();
    }
    public void OnMouseDown()
    {
        if(destroyed)Debug.Log("Ya esta carta fue destruida");
        else if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
          if(player.EffectLureIsActive) Debug.Log("Debe seleccionar una carta plata en el campo");
         else if(!invoked)
         {
          if(this.Name == "Pildoras Ninjas")
          {
            player.ShowMenuSummonBoost(this.gameObject,1);
            invoked = true;
            player.playedCards++;
            player.ChangedCards = true;
          }
          else if(this.Name == "Jutsu de la Alianza Shinobi")
          {
            player.ShowMenuSummonBoost(this.gameObject,2);
            invoked = true;
            player.playedCards++;
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