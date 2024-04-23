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
      if(!EffectActivated)
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