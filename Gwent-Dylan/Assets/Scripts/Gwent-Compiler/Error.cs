using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum ErrorType
{
    LexicalError, // Ocurren cuando el Lexer encuentra un token invalido
    SyntaxError, //Ocurren cuando el parser encuentra una expresion invalida
    SemanticError //Ocurren cuando el evaluador encuentra una expresion invalida
}
public  class Error : Exception 
{
    public string Message{get;}
    public ErrorType ErrorType{get;}
    public Error(string message,ErrorType errorType)
    {
        Message = message;
        ErrorType = errorType;
    }
    public string Report()
    {
        return $"{ErrorType}: {Message}";
    }
}