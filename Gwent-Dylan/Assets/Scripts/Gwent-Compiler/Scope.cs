using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scope : MonoBehaviour
{
   public Dictionary<string,Card> Cards = new Dictionary<string, Card>();
   public Dictionary<string,EffectExpression> Effects = new Dictionary<string, EffectExpression>();
   public Dictionary<string,object> Values = new Dictionary<string, object>();
   public GameManager Context;
   public void PushCard(string value,Card card)
   {
       Cards[value] = card;
   }
   public void PushEffect(string value,EffectExpression effect)
   {
      if(Effects.ContainsKey(value)) Error.Report(ErrorType.SemanticError,$"Ya el efecto {value} fue definido");
      else Effects[value] = effect;
   }
   public EffectExpression GetEffect(string value)
   {
      if(Effects.ContainsKey(value)) return Effects[value];
      else
      {
        Error.Report(ErrorType.SemanticError,$"El efecto {value} no ha sido definido");
        return null;
      }
   }
   public Card GetCard(string name)
   {
      if(Values.TryGetValue(name,out var cardObj) && cardObj is Card card) return card;
      Debug.LogWarning($"No se encontro la carta {name}");
      return null;
   }
}