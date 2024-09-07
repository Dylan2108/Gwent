using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using System.Data;
public class Evaluator : MonoBehaviour
{
   private Card Cards;
   private Scope scope;
   public Evaluator(Scope scope,Card cards)
   {
     Cards = cards;
     this.scope = scope;
   }
   public void EvaluateEffect()
   {
      foreach(var onActivationElements in Cards.onActivation.OnActivation)
      {
        EvaluateOnActivationElement(onActivationElements);
      }
   }
   private void EvaluateOnActivationElement(OnActivationElementsExpression onActivationElements)
   {
      EvaluateEffectCall(onActivationElements.EffectCall,onActivationElements.Selector);
      if(onActivationElements.PostAction != null && onActivationElements.PostAction.Count>0)
      {
        foreach(var postAction in onActivationElements.PostAction)
        {
            EvaluatePostAction(postAction,onActivationElements.Selector.Source);
        }
      }
   }
   private void EvaluateEffectCall(EffectCallExpression effectCall,SelectorExpression selector)
   {
        EffectExpression effect = scope.Effects[effectCall.Name];
        foreach(var assign in effectCall.Params)
        {
          scope.Values[assign.Variable.Name] = assign.Value.Evaluate(scope);
        }
        if(selector !=null)
        {
          scope.Values["targets"] = EvaluateSelector(selector);
          EvaluateAction(effect.Action);
          scope.Values.Remove("targets");
        }
        else
        {
          EvaluateAction(effect.Action);
        }
   }
   private void EvaluatePostAction(PostActionExpression postAction,string selector)
   {
      EffectExpression effect = scope.Effects[(string)postAction.Type.Evaluate(scope)];
      foreach(var assign in postAction.Body)
      {
        scope.Values[assign.Variable.Name] = assign.Value.Evaluate(scope);
        if(postAction.Selector!=null)
        {
          scope.Values["targets"] = EvaluateSelector(postAction.Selector,selector);
          EvaluateAction(effect.Action);
          scope.Values.Remove("targets");
        }
        else
        {
          EvaluateAction(effect.Action);
        }
      }
   }
   private void EvaluateAction(ActionExpression action)
   {
      EvaluateStatementBlock(action.Body);
   }
   private List<Card> EvaluateSelector(SelectorExpression selector,string name = null)
   {
      List<Card> cards = new List<Card>();
      if(selector.Source == @"""parent""") cards = EvaluateSource(name);
      else cards = EvaluateSource(selector.Source);
      List<Card> cards_2 = new List<Card>();
      foreach(var card in cards)
      {
        scope.Values[selector.Predicate.Variable.Name] = card;
        if((bool) selector.Predicate.Condition.Evaluate(scope)) cards_2.Add(card);
        scope.Values.Remove(selector.Predicate.Variable.Name);
      }
      if(selector.Single.Value)
      {
        List<Card> cards_3 = new List<Card>{cards_2[0]};
        return cards_3;
      }
      else
      {
        return cards_2;
      }
   }
   public void EvaluateStatementBlock(StatementBlockExpression statementBlock)
   {
       foreach(var statement in statementBlock.expressions)
       {
          statement.Evaluater(scope);
       }
   }
   private List<Card> EvaluateSource(string name)
   {
       switch(name)
       {
          case @"""hand""" : return scope.Context.HandOfPlayer(scope.Context.TriggerPlayer());
          case @"""otherhand""":
          if(scope.Context.TriggerPlayer() == 0) return scope.Context.HandOfPlayer(0);
          else return scope.Context.HandOfPlayer(1);
          case @"""deck""": return scope.Context.DeckOfPlayer(scope.Context.TriggerPlayer());
          case @"""otherdeck""":
          if(scope.Context.TriggerPlayer() == 0) return scope.Context.DeckOfPlayer(0);
          else return scope.Context.DeckOfPlayer(1);
          case @"""field""": return scope.Context.FieldOfPlayer(scope.Context.TriggerPlayer());
          case @"""otherfield""":
          if(scope.Context.TriggerPlayer() == 0) return scope.Context.FieldOfPlayer(0);
          else return scope.Context.FieldOfPlayer(1);
          case @"""board""": return scope.Context.Board();
          default: return scope.Context.Board();
       }
   }
   private Func<Card,bool> EvaluatePreddicate(PredicateExpression predicate)
   {
      return Cards =>
      {
        scope.Values[predicate.Variable.Name] = Cards;
        return (bool)predicate.Condition.Evaluate(scope);
      };
   }
}