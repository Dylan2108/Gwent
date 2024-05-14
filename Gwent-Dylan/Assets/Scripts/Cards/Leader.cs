using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
   public string Name;
   public Deck deck;
   public bool EffectActivated;
   void Start()
   {
      deck = transform.parent.GetComponent<Deck>();
   }
   public void OnMouseDown()
   {
      bool EffectLureIsActive = deck.CheckEffectLure();
      if(EffectLureIsActive) Debug.Log("Debe seleccionar una carta plata en el campo");
      else if(!EffectActivated)
      {
            deck.DrawCard();
            EffectActivated = true;
      }
      else
      {
         Debug.Log("Ya el efecto de esta carta fue activado");
      }
   }
}