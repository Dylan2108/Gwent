using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : SpecialCard
{
   public Scope scope;
   public Deck deck;//Deck al cual pertenece la carta
   public bool EffectActivated;//Para saber si el efecto de la carta fue activado
   void Start()
   {
      deck = transform.parent.GetComponent<Deck>();//Toma como referencia al deck padre  de la carta
   }
   public void OnMouseDown()
   {
      bool EffectLureIsActive = deck.CheckEffectLure();
      if(EffectLureIsActive) Debug.Log("Debe seleccionar una carta plata en el campo");
      else if(!EffectActivated)
      {//Se activa el efecto de la carta
            deck.DrawCard();
            EffectActivated = true;
            deck.IncrementPlayedCards();
      }
      else if(!EffectActivated && this.name != "Naruto Uzumaki")
      {
         GameObject Context = GameObject.Find("Context");
         Scope ScopeComponent = Context.GetComponent<Scope>();
         scope = ScopeComponent;
         Evaluator evaluator = new Evaluator(this.scope,this);
         evaluator.EvaluateEffect();
      }
      else
      {
         Debug.Log("Ya el efecto de esta carta fue activado");
      }
   }
}