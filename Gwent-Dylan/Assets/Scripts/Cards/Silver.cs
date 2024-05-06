using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : MonoBehaviour
{//Cartas Plata
    public string name;
    public int originalAtk;
    public int atk;
    public int modAtk; // Muestra las modificaciones en el atk realizadas por los climas
    public string atkType;
    public Player player;
    public bool invoked;
    public bool EffectActivated;
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia a la mano que sea su padre
        originalAtk = atk;
    }
    public void OnMouseDown()
    {
      if((player.isMyTurn && player.playedCards == 0) || player.ICanStillSummoning)
        {
         if(!invoked)
         {
           player.SummonSilverCard(this);
           invoked = true;
           player.playedCards++;
         }
         else if(invoked && !EffectActivated)
         {
            if(this.name == "Gaara")
            {
                player.EliminateCardLessAtk();
                EffectActivated = true;
            }
            else if(this.name == "Hinata Hyuga")
            {
                player.InvokeWeatherCardEffect();
                EffectActivated = true;
            }
            else if(this.name == "Rock Lee")
            {
                player.InvokeBoostCardEffect("Melee");
                EffectActivated = true;
            }
            else if(this.name == "Sakura Haruno")
            {
                player.EffectDrawCard();
                EffectActivated = true;
            }
         }
         else
         {
            Debug.Log("Ya esta carta esta invocada");
         }
        }
        else if(!player.isMyTurn)
        {
            Debug.Log("No es tu turno");
        }
        else if(player.playedCards!=0 && !player.ICanStillSummoning)
        {
            Debug.Log("Ya jugaste una carta este turno");
        }
    }
}