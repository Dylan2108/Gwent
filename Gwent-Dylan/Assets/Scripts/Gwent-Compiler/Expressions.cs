using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public interface Node
{
    
}
public abstract class Expression : Node
{
  public abstract object Evaluate(Scope scope);
}
public class NumberExpression : Expression
{
    public int Value{get;}
    public NumberExpression(int value)
    {
        Value = value;
    }
    public override object Evaluate(Scope scope)
    {
        return Value;
    }
}
public class StringExpression : Expression
{
    public string Value{get;}
    public StringExpression(string value)
    {
        Value = value;
    }
    public override object Evaluate(Scope scope)
    {
       return Value;
    }
}
public class BoolExpression : Expression
{
    public bool Value{get;}
    public BoolExpression(bool value)
    {
       Value = value;
    }
    public override object Evaluate(Scope scope)
    {
        return Value;
    }
}
public class BinaryExpression : Expression
{//Expresiones Binarias
    public Expression Left{get;}
    public Token Symbol{get;}
    public Expression Right{get;}
    public BinaryExpression(Expression left,Token symbol,Expression right)
    {
        Left = left;
        Symbol = symbol;
        Right = right;
    }
    public override object Evaluate(Scope scope)
    {
        object left = Left.Evaluate(scope);
        object right = Right.Evaluate(scope);
        if(left is double && right is double)
        {
            switch(Symbol.Type)
            {
                case TokenType.Plus: return Convert.ToDouble(left) + Convert.ToDouble(right);
                case TokenType.Less: return Convert.ToDouble(left) - Convert.ToDouble(right);
                case TokenType.Multiply: return Convert.ToDouble(left) * Convert.ToDouble(right);
                case TokenType.Divide: return Convert.ToDouble(left) / Convert.ToDouble(right);
                case TokenType.Modulus: return Convert.ToDouble(left) % Convert.ToDouble(right);
                case TokenType.Pow: return Math.Pow(Convert.ToDouble(left),Convert.ToDouble(right));
                case TokenType.GreaterThan: return Convert.ToDouble(left) > Convert.ToDouble(right);
                case TokenType.GreatEqualThan: return Convert.ToDouble(left) >= Convert.ToDouble(right);
                case TokenType.LessThan: return Convert.ToDouble(left) < Convert.ToDouble(right);
                case TokenType.LessEqualThan : return Convert.ToDouble(left) <= Convert.ToDouble(right);
                case TokenType.NotEqual : return !left.Equals(right);
                case TokenType.EqualEqual : return left.Equals(right);
                default:
                Error.Report(ErrorType.SemanticError,"Operacion invalida");
                return null;
            }
        }
        else if(left is string && right is string)
        {
            switch(Symbol.Type)
            {
                case TokenType.Concatenation: return left.ToString() + right.ToString();
                case TokenType.SpaceConcatenation : return left.ToString() + " " + right.ToString();
                case TokenType.EqualEqual: return left.Equals(right);
                default:
                Error.Report(ErrorType.SemanticError,"Operacion invalida");
                return null;
            }
        }
        else if(left is Card && right is string)
        {
            switch(Symbol.Type)
            {
                case TokenType.EqualEqual: return left.Equals(right);
                case TokenType.LessThan: return Convert.ToDouble(left) < Convert.ToDouble(right);
                case TokenType.LessEqualThan : return Convert.ToDouble(left) <= Convert.ToDouble(right);
                case TokenType.GreaterThan: return Convert.ToDouble(left) > Convert.ToDouble(right);
                case TokenType.GreatEqualThan: return Convert.ToDouble(left) >= Convert.ToDouble(right);
                case TokenType.NotEqual: return !left.Equals(right);
                default:
                Error.Report(ErrorType.SemanticError,"Operacion invalida");
                return null;
            }
        }
        else if(left is Card && right is int)
        {
            switch(Symbol.Type)
            {
                case TokenType.EqualEqual: return left.Equals(right);
                case TokenType.LessThan: return Convert.ToDouble(left) < Convert.ToDouble(right);
                case TokenType.LessEqualThan: return Convert.ToDouble(left) <= Convert.ToDouble(right);
                case TokenType.GreaterThan: return Convert.ToDouble(left) > Convert.ToDouble(right);
                case TokenType.GreatEqualThan: return Convert.ToDouble(left) >= Convert.ToDouble(right);
                case TokenType.NotEqual: return !left.Equals(right);
                default:
                Error.Report(ErrorType.SemanticError,"Operacion invalida");
                return null;
            }
        }
        else if(left is int && right is int)
        {
            switch(Symbol.Type)
            {
                case TokenType.EqualEqual: return left.Equals(right);
                case TokenType.LessThan: return Convert.ToDouble(left) < Convert.ToDouble(right);
                case TokenType.LessEqualThan: return Convert.ToDouble(left) <= Convert.ToDouble(right);
                case TokenType.GreaterThan: return Convert.ToDouble(left) > Convert.ToDouble(right);
                case TokenType.GreatEqualThan: return Convert.ToDouble(left) >= Convert.ToDouble(right);
                case TokenType.NotEqual: return !left.Equals(right);
                case TokenType.Plus: return Convert.ToDouble(left) + Convert.ToDouble(right);
                case TokenType.Less: return Convert.ToDouble(left) - Convert.ToDouble(right);
                case TokenType.Multiply: return Convert.ToDouble(left) * Convert.ToDouble(right);
                case TokenType.Divide: return Convert.ToDouble(left) / Convert.ToDouble(right);
                case TokenType.Modulus: return Convert.ToDouble(left) % Convert.ToDouble(right);
                case TokenType.Pow: return Math.Pow(Convert.ToDouble(left),Convert.ToDouble(right));
                default:
                Error.Report(ErrorType.SemanticError,"Operacion Invalida");
                return null;
            }
        }
        else
        {
            Error.Report(ErrorType.SemanticError,"Operacion invalida");
            return null;
        }
    }
}
public class UnaryExpression : Expression
{//Expresiones Unarias
   public Token Symbol{get;}
   public Expression Right{get;}
   public UnaryExpression(Token symbol,Expression right)
   {
    Symbol = symbol;
    Right = right;
   }
    public override object Evaluate(Scope scope)
    {
        object right = Right.Evaluate(scope);
        switch(Symbol.Type)
        {
            case TokenType.Less: return -Convert.ToDouble(right);
            case TokenType.Not: return !(bool)right;
            case TokenType.PlusPlus:
            int value = (int)right;
            int newValue = value + 1;
            scope.Values[(Right as VariableExpression).Name] = newValue;
            return newValue;
            case TokenType.LessLess:
            int value_2 = (int)right;
            int newValue_2 = value_2 - 1;
            scope.Values[(Right as VariableExpression).Name] = newValue_2;
            return newValue_2;
            default:
            Error.Report(ErrorType.SemanticError,"Operacion Invalida");
            return null;
        }
    }
}
public class GroupingExpresion : Expression
{//Expresiones de Agrupamiento
    public Expression Expression{get;}
    public GroupingExpresion(Expression expression)
    {
        Expression = expression;
    }
    public override object Evaluate(Scope scope)
    {
        return Expression.Evaluate(scope);
    }
}
public class AssignExpresion : StatementExpression
{//Expresiones de Asignacion
   public VariableExpression Variable{get;}
   public Token ID{get;}
   public Expression Value{get;}

   public AssignExpresion(VariableExpression variable,Token id,Expression value)
   {
    Variable = variable;
    ID = id;
    Value = value;
   }
   public void Evaluater(Scope scope)
   {
      object rightValue = Value.Evaluate(scope);
      if(ID.Type == TokenType.Equal)
      {
        if(Variable is VariableCompoundExpression)
        {
            (Variable as VariableCompoundExpression).GetValue(scope,rightValue);
        }
        else
        {
            scope.Values[Variable.Name] = Value;
        }
      }
      else if(ID.Type == TokenType.PlusEqual || ID.Type == TokenType.LessEqual)
      {
        object leftValue = scope.Values[Variable.Name];
        if(leftValue is int leftInt &&  rightValue is int rightInt)
        {
            int result = (ID.Type == TokenType.PlusEqual)? leftInt + rightInt : leftInt - rightInt;
            scope.Values[Variable.Name] = result;
        }
        else if(leftValue is double leftDouble && rightValue is double rightDouble)
        {
            double result = (ID.Type == TokenType.PlusEqual)? leftDouble + rightDouble : leftDouble - rightDouble;
            scope.Values[Variable.Name] = result;
        }
        else if(leftValue is Card card && rightValue is int value)
        {
            if(ID.Type==TokenType.PlusEqual)
            {
                card.power += value;
            }
            else if(ID.Type==TokenType.LessEqual)
            {
                card.power -= value;
            }
            scope.Values[Variable.Name] = card;
        }
        else
        {
            Error.Report(ErrorType.SemanticError,"Operacion invalida");
        }
      }
      else
      {
        Error.Report(ErrorType.SemanticError,"Operacion invalida");
      }
   }
}
public class VariableExpression : Expression
{//Variables
    public Token ID{get;}
    public string Name{get;}
    public Type type{get;set;}
    public bool IsConstant{get;set;}
    public enum Type
    {
        INT,STRING,BOOL,NULL,FIELD,TARGETS,VOID,CARD,CONTEXT
    }
    public VariableExpression(Token id)
    {
        ID = id;
        Name = id.Value;
        type = Type.NULL;
    }
    public void SetType(TokenType typeName)
    {
        if(typeName == TokenType.Booleans) type = Type.BOOL;
        if(typeName == TokenType.Numbers) type = Type.INT;
        if(typeName == TokenType.Strings) type = Type.STRING;
    }
    public override object Evaluate(Scope scope)
    {
        return scope.Values[Name];
    }
}
public class VariableCompoundExpression : VariableExpression,StatementExpression
{
    public ParamsExpression Argument{get;}
    public VariableCompoundExpression(Token token) : base(token)
    {
        Argument = new ParamsExpression();
    }
    public void Evaluater(Scope scope)
    {
       object last = null;
       if(Name != "context") last = scope.Values[Name];
       foreach(var arg in Argument.Params)
       {
          if(arg is FunctionExpression)
          {
            last = (arg as FunctionExpression).ValueReturn(scope,last);
          }
          else if(arg is PointerExpression)
          {
            PointerExpression pointer = arg as PointerExpression;
            switch(pointer.Pointer)
            {
                case "Hand": last = scope.Context.HandOfPlayer(scope.Context.TriggerPlayer()); break;
                case "Deck": last = scope.Context.DeckOfPlayer(scope.Context.TriggerPlayer()); break;
                case "Graveyard": last = scope.Context.GraveyardOfPlayer(scope.Context.TriggerPlayer()); break;
                case "Field": last = scope.Context.FieldOfPlayer(scope.Context.TriggerPlayer()); break;
                case "Board": last = scope.Context.Board(); break;
            }
          }
       }
    }
    public override object Evaluate(Scope scope)
    {
        object last = null;
        if(Name != "context") last = scope.Values[Name];
        foreach(var arg in Argument.Params)
        {
            if(arg is FunctionExpression)
            {
                last = (arg as FunctionExpression).ValueReturn(scope,last);
            }
            else if(arg is IndexExpression)
            {
                if(last is List<Card> list)
                {
                    List<Card> cards = list;
                    IndexExpression index = arg as IndexExpression;
                    last = cards[index.Index];
                }
                else
                {
                    string[] range = last as string[];
                    IndexExpression index = arg as IndexExpression;
                    last = range[index.Index]; 
                }
            }
            else if(arg is PointerExpression)
            {
                PointerExpression pointer = arg as PointerExpression;
                switch(pointer.Pointer)
                {
                    case "Hand": last = scope.Context.HandOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Deck": last = scope.Context.DeckOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Graveyard": last = scope.Context.GraveyardOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Field": last = scope.Context.FieldOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Board": last = scope.Context.Board();break;
                }
            }
            else
            {
                Card card = last as Card;
                switch(arg)
                {
                    case TypeExpression : last = card.Type; break;
                    case NameExpression : last = card.name; break;
                    case FactionExpression : last = card.faction; break;
                    case PowerExpression : last = card.power; break;
                    case RangeExpression : last = card.range; break;
                    case OwnerExpression : last = card.owner; break;
                }
            }
        }
        return last;
    }
    public void GetValue(Scope scope,object value)
    {
       object last = scope.Values[Name];
       if(Name == "target")
       {
          last = scope.Values[Name];
       }
       foreach(var arg in Argument.Params)
       {
           if(arg is FunctionExpression)
           {
              last = (arg as FunctionExpression).ValueReturn(scope,last);
           }
           else if(arg is IndexExpression)
           {
              if(last is List<Card> list)
              {
                List<Card> cards = list;
                IndexExpression index = arg as IndexExpression;
                last = cards[index.Index];
              }
              else
              {
                string[] range = last as string[];
                IndexExpression index = arg as IndexExpression;
                range[index.Index] = value as string;
              }
           }
           else if(arg is PointerExpression)
            {
                PointerExpression pointer = arg as PointerExpression;
                switch(pointer.Pointer)
                {
                    case "Hand": last = scope.Context.HandOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Deck": last = scope.Context.DeckOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Graveyard": last = scope.Context.GraveyardOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Field": last = scope.Context.FieldOfPlayer(scope.Context.TriggerPlayer());break;
                    case "Board": last = scope.Context.Board();break;
                }
            }
            else
            {
                Card card = last as Card;
                switch(arg)
                {
                    case TypeExpression : last = card.Type; break;
                    case NameExpression : last = card.name; break;
                    case FactionExpression : last = card.faction; break;
                    case PowerExpression : last = card.power; break;
                    case RangeExpression : last = card.range; break;
                    case OwnerExpression : last = card.owner; break;
                }
            }
       }
    }
}
public interface StatementExpression : Node
{//Instrucciones
   public void Evaluater(Scope scope);
}
public class StatementBlockExpression : Node
{//Bloques de instrucciones
    public List<StatementExpression> expressions{get;set;}
    public StatementBlockExpression()
    {
        expressions = new List<StatementExpression>();
    }
}
public class FunctionExpression : StatementExpression
{//Funciones de las listas de cartas
    public string Name{get;}
    public ParamsExpression ParamsExpression{get;}
    public VariableExpression.Type Type{get; set;}
    public FunctionExpression(string name,ParamsExpression paramsExpression)
    {
        Name = name;
        ParamsExpression = paramsExpression;
        Type = VariableExpression.Type.NULL;
        VariableReturn();
    }
    public void VariableReturn()
    {
        if(Name == "FieldOfPlayer") Type = VariableExpression.Type.CONTEXT;
        if(Name == "HandOfPlayer") Type = VariableExpression.Type.FIELD;
        if(Name == "GraveyardOfPlayer") Type = VariableExpression.Type.FIELD;
        if(Name == "DeckOfPlayer") Type = VariableExpression.Type.FIELD;
        if(Name == "Find") Type = VariableExpression.Type.TARGETS;
        if(Name == "Push") Type = VariableExpression.Type.VOID;
        if(Name == "SendBottom") Type = VariableExpression.Type.VOID;
        if(Name == "Pop") Type = VariableExpression.Type.CARD;
        if(Name == "Remove") Type = VariableExpression.Type.VOID;
        if(Name == "Shuffle") Type = VariableExpression.Type.VOID;
        if(Name == "Add") Type = VariableExpression.Type.VOID;
    }
    public object ValueReturn(Scope scope,object value)
    {
       switch(Name)
       {
         case "TriggerPlayer": return scope.Context.TriggerPlayer();
         case "HandOfPlayer":
          if(ParamsExpression.Params[0] is FunctionExpression) return scope.Context.HandOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as FunctionExpression).ValueReturn(scope,value)));
          else return scope.Context.HandOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as Expression).Evaluate(scope)));
          case "DeckOfPlayer":
          if(ParamsExpression.Params[0] is FunctionExpression) return scope.Context.DeckOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as FunctionExpression).ValueReturn(scope,value)));
          else return scope.Context.DeckOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as Expression).Evaluate(scope)));
          case "GraveyardOfPlayer":
          if(ParamsExpression.Params[0] is FunctionExpression) return scope.Context.GraveyardOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as FunctionExpression).ValueReturn(scope,value)));
          else return scope.Context.GraveyardOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as Expression).Evaluate(scope)));
          case "FieldOfPlayer":
          if(ParamsExpression.Params[0] is FunctionExpression) return scope.Context.FieldOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as FunctionExpression).ValueReturn(scope,value)));
          else return scope.Context.FieldOfPlayer(Convert.ToInt32((ParamsExpression.Params[0] as Expression).Evaluate(scope)));
          case "Find":
          List<Card> cardList = value as List<Card>;
          PredicateExpression predicate = ParamsExpression.Params[0] as PredicateExpression;
          if(cardList!=null && predicate!=null)
          {
            return Find(predicate,cardList);
          }
          return null;
          case "Push":
          Card cardToPush = (ParamsExpression.Params[0] as Expression).Evaluate(scope) as Card;
          if(cardToPush!=null)
          {
            (value as List<Card>).Insert(0,cardToPush);
          }
          else
          {
            Debug.LogWarning("La carta a agregar es nula");
          }
          return null;
          case "SendBottom":
          Card cardToSendBottom = (ParamsExpression.Params[0] as Expression).Evaluate(scope) as Card;
          if(cardToSendBottom!=null)
          {
            (value as List<Card>).Add(cardToSendBottom);
          }
          else
          {
            Debug.LogWarning("La carta a agregar es nula");
          }
          return null;
          case "Pop":
          List<Card> cardList_2 = value as List<Card>;
          if(cardList_2!=null && cardList_2.Count>0)
          {
            Card topCard = cardList_2[0];
            cardList_2.RemoveAt(0);
            return topCard;
          }
          else
          {
            Debug.LogWarning("La lista esta vacia o es nula");
            return null;
          }
          case "Remove":
          Card cardToRemove = (ParamsExpression.Params[0] as Expression).Evaluate(scope) as Card;
          (value as List<Card>).Remove(cardToRemove);
          return null;
          case "Shuffle":
          List<Card> cardsToShuffle = value as List<Card>;
          if(cardsToShuffle!=null && cardsToShuffle.Count>0)
          {
             int n = cardsToShuffle.Count;
             System.Random random = new System.Random();
             while(n>1)
             {
                n--;
                int k = random.Next(n+1);
                Card temp = cardsToShuffle[k];
                cardsToShuffle[k] = cardsToShuffle[n];
                cardsToShuffle[n] = temp;
             }
          }
          else
          {
            Debug.LogWarning("La lista de cartas era nula o estaba vacia");
          }
          return null;
          default : return null; 
       }
    }
    public void Evaluater(Scope scope)
    {
        throw new NotImplementedException();
    }
    public List<Card> Find(PredicateExpression predicate,List<Card> cards)
    {
       List<Card> result = new List<Card>();
       foreach(var card in cards)
       {
         new Scope().Values[predicate.Variable.Name] = card;
         if((bool)predicate.Condition.Evaluate(new Scope())) result.Add(card);
         new Scope().Values.Remove(predicate.Variable.Name);
       }
       return result;
    }
}
public class PointerExpression : Node
{
    public string Pointer{get;}
    public PointerExpression(string pointer)
    {
        Pointer = pointer;
    }
}
public class ForExpression : StatementExpression
{//Ciclos For
    public VariableExpression Variable{get;}
    public VariableExpression Target{get;}
    public StatementBlockExpression Body{get;}
    public ForExpression(VariableExpression variable,VariableExpression target,StatementBlockExpression body)
    {
        Variable = variable;
        Target = target;
        Body = body;
    }
    public void Evaluater(Scope scope)
    {
        foreach(Card target in scope.Values["targets"] as List<Card>)
        {
            scope.Values["target"] = Target;
            foreach(var statement in Body.expressions)
            {
                statement.Evaluater(scope);
            }
            scope.Values.Remove("target");
        }
    }
}
public class WhileExpression : StatementExpression
{//Ciclos while
    public Expression Condition{get;}
    public StatementBlockExpression Body{get;}
    public WhileExpression(Expression condition,StatementBlockExpression body)
    {
        Condition = condition;
        Body = body;
    }
    public void Evaluater(Scope scope)
    {
        while((bool)Condition.Evaluate(scope))
        {
            foreach(var statement in Body.expressions)
            {
                statement.Evaluater(scope);
            }
        }
    }
}
public class EffectExpression : Node
{//Efectos
     public NameExpression Name{get;set;}
     public ParamsExpression Params{get;set;}
     public ActionExpression Action{get;set;}
     public EffectExpression()
     {
     
     }
}
public class NameExpression : Node
{// Nombres de Carta o Efectos
    public Expression Name{get;}
    public NameExpression(Expression name)
    {
        Name = name;
    }
}
public class ParamsExpression : Node
{//Parametros
    public List<Node> Params{get;}
    public ParamsExpression()
    {
        Params = new List<Node>(); 
    }
}
public class ActionExpression : Node
{//Acciones de los efectos
    public VariableExpression Targets{get;}
    public VariableExpression Context{get;}
    public StatementBlockExpression Body{get;}
    public ActionExpression(VariableExpression targets,VariableExpression context,StatementBlockExpression body)
    {
        Targets = targets;
        Context = context;
        Body = body;
    }
}
public class CardExpression : Node
{//Cartas
    public TypeExpression Type{get; set;}
    public NameExpression Name{get; set;}
    public FactionExpression Faction{get; set;}
    public PowerExpression Power{get; set;}
    public RangeExpression Range{get; set;}
    public OnActivationExpression OnActivation{get; set;}
    public CardExpression()
    {
      
    }
}
public class TypeExpression : Node
{//El tipo de la carta
    public Expression Type{get;}
    public TypeExpression(Expression type)
    {
        Type = type;
    }
}
public class FactionExpression : Node
{//La faccion de la carta 
    public Expression Faction{get;}
    public FactionExpression(Expression faction)
    {
        Faction = faction;
    }
}
public class PowerExpression : Node
{//El ataque de la carta
    public Expression Power{get;}
    public PowerExpression(Expression power)
    {
        Power = power;
    }
}
public class RangeExpression : Node
{//Los tipos de ataque de la carta
    public Expression[] Ranges{get;}
    public string Range{get;}
    public RangeExpression(Expression[] ranges)
    {
        Ranges = ranges;
    }
    public RangeExpression(string range)
    {
        Range = range;
    }
}
public class OnActivationExpression : Node
{//Como se comporta una carta al ser convocada
    public List<OnActivationElementsExpression> OnActivation{get;}
    public OnActivationExpression()
    {
        OnActivation = new List<OnActivationElementsExpression>();
    }
}
public class OnActivationElementsExpression : Node
{//Todos los elementos de On Activation
   public EffectCallExpression EffectCall{get;}
   public SelectorExpression Selector{get;}
   public List<PostActionExpression> PostAction{get;}
   public OnActivationElementsExpression(EffectCallExpression effectCall,SelectorExpression selector,List<PostActionExpression> postAction)
   {
     EffectCall = effectCall;
     Selector = selector;
     PostAction = postAction;
   }
}
public class EffectCallExpression : Node
{//LLama un efecto anteriormente definido
    public string Name{get;}
    public List<AssignExpresion> Params{get;}
    public EffectCallExpression(string name,List<AssignExpresion> Params)
    {
        Name = name;
        this.Params = Params;
    }
}
public class SelectorExpression : Node
{//Decide a que cartas se les va a aplicar el efecto
    public string Source{get; set;}
    public SingleExpression Single{get;}
    public PredicateExpression Predicate{get;}
    public SelectorExpression(string source,SingleExpression single,PredicateExpression predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
    }
}
public class SingleExpression : Node
{//Decide cuantos objetivos se van a tomar
    public bool Value{get;}
    public SingleExpression(Token token)
    {
        if(token.Type == TokenType.Booleans)
        {
            if(token.Value == "true") Value = true;
            else Value = false;
        }
    }
}
public class PredicateExpression : Node
{//Filtro para los efectos
    public VariableExpression Variable{get;}
    public Expression Condition{get;}
    public PredicateExpression(VariableExpression variable,Expression condition)
    {
        Variable = variable;
        Condition = condition;
    }
}
public class PostActionExpression : Node
{//Declaracion de otro efecto
    public Expression Type{get;}
    public SelectorExpression Selector{get;}
    public List<AssignExpresion> Body{get;}
    public PostActionExpression(Expression type,SelectorExpression selector)
    {
        Type = type;
        Selector = selector;
        Body = new List<AssignExpresion>();
    }
}
public class ProgramExpression : Node
{
   public List<EffectExpression> CompiledEffects{get;}
   public List<CardExpression> CompiledCards{get;}
   public ProgramExpression()
   {
     CompiledCards = new List<CardExpression>();
     CompiledEffects = new List<EffectExpression>();
   }
}
public class IndexExpression : Node
{
    public int Index{get;}
    public IndexExpression(int Index)
    {
        this.Index = Index;
    }
}
public class OwnerExpression : Node
{
    public string Owner{get;}
    public OwnerExpression(string owner)
    {
        Owner = owner;
    }
}