using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Expression
{
  
}
public class NumberExpression : Expression
{
    public int Value{get;}
    public NumberExpression(int value)
    {
        Value = value;
    }
}
public class StringExpression : Expression
{
    public string Value{get;}
    public StringExpression(string value)
    {
        Value = value;
    }
    public object Evaluate(Scope scope)
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
public class GroupingExpresion : Expression
{//Expresiones de Agrupamiento
    public Expression Expression{get;}
    public GroupingExpresion(Expression expression)
    {
        Expression = expression;
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
}
public class VariableCompoundExpression : VariableExpression,StatementExpression
{
    public ParamsExpression Argument{get;}
    public VariableCompoundExpression(Token token) : base(token)
    {
        Argument = new ParamsExpression();
    }
}
public interface StatementExpression : Expression
{//Instrucciones

}
public class StatementBlockExpression : Expression
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
}
public class PointerExpression : Expression
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
}
public class EffectExpression : Expression
{//Efectos
     public NameExpression Name{get;set;}
     public ParamsExpression Params{get;set;}
     public ActionExpression Action{get;set;}
     public EffectExpression()
     {
     
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
    public StatementBlockExpression Body{get;}
    public ActionExpression(VariableExpression targets,VariableExpression context,StatementBlockExpression body)
    {
        Targets = targets;
        Context = context;
        Body = body;
    }
}
public class CardExpression : Expression
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
public class OnActivationExpression : Expression
{//Como se comporta una carta al ser convocada
    public List<OnActivationElementsExpression> OnActivation{get;}
    public OnActivationExpression()
    {
        OnActivation = new List<OnActivationElementsExpression>();
    }
}
public class OnActivationElementsExpression : Expression
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