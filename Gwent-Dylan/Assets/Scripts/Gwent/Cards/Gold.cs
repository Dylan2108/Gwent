using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Gold : UnitCard
{//Carta de Oro
     public Scope scope;
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia al jugador que sea su padre
    }
    public void OnMouseDown()
    {
        if(destroyed)Debug.Log("Ya esta carta fue destruida");
        else if((player.isMyTurn && player.playedCards == 0) || player.ICanStillSummoning)
        {
          if(player.EffectLureIsActive) Debug.Log("Debe seleccionar una carta plata en el campo");
          else if(!invoked)
         {//Invoca la carta
           player.SummonUnitCard(this);
           invoked = true;
           player.playedCards++;
           player.ChangedCards  = true;
         }
         else if(invoked && !EffectActivated)
         {//Activa el efecto de la carta
            if(this.name == "Itachi Uchiha")
            {
                player.InvokeBoostCardEffect("Melee");
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Jiraiya")
            {
                player.EliminateCardLessAtk();
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Might Guy")
            {
                player.InvokeWeatherCardEffect();
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Orochimaru")
            {
                player.InvokeBoostCardEffect("Siege");
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Madara Uchiha")
            {
                player.EffectProm();
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Kakashi Hatake" || this.name == "Tsunade")
            {
                player.EffectDrawCard();
                EffectActivated =true;
                player.playedCards++;
            }
            else if(this.name == "Minato Namikaze")
            {
                player.EliminateCardHigherAtk();
                EffectActivated = true;
                player.playedCards++;
            }
            else if(this.name == "Sasuke Uchiha")
            {
                player.EliminateRow();
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
            Debug.Log("Ya esta carta esta convocada");
         }
        }
        else if(!player.isMyTurn)Debug.Log("No es tu turno");
        else if(player.playedCards!=0 && !player.ICanStillSummoning)Debug.Log("Ya jugaste una carta este turno");
    }
}