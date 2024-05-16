using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
   public string Name;//Nombre
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
      else
      {
         Debug.Log("Ya el efecto de esta carta fue activado");
      }
   }
}