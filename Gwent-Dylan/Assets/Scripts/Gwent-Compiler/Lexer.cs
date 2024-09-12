using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Lexer : MonoBehaviour
{
   private string input; //El texto a analizar
   private List<Token> tokens;//La lista de Tokens en que es convertido el texto de entrada
   private int Start;
   private int Current;
   private int Line;
   public TMP_InputField errorText;//El texto para los errores
   private readonly Dictionary<string,Token> Keywords = new Dictionary<string, Token>();//Diccionario que contiene las palabras claves
   public Lexer(string input)
   {
    this.input = input;
    tokens = new List<Token>();
    Start = 0;
    Current = 0;
    Line = 1;
    Keywords["card"] = new Token("card",TokenType.card,"card",0);
    Keywords["effect"] = new Token("effect",TokenType.effect,"effect",0);
    Keywords["Name"] = new Token("Name",TokenType.Name,"Name",0);
    Keywords["Params"] = new Token("Params",TokenType.Params,"Params",0);
    Keywords["Action"] = new Token("Action",TokenType.Action,"Action",0);
    Keywords["while"] = new Token("while",TokenType.While,"while",0);
    Keywords["for"] = new Token("for",TokenType.For,"for",0);
    Keywords["in"] = new Token("in",TokenType.In,"in",0);
    Keywords["TriggerPlayer"] = new Token("TriggerPlayer",TokenType.Function,"TriggerPlayer",0);
    Keywords["HandOfPlayer"] = new Token("HandOfPlayer",TokenType.Function,"HandOfPlayer",0);
    Keywords["FieldOfPlayer"] = new Token("FieldOfPlayer",TokenType.Function,"FieldOfPlayer",0);
    Keywords["GraveyardOfPlayer"] = new Token("GraveyardOfPlayer",TokenType.Function,"GraveyardOfPlayer",0);
    Keywords["DeckOfPlayer"] = new Token("DeckOfPlayer",TokenType.Function,"DeckOfPlayer",0);
    Keywords["Hand"] = new Token("Hand",TokenType.Pointer,"Hand",0);
    Keywords["Field"] = new Token("Field",TokenType.Pointer,"Field",0);
    Keywords["Graveyard"] = new Token("Graveyard",TokenType.Pointer,"Graveyard",0);
    Keywords["Deck"] = new Token("Deck",TokenType.Pointer,"Deck",0);
    Keywords["Board"] = new Token("Board",TokenType.Pointer,"Board",0);
    Keywords["Push"] = new Token("Push",TokenType.Function,"Push",0);
    Keywords["SendBottom"] = new Token("SendBottom",TokenType.Function,"SendBottom",0);
    Keywords["Pop"] = new Token("Pop",TokenType.Function,"Pop",0);
    Keywords["Remove"] = new Token("Remove",TokenType.Function,"Remove",0);
    Keywords["Shuffle"] = new Token("Shuffle",TokenType.Function,"Shuffle",0);
    Keywords["Find"] = new Token("Find",TokenType.Function,"Find",0);
    Keywords["Type"] = new Token("Type",TokenType.Type,"Type",0);
    Keywords["Faction"] = new Token("Faction",TokenType.Faction,"Faction",0);
    Keywords["Power"] = new Token("Power",TokenType.Power,"Power",0);
    Keywords["Range"] = new Token("Range",TokenType.Range,"Range",0);
    Keywords["Owner"] = new Token("Owner",TokenType.Owner,"Owner",0);
    Keywords["OnActivation"] = new Token("OnActivation",TokenType.OnActivation,"OnActivation",0);
    Keywords["Effect"] = new Token("Effect",TokenType.Effect,"Effect",0);
    Keywords["Selector"] = new Token("Selector",TokenType.Selector,"Selector",0);
    Keywords["Source"] = new Token("Source",TokenType.Source,"Source",0);
    Keywords["Single"] = new Token("Single",TokenType.Single,"Single",0);
    Keywords["Predicate"] = new Token("Predicate",TokenType.Predicate,"Predicate",0);
    Keywords["PostAction"] = new Token("PostAction",TokenType.PostAction,"PostAction",0);
    Keywords["Number"] = new Token("Number",TokenType.Numbers,"Number",0);
    Keywords["String"] = new Token("String",TokenType.Strings,"String",0);
    Keywords["Bool"] = new Token("Bool",TokenType.Booleans,"Bool",0);
    Keywords["true"] = new Token("true",TokenType.True,"true",0);
    Keywords["false"] = new Token("false",TokenType.False,"false",0);
   }
   public void ShowError(string error)//Muestra el error en pantalla
   {
       errorText.text = error;
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
           else
           {
              string error = $"Error Lexico. Caracter inesperado : linea {Line}";
              ShowError(error);
              throw new Error($"Caracter inesperado : linea {Line}",ErrorType.LexicalError);
           }
           break;
        case '|':
           if(Match('|')) AddToken(TokenType.Or);
           else
           {
               string error = $"Error Lexico. Caracter inesperado : linea {Line}";
               ShowError(error);
               throw new Error($"Caracter inesperado: linea {Line}",ErrorType.LexicalError);
           }
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
        if(IsDigit(c))
        {
            Number();
        }
        else if(IsAlpha(c) || c == '_')
        {
            Identifier();
        }
        else
        {
            string error = $"Error Lexico. El caracter {c} es incorrecto : linea {Line}";
            ShowError(error);
            throw new Error($"El caracter {c} es incorrecto : linea {Line}",ErrorType.LexicalError);
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
   private char PeekNext()
   {
      if(Current + 1 >= input.Length) return '\0';
      return input[Current + 1];
   }   
   private bool IsDigit(char c)
   {
      return c>= '0' && c <= '9';
   }
   private bool IsAlpha(char c)
   {
      return (c>= 'a' && c<= 'z') || (c>='A' && c<='Z') || c=='_';
   }
   private bool IsAlphaNumeric(char c)
   {
      return IsDigit(c) || IsAlpha(c);
   }
   private void String()
   {//Permite obtener el valor de un string y agregarlo a la lista de tokens
      while(Peek() !='"' && !IsAtEnd())
      {
        if(Peek() == '\n') Line++;
        Advance();
      }
      if(IsAtEnd())
      {
         string error = $"Error Lexico. Cadena sin terminar : linea {Line} ";
         ShowError(error);
         throw new Error($"Cadena sin terminar : linea{Line}",ErrorType.LexicalError);
      }
      Advance();
      string value = input.Substring(Start + 1,Current-(Start + 1));
      AddToken(TokenType.Strings,value);
   }
   private void Number()
   {//Obtiene el valor del numero que se esta analizando
      while(IsDigit(Peek())) Advance();
      if(Peek() == '.' && IsDigit(PeekNext()))
      {
         Advance();
         while(IsDigit(Peek())) Advance();
      }
      AddToken(TokenType.Numbers,double.Parse(input.Substring(Start,Current - Start)));
   }
   private void Identifier()
   {//Determina si una palabra es una palabra clave o un identificador
     while(IsAlphaNumeric(Peek())) Advance();
     string text = input.Substring(Start,Current - Start);
     TokenType tokenType = TokenType.Identifiers;
     if(Keywords.ContainsKey(text))
     {
        Token token = Keywords[text];
        tokens.Add(new Token(token.Value,token.Type,token.Literal,Line));
     }
     else
     {
        AddToken(tokenType);
     }
   }
}