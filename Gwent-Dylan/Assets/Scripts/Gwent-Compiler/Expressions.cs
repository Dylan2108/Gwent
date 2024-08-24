using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Expression : MonoBehaviour
{
     
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
}
public class LiteralExpression : Expression
{//Literales(Booleanos,Strings,Valores Numericos)
    public object Value{get;}
    public LiteralExpression(object value)
    {
        Value = value;
    }
}
public class GroupingExpresion : Expression
{//Expresiones de Agrupamiento
    public Expression Expression{get;}
    public GroupingExpresion(Expression expression)
    {
        Expression = expression;
    }
}
public class AssignExpresion : Expression
{//Expresiones de Asignacion
   public Token ID{get;}
   public Expression Value{get;}
   public AssignExpresion(Token id,Expression value)
   {
    ID = id;
    Value = value;
   }
}
public class VariableExpression : Expression
{//Variables
    public Token ID{get;}
    public VariableExpression(Token id)
    {
        ID = id;
    }
}
public class StatementExpression : Expression
{//Bloques de instrucciones
    List<Expression> expressions{get;}
    public StatementExpression()
    {
        expressions = new List<Expression>();
    }
}
public class FunctionExpression : Expression //No esta completa
{//Funciones de las listas de cartas
    public string Name{get;}
    public ParamsExpression paramsExpression;
}
public class ForExpression : Expression
{//Ciclos For
    public VariableExpression Variable{get;}
    public VariableExpression Target{get;}
    public StatementExpression Body{get;}
    public ForExpression(VariableExpression variable,VariableExpression target,StatementExpression body)
    {
        Variable = variable;
        Target = target;
        Body = body;
    }
}
public class WhileExpression : Expression
{//Ciclos while
    public Expression Condition{get;}
    public StatementExpression Body{get;}
    public WhileExpression(Expression condition,StatementExpression body)
    {
        Condition = condition;
        Body = body;
    }
}
public class EffectExpression : Expression
{//Efectos
     public NameExpression Name{get;}
     public ParamsExpression Params{get;}
     public ActionExpression Action{get;}
     public EffectExpression(NameExpression name,ParamsExpression Params,ActionExpression action)
     {
        Name = name;
        this.Params = Params;
        Action = action; 
     }
}
public class NameExpression : Expression
{// Nombres de Carta o Efectos
    public Expression Name{get;}
    public NameExpression(Expression name)
    {
        Name = name;
    }
}
public class ParamsExpression : Expression
{//Parametros
    public List<Expression> Params{get;}
    public ParamsExpression()
    {
        Params = new List<Expression>(); 
    }
}
public class ActionExpression : Expression
{//Acciones de los efectos
    public VariableExpression Targets{get;}
    public VariableExpression Context{get;}
    public StatementExpression Body{get;}
    public ActionExpression(VariableExpression targets,VariableExpression context,StatementExpression body)
    {
        Targets = targets;
        Context = context;
        Body = body;
    }
}
public class CardExpression : Expression
{//Cartas
    public TypeExpression Type{get;}
    public NameExpression Name{get;}
    public FactionExpression Faction{get;}
    public PowerExpression Power{get;}
    public RangeExpression Range{get;}
    public OnActivationExpression OnActivation{get;}
    public CardExpression(TypeExpression type,NameExpression name,FactionExpression faction,PowerExpression power,RangeExpression range,OnActivationExpression onActivation)
    {
        Type = type;
        Name = name;
        Faction = faction;
        Power = power;
        Range = range;
        OnActivation = onActivation;
    }
}
public class TypeExpression : Expression
{//El tipo de la carta
    public Expression Type{get;}
    public TypeExpression(Expression type)
    {
        Type = type;
    }
}
public class FactionExpression : Expression
{//La faccion de la carta 
    public Expression Faction{get;}
    public FactionExpression(Expression faction)
    {
        Faction = faction;
    }
}
public class PowerExpression : Expression
{//El ataque de la carta
    public Expression Power{get;}
    public PowerExpression(Expression power)
    {
        Power = power;
    }
}
public class RangeExpression : Expression
{//Los tipos de ataque de la carta
    public List<Expression> Ranges{get;}
    public string Range{get;}
    public RangeExpression(List<Expression> ranges)
    {
        Ranges = ranges;
    }
    public RangeExpression(string range)
    {
        Range = range;
    }
}
public class OnActivationExpression : Expression
{//Como se comporta una carta al ser convocada
    public List<OnActivationElementsExpression> OnActivation{get;}
    public OnActivationExpression()
    {
        OnActivation = new List<OnActivationElementsExpression>();
    }
}
public class OnActivationElementsExpression : Expression
{//Todos los elementos de los elementos de On Activation
   public EffectCallExpression EffectCall{get;}
   public SelectorExpression Selector{get;}
   public PostActionExpression PostAction{get;}
   public OnActivationElementsExpression(EffectCallExpression effectCall,SelectorExpression selector,PostActionExpression postAction)
   {
     EffectCall = effectCall;
     Selector = selector;
     PostAction = postAction;
   }
}
public class EffectCallExpression : Expression
{//LLama un efecto anteriormente definido
    public string Name{get;}
    public List<AssignExpresion> Params{get;}
    public EffectCallExpression(string name,List<AssignExpresion> Params)
    {
        Name = name;
        this.Params = Params;
    }
}
public class SelectorExpression : Expression
{//Decide a que cartas se les va a aplicar el efecto
    public string Source{get;}
    public SingleExpression Single{get;}
    public PredicateExpression Predicate{get;}
    public SelectorExpression(string source,SingleExpression single,PredicateExpression predicate)
    {
        Source = source;
        Single = single;
        Predicate = predicate;
    }
}
public class SingleExpression : Expression
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
public class PredicateExpression : Expression
{//Filtro para los efectos
    public VariableExpression Variable{get;}
    public Expression Condition{get;}
    public PredicateExpression(VariableExpression variable,Expression condition)
    {
        Variable = variable;
        Condition = condition;
    }
}
public class PostActionExpression : Expression
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
public class ProgramExpression : Expression
{
   public List<EffectExpression> CompiledEffects{get;}
   public List<CardExpression> CompiledCards{get;}
   public ProgramExpression()
   {
     CompiledCards = new List<CardExpression>();
     CompiledEffects = new List<EffectExpression>();
   }
}