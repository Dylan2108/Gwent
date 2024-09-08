using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class CompileCards : MonoBehaviour
{
    public TMP_InputField inputText;
    public Scope scope;
    public void Compile()
    {
         string input = inputText.text;
         ProcessText(input);
    }
    public void ProcessText(string input)
    {
         Lexer lexer = new Lexer(input);
         List<Token> tokens = lexer.GetTokens();
         Parser parser = new Parser(tokens);
         Node AST = parser.ParseProgram();
         Semantic semantic = new Semantic();
         semantic.CheckProgram(AST);
    }
}