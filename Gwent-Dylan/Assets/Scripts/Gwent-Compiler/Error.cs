using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ErrorType
{
    LexicalError, // Ocurren cuando el Lexer encuentra un token invalido
    SyntaxError, //Ocurren cuando el parser encuentra una expresion invalida
    SemanticError //Ocurren cuando el evaluador encuentra una expresion invalida
}
public static class Error 
{
    public static void Report(ErrorType error,string message)
    {
        Debug.LogError($"Error: {error}, {message}");
    }
}