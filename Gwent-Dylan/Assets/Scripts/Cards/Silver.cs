using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : MonoBehaviour
{//Cartas Plata
    public string name;//Nombre
    public int originalAtk;//Ataque
    public int atk;
    public string atkType;//Tipo de ataque 
    public Player player;//Jugador que posee la carta 
    public Lure LureCard;//La carta senuelo que puede elegir a esta carta como objetivo
    public bool invoked;//Para saber  si esta invocada
    public bool destroyed;//Para saber si ue destruida 
    public bool EffectActivated;//Para saber si el efecto fue activado
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia al jugador que sea su padre
        originalAtk = atk;
    }
    public void Update()
    {
        foreach(var card in player.cardsInHand)
        {
            if(card != null)
            {
                Lure LureComponent = card.GetComponent<Lure>();//Trata de encontrar a la carta senuelo
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
         {//Invoca la carta
           player.SummonSilverCard(this);
           invoked = true;
           player.playedCards++;
           player.ChangedCards = true;
         }
         else if(invoked && player.EffectLureIsActive)
         {//Activa el efecto de la carta senuelo
            player.EffectLure(this,LureCard);
            player.EffectLureIsActive = false;
            player.playedCards++;
         }
         else if(invoked && !EffectActivated)
         {//Activa el efecto de la carta plata
            if(this.name == "Gaara")
            {
                player.EliminateCardLessAtk();
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Hinata Hyuga")
            {
                player.InvokeWeatherCardEffect();
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Rock Lee")
            {
                player.InvokeBoostCardEffect("Melee");
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Sakura Haruno")
            {
                player.EffectDrawCard();
                EffectActivated = true;
                player.playedCards++;
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