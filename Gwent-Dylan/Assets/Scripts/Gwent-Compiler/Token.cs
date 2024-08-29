using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TokenType
   {
      //Simbolos
      LeftParenthesis,RightParenthesis,LeftBracket,RightBracket,LeftBrace,RightBrace,Comma,Dot,Colon,SemiColon,Modulus,Pow,
      //Simbolos de varios Caracteres
      Not,NotEqual,Plus,PlusEqual,PlusPlus,Less,LessEqual,LessLess,Multiply,MultiplyEqual,Divide,DivideEqual,Equal,EqualEqual,Lambda,GreaterThan,
      GreatEqualThan,LessThan,LessEqualThan,Concatenation,SpaceConcatenation,ConcatenationEqual,And,Or,
      //Tipos de Literales
      Identifiers,StringsLiterals,NumbersLiterals,True,False,
      //Principales palabras reservadas
      card,effect,
      //Palabras reservadas para los efectos
      Name,Params,Action,
      //Palabras reservadas para ciclos
      While,For,In,
      //Propiedades del contexto
      TriggerPlayer,Board,HandOfPlayer,FieldOfPlayer,GraveyardOfPlayer,DeckOfPlayer,Hand,Field,Graveyard,Deck,Function,
      //Funciones
      Find,Push,SendBottom,Pop,Remove,Shuffle,
      //Cartas
      Type,Faction,Power,Range,Owner,OnActivation,Pointer,
      //On Activation
      Effect,Selector,Source,Single,Predicate,PostAction,
      //Tipos de Datos
      Numbers,Strings,Booleans,
      //Final del input
      EOF
   }
public class Token : MonoBehaviour
{
   
    public string Value{get; private set;}
    public TokenType Type{get; private set;}
    public object Literal{get; private set;}
    public int Line{get; private set;}
    public Token(string value,TokenType type,object literal,int line)
    {
        Value = value;
        Type = type;
        Literal = literal;
        Line = line;
    }
    public override string ToString()
    {
        return Type + " " + Value + " " + Literal;
    }   
}