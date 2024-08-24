using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lexer : MonoBehaviour
{
   private string input; //El texto a analizar
   private List<Token> tokens;
   private int Start;
   private int Current;
   private int Line;
   private readonly Dictionary<string,TokenType> Keywords = new Dictionary<string, TokenType>{{"card",TokenType.card},{"effect",TokenType.effect},
   {"Name",TokenType.Name},{"Params",TokenType.Params},{"Action",TokenType.Action},{"while",TokenType.While},{"for",TokenType.For},{"in",TokenType.In},
   {"TriggerPlayer",TokenType.TriggerPlayer},{"HandOfPlayer",TokenType.HandOfPlayer},{"FieldOfPlayer",TokenType.FieldOfPlayer},
   {"GraveyardOfPlayer",TokenType.GraveyardOfPlayer},{"DeckOfPlayer",TokenType.DeckOfPlayer},{"Hand",TokenType.Hand},{"Field",TokenType.Field},
   {"Graveyard",TokenType.Graveyard},{"Deck",TokenType.Deck},{"Board",TokenType.Board},{"Push",TokenType.Push},{"SendBottom",TokenType.SendBottom},
   {"Pop",TokenType.Pop},{"Remove",TokenType.Remove},{"Shuffle",TokenType.Shuffle},{"Find",TokenType.Find},{"Type",TokenType.Type},{"Faction",TokenType.Faction},
   {"Power",TokenType.Power},{"Range",TokenType.Range},{"Owner",TokenType.Owner},{"OnActivation",TokenType.OnActivation},{"Effect",TokenType.Effect},
   {"Selector",TokenType.Selector},{"Source",TokenType.Source},{"Single",TokenType.Single},{"Predicate",TokenType.Predicate},
   {"PostAction",TokenType.PostAction},{"Number",TokenType.Numbers},{"String",TokenType.Strings},{"Bool",TokenType.Booleans},{"true",TokenType.True},
   {"false",TokenType.False}};
   public Lexer(string input)
   {
    this.input = input;
    tokens = new List<Token>();
    Start = 0;
    Current = 0;
    Line = 1;
   }
   public List<Token> GetTokens()
   {//Escanea el texto de entrada y genera la lista de tokens
      while(!IsAtEnd())
      {
        Start = Current;
        ScanToken();
      }
      tokens.Add(new Token("",TokenType.EOF,null,Line));
      return tokens;
   }
   private bool IsAtEnd()
   {//Verifica si ya se llego al final del texto
      return Current >= input.Length;
   }
   private void AddToken(TokenType type)
   {
      AddToken(type,null); 
   }
   private void AddToken(TokenType type,object literal)
   {//Agrega el token con su respectivo valor a la lista
      string text = input.Substring(Start,Current - Start);
      tokens.Add(new Token(text,type,literal,Line));
   }
   private void ScanToken()
   {//Escanea el caracter actual y verifica que tipo de token es
     char c = Advance();
     switch(c)
     {
        case '(' : AddToken(TokenType.LeftParenthesis); break;
        case ')' : AddToken(TokenType.RightParenthesis); break;
        case '[' : AddToken(TokenType.LeftBracket); break;
        case ']' : AddToken(TokenType.RightBracket); break;
        case '{' : AddToken(TokenType.LeftBrace); break;
        case '}' : AddToken(TokenType.RightBrace); break;
        case ',' : AddToken(TokenType.Comma); break;
        case '.' : AddToken(TokenType.Dot); break;
        case ';' : AddToken(TokenType.SemiColon); break;
        case ':' : AddToken(TokenType.Colon); break;
        case '^' : AddToken(TokenType.Pow); break;
        case '%' : AddToken(TokenType.Modulus); break;
        case '!' : AddToken(Match('=')? TokenType.NotEqual : TokenType.Not); break;
        case '=' : AddToken(Match('=')? TokenType.EqualEqual : Match('>')? TokenType.Lambda : TokenType.Equal); break;
        case '<' : AddToken(Match('=')? TokenType.LessEqualThan : TokenType.LessThan); break;
        case '>' : AddToken(Match('=')? TokenType.GreatEqualThan : TokenType.GreaterThan); break;
        case '+' : AddToken(Match('=')? TokenType.PlusEqual : Match('+')? TokenType.PlusPlus : TokenType.Plus); break;
        case '-' : AddToken(Match('=')? TokenType.LessEqual : Match('-')? TokenType.LessLess : TokenType.Less); break;
        case '*' : AddToken(Match('=')? TokenType.MultiplyEqual : TokenType.Multiply); break;
        case '@' : AddToken(Match('=')? TokenType.ConcatenationEqual : Match('@')? TokenType.SpaceConcatenation : TokenType.Concatenation); break;
        case '&':
           if(Match('&')) AddToken(TokenType.And);
           else Error.Report(ErrorType.LexicalError,"Caracter inesperado");
           break;
        case '|':
           if(Match('|')) AddToken(TokenType.Or);
           else Error.Report(ErrorType.LexicalError,"Caracter inesperado");
           break;
        case '/':
           if(Match('/'))
           {
             while(Peek() != '\n' && !IsAtEnd()) Advance();
           }
           else
           {
              AddToken(Match('=')? TokenType.DivideEqual : TokenType.Divide);
           }
           break;
           //Para ignorar los espacios en blanco
        case ' ':
        case '\r':
        case '\t':
        break;
        case '\n': Line++; break;
        case '"' : String(); break;
        default:
        if(char.IsDigit(c))
        {
            Number();
        }
        else if(char.IsLetter(c) || c == '_')
        {
            Identifier();
        }
        else
        {
            Error.Report(ErrorType.LexicalError,$"El caracter {c} es incorrecto");
        }
        break;
     }
   }
   private char Advance()
   {//Devuelve el caracter actual y avanza al siguiente
      Current++;
      return input[Current - 1];
   }
   private bool Match(char expected)
   {//Devuelve verdadero si el caracter actual coincide con el esperado
     if(IsAtEnd()) return false;
     if(input[Current] != expected) return false;
     Current++;
     return true;
   }
   private char Peek()
   {//Devuelve el caracter actual y no avanza al siguiente
      if(IsAtEnd()) return '\0'; //Si ya llego al final devuelve el valor predeterminado de char
      return input[Current];
   }
   private void String()
   {//Permite obtener el valor de un string y agregarlo a la lista de tokens
      while(Peek() !='"' && !IsAtEnd())
      {
        if(Peek() == '\n') Line++;
        Advance();
      }
      if(IsAtEnd()) Error.Report(ErrorType.LexicalError,"Error ,cadena sin terminar");
      Advance();
      string value = input.Substring(Start + 1,Current-(Start + 1));
      AddToken(TokenType.Strings,value);
   }
   private void Number()
   {//Obtiene el valor del numero que se esta analizando
      int dotCounter = 0;
      bool isValidNumber = true;
      while(char.IsLetterOrDigit(Peek()) || Peek() == '.')
      {
        if(Peek() == '.') dotCounter++;
        if(char.IsLetter(Peek())) isValidNumber = false;
        Advance();
      }
      if(dotCounter == 1 && (IsAtEnd() || Peek() == '.')) isValidNumber = false;
      if(dotCounter > 1 || !isValidNumber) Error.Report(ErrorType.LexicalError,$"Token invalido en '{input.Substring(Start,Current-Start)}'");
      else AddToken(TokenType.Numbers,double.Parse(input.Substring(Start,Current - Start)));
   }
   private void Identifier()
   {//Determina si una palabra es una palabra clave o un identificador
     TokenType tokenType;
     while(char.IsLetterOrDigit(Peek()) || Peek() == '_') Advance();
     string text = input.Substring(Start,Current - Start);
     if(Keywords.ContainsKey(text))
     {
        tokenType = Keywords[text];
     }
     else
     {
        tokenType = TokenType.Identifiers;
     }
     AddToken(tokenType);
   }
}