using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : MonoBehaviour
{//Cartas Plata
    public string name;
    public int originalAtk;
    public int atk;
    public string atkType;
    public Player player;
    public Lure LureCard;
    public bool invoked;
    public bool destroyed;
    public bool EffectActivated;
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia a la mano que sea su padre
        originalAtk = atk;
    }
    public void Update()
    {
        foreach(var card in player.cardsInHand)
        {
            if(card != null)
            {
                Lure LureComponent = card.GetComponent<Lure>();
                if(LureComponent != null) LureCard = LureComponent;
            }
        }
    }
    public void OnMouseDown()
    {
      if(destroyed)Debug.Log("Ya esta carta fue destruida");
      else if((player.isMyTurn && player.playedCards == 0) || player.ICanStillSummoning)
        {
          if(player.EffectLureIsActive && !invoked) Debug.Log("Debe seleccionar una carta plata en el campo");
         else if(!invoked)
         {
           player.SummonSilverCard(this);
           invoked = true;
           player.playedCards++;
           player.ChangedCards = true;
         }
         else if(invoked && player.EffectLureIsActive)
         {
            player.EffectLure(this,LureCard);
            player.EffectLureIsActive = false;
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
        else if(!player.isMyTurn)Debug.Log("No es tu turno");
        else if(player.playedCards!=0 && !player.ICanStillSummoning)Debug.Log("Ya jugaste una carta este turno");
    }
}