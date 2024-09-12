using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Scope : MonoBehaviour
{
   public Dictionary<string,Card> Cards = new Dictionary<string, Card>();
   public Dictionary<string,EffectExpression> Effects = new Dictionary<string, EffectExpression>();
   public Dictionary<string,object> Values = new Dictionary<string, object>();
   public GameManager Context;
   public TMP_InputField errorText;
   public void ShowError(string error)
   {
      errorText.text = error;
   }
   public void PushCard(string value,Card card)
   {
       Cards[value] = card;
   }
   public void PushEffect(string value,EffectExpression effect)
   {
      if(Effects.ContainsKey(value))
      {
         string error = $"Error Semantico. Ya el efecto {value} fue definido";
         ShowError(error);
         throw new Error("Ya el efecto {value} fue definido",ErrorType.SemanticError);
      } 
      else Effects[value] = effect;
   }
   public EffectExpression GetEffect(string value)
   {
      if(Effects.ContainsKey(value)) return Effects[value];
      else
      {
        string error = $"Error Semantico. El efecto {value} no ha sido definido";
        ShowError(error);
        throw new Error($"El efecto {value} no ha sido definido",ErrorType.SemanticError);
      }
   }
   public Card GetCard(string name)
   {
      if(Values.TryGetValue(name,out var cardObj) && cardObj is Card card) return card;
      Debug.LogWarning($"No se encontro la carta {name}");
      return null;
   }
}