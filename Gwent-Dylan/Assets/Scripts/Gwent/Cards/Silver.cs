using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : UnitCard
{//Cartas Plata
     public Scope scope;
    public Lure LureCard;//La carta senuelo que puede elegir a esta carta como objetivo
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia al jugador que sea su padres
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
           player.SummonUnitCard(this);
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
            else
            {
                GameObject Context = GameObject.Find("Context");
                Scope ScopeComponent = Context.GetComponent<Scope>();
                scope = ScopeComponent;
                Evaluator evaluator = new Evaluator(this.scope,this);
                evaluator.EvaluateEffect();
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