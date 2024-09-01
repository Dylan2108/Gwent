using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.Text;

public class Parser : MonoBehaviour
{
    private List<Token> Tokens;
    private int CurrentPosition;
    public Parser(List<Token> tokens)
    {
        Tokens = tokens;
        CurrentPosition = 0;
    }
    public Expression ParseProgram()//Parsea un programa
    {
       ProgramExpression program = new ProgramExpression();
       while(!IsAtEnd())
       {
          if(Match(TokenType.effect))
          {
            Consume(TokenType.LeftBrace,"Se esperaba '{'");
            program.CompiledEffects.Add(ParseEffect());
            Consume(TokenType.RightBrace,"Se esperaba '}'");
          }
          else if(Match(TokenType.card))
          {
            Consume(TokenType.LeftBrace,"Se esperaba '{'");
            program.CompiledCards.Add(ParseCard());
            Consume(TokenType.RightBrace,"Se esperaba '}'");
          }
          else
          {
            Error.Report(ErrorType.SyntaxError,"Se esperaba la declaracion de una carta o un efecto");
          }
       }
       return program;
    }
    private CardExpression ParseCard()//Parsea una carta
    {
        CardExpression card = new CardExpression();
        int[] elements = new int[6];
        while(!Check(TokenType.RightBrace) && !IsAtEnd())
        {
            if(Match(TokenType.Type))
            {
                elements[0] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                card.Type = new TypeExpression(Expression());
                Consume(TokenType.Comma,"Se esperaba ','");
            }
            else if(Match(TokenType.Name))
            {
                elements[1] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                card.Name = new NameExpression(Expression());
                Consume(TokenType.Comma,"Se esperaba ','");
            }
            else if(Match(TokenType.Faction))
            {
                elements[2] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                card.Faction = new FactionExpression(Expression());
                Consume(TokenType.Comma,"Se esperaba ','");
            }
            else if(Match(TokenType.Power))
            {
                elements[3] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                card.Power = new PowerExpression(Expression());
                Consume(TokenType.Comma,"Se esperaba ','");
            }
            else if(Match(TokenType.Range))
            {
                elements[4] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                Consume(TokenType.LeftBracket,"Se esperaba '['");
                List<Expression> expressions = new List<Expression>();
                for(int i = 0;i<3;i++)
                {
                    expressions.Add(Expression());
                    if(Match(TokenType.Comma)) continue;
                    else break;
                }
                Consume(TokenType.RightBracket,"Se esperaba ']'");
                Consume(TokenType.Comma,"Se esperaba ','");
                card.Range = new RangeExpression(expressions.ToArray());
            }
            else if(Match(TokenType.OnActivation))
            {
                elements[5] += 1;
                card.OnActivation = ParseOnActivation();
            }
            else
            {
                Error.Report(ErrorType.SyntaxError,"Se definio incorrectamente la carta");
            }
        }
        if(elements[0] < 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad Type");
        }
        else if(elements[0] > 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un Type");
        }
        if(elements[1] < 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad Name");
        }
        else if(elements[1] > 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un Type");
        }
        if(elements[2]<1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad Faction");
        }
        else if(elements[2]>1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un Faction");
        }
        if(elements[3]<1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad Power");
        }
        else if(elements[3]>1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un Power");
        }
        if(elements[4]<1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad Range");
        }
        else if(elements[4]>1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un range");
        }
        if(elements[5]<1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad OnActivation");
        } 
        else if(elements[5]>1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un OnActivation");
        }
        return card;
    }
    private EffectExpression ParseEffect()//Parsea un efecto
    {
        EffectExpression effect = new EffectExpression();
        int[] elements = new int[3];
        while(!Check(TokenType.RightBrace) && !IsAtEnd())
        {
            if(Match(TokenType.Name))
            {
                elements[0] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                effect.Name = new NameExpression(Expression());
                Consume(TokenType.Comma,"Se esperaba ','");
            }
            else if(Match(TokenType.Params))
            {
                elements[1] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                effect.Params = ParseParams();
            }
            else if(Match(TokenType.Action))
            {
                elements[2] += 1;
                Consume(TokenType.Colon,"Se esperaba ':'");
                effect.Action = ParseAction();
            }
            else
            {
                Error.Report(ErrorType.SyntaxError,"Se definio incorrectamente el efecto");
            }
        }
        if(elements[0] < 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad Name");
        }
        else if(elements[0] > 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un Name");
        }
        if(elements[1] > 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede puede definir mas de un Params");
        }
        if(elements[2] < 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio correctamente la propiedad Action");
        }
        else if(elements[2] > 1)
        {
            Error.Report(ErrorType.SyntaxError,"No se puede definir mas de un Action");
        }
        return effect;
    }
    private OnActivationExpression ParseOnActivation()//Parsea una expresion de OnActivation
    {
       Consume(TokenType.Colon,"Se esperaba ':'");
       Consume(TokenType.LeftBracket,"Se esperaba '['");
       OnActivationExpression onActivation = new OnActivationExpression();
       while(!Check(TokenType.RightBracket) && !IsAtEnd())
       {
          onActivation.OnActivation.Add(ParseOnActivationElements());
       }
       Consume(TokenType.RightBracket,"Se esperaba ']'");
       return onActivation;
    }
    private ParamsExpression ParseParams()//Parsea una expresion de parametros
    {
       Consume(TokenType.LeftBrace,"Se esperaba '{'");
       ParamsExpression Params = new ParamsExpression();
       while(!Check(TokenType.RightBrace) && !IsAtEnd())
       {
         VariableExpression variable = ParseVariable();
         Consume(TokenType.Colon,"Se esperaba ':'");
         if(Check(TokenType.Numbers) || Check(TokenType.Strings) || Check(TokenType.Booleans))
         {
            variable.SetType(Advance().Type);
            Params.Params.Add(variable);
            if(!Check(TokenType.RightBrace))
            {
                Consume(TokenType.Comma,"Se esperaba ','");
            }
         }
         else
         {
            Error.Report(ErrorType.SyntaxError,"Se esperaba la definicion de Params");
         }
       }
       Consume(TokenType.RightBrace,"Se espera '}'");
       return Params;
    }
    private OnActivationElementsExpression ParseOnActivationElements()//Parsea una expresion de OnActivationElements
    {
       Consume(TokenType.LeftBrace,"Se esperaba '{'");
       EffectCallExpression effectCall = null;
       SelectorExpression selector = null;
       List<PostActionExpression> postAction = new List<PostActionExpression>();
       while(!Check(TokenType.RightBrace) && !IsAtEnd())
       {
          if(Match(TokenType.Effect))
          {
            if(effectCall == null)
            {
                Consume(TokenType.Colon,"Se esperaba ':'");
                effectCall = ParseEffectCall();
            }
          }
          else if(Match(TokenType.Selector))
          {
            if(selector == null)
            {
                Consume(TokenType.Colon,"Se esperaba ':'");
                selector = ParseSelector();
                if(selector.Source == null) Error.Report(ErrorType.SyntaxError,"Falto la definicion de Source");
            }
          }
          else if(Match(TokenType.PostAction))
          {
            
                Consume(TokenType.Colon,"Se esperaba :");
                postAction.Add(ParsePostAction());
          }
          else
          {
            Error.Report(ErrorType.SyntaxError,"Expresion de OnActivation invalida");
          }
       }
       Consume(TokenType.RightBrace,"Se esperaba '}'");
       return new OnActivationElementsExpression(effectCall,selector,postAction);
    }
    private EffectCallExpression ParseEffectCall()//Parsea una expresion de Effect
    {
        string value = null;
        List<AssignExpresion> assign = new List<AssignExpresion>();
        if(Check(TokenType.Strings))
        {
            value = Advance().Value;
            Consume(TokenType.Comma,"Se esperaba ','");
            while(!Check(TokenType.Selector))
            {
                if(Check(TokenType.Identifiers))
                {
                    VariableExpression variable = ParseVariable();
                    Token token = Peek();
                    Consume(TokenType.Colon,"Se esperaba :");
                    Expression expression = Expression();
                    AssignExpresion variable_assign = new AssignExpresion(variable,token,expression);
                    assign.Add(variable_assign);
                    Consume(TokenType.Comma,"Se esperaba ','");
                }
            }
        }
        else
        {
            Consume(TokenType.LeftBrace,"Se esperaba '{'");
            while(!Check(TokenType.RightBrace) && !IsAtEnd())
            {
                if(Match(TokenType.Name))
                {
                    if(Match(TokenType.Colon))
                    {
                        if(value == null)
                        {
                            value = Advance().Value;
                           if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
                        }
                        else
                        {
                            Error.Report(ErrorType.SyntaxError,"Ya habia sido definido un efecto");
                        }
                    }
                    else
                    {
                        if(value == null)
                        {
                            value = Advance().Value;
                            if(!Check(TokenType.RightBrace)) Consume(TokenType.RightBrace,"Se esperaba '}'");
                        }
                        else
                        {
                            Error.Report(ErrorType.SyntaxError,"Ya habia sido definido un efecto");
                        }
                    }
                }
                else if(Check(TokenType.Identifiers))
                {
                    VariableExpression variable = ParseVariable();
                    Token token = Peek();
                    Consume(TokenType.Colon,"Se esperaba ':'");
                    Expression expression = Expression();
                    AssignExpresion assign1 = new AssignExpresion(variable,token,expression);
                    assign.Add(assign1);
                    if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
                }
            }
            Consume(TokenType.RightBrace,"Se esperaba '}'");
        }
        if(value==null)
        {
            Error.Report(ErrorType.SyntaxError,"No se definio un efecto");
            return null;
        }
        return new EffectCallExpression(value,assign);
    }
    private SelectorExpression ParseSelector()//Parsea una expresion de selector
    {
        Consume(TokenType.LeftBrace,"Se esperaba '{'");
        string source = null;
        SingleExpression single = null;
        PredicateExpression predicate = null;
        while(!Check(TokenType.RightBrace) && !IsAtEnd())
        {
            if(Match(TokenType.Source))
            {
                Consume(TokenType.Colon,"Se esperaba ':'");
                if(source == null)
                {
                    if(Convert.ToString(Peek().Value)=="deck" || Convert.ToString(Peek().Value)=="otherdeck" || Convert.ToString(Peek().Value)=="hand" || Convert.ToString(Peek().Value)=="otherhand" || Convert.ToString(Peek().Value)=="field" || Convert.ToString(Peek().Value)=="otherfield" || Convert.ToString(Peek().Value)=="board" || Convert.ToString(Peek().Value)=="parent")
                    {
                        source = Advance().Value;
                    }
                    else
                    {
                        Error.Report(ErrorType.SyntaxError,"Se esperaba la definicion de un Source");
                    }
                }
                else
                {
                    Error.Report(ErrorType.SyntaxError,"No puede definir mas de un Source");
                }
                if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
            }
            else if(Match(TokenType.Single))
            {
                Consume(TokenType.Colon,"Se esperaba :");
                if(single == null)
                {
                    single = new SingleExpression(Advance());
                }
                else
                {
                    Error.Report(ErrorType.SyntaxError,"No puede definir mas de un Single");
                }
                if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
            }
            else if(Match(TokenType.Predicate))
            {
                Consume(TokenType.Colon,"Se esperaba ':' ");
                if(predicate == null)
                {
                    predicate = ParsePredicate();
                }
                else
                {
                    Error.Report(ErrorType.SyntaxError,"No puede definir mas de un Predicate");
                }
                if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
            }
            else
            {
                Error.Report(ErrorType.SyntaxError,"Se esperaba la implementacion de los elementos de un Selector");
            }
        }
        Consume(TokenType.RightBrace,"Se esperaba '}'");
        if(single==null! || predicate==null!)
        {
            Error.Report(ErrorType.SyntaxError,"Se esperaba la implementacion de los elementos de un Selector");
            return null;
        }
        return new SelectorExpression(source,single,predicate);
    }
    private PostActionExpression ParsePostAction()//Parsea una expresion PostAction
    {
       Consume(TokenType.LeftBrace,"Se esperaba '('");
       Expression type = null;
       SelectorExpression selector = null;
       List<AssignExpresion> body = new List<AssignExpresion>();
       while(!Check(TokenType.RightBrace) && !IsAtEnd())
       {
         if(Match(TokenType.Type))
         {
            Consume(TokenType.Colon,"Se esperaba ':'");
            type = Expression();
            if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
         }
         else if(Match(TokenType.Selector))
         {
            Consume(TokenType.Colon,"Se esperaba ':'");
            selector = ParseSelector();
            if(selector.Source == null)
            {
               selector.Source = "parent";
            }
            if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
         }
         else if(Check(TokenType.Identifiers))
         {
            VariableExpression variable = ParseVariable();
            Token token = Peek();
            Consume(TokenType.Colon,"Se esperaba ':'");
            Expression expression = Expression();
            AssignExpresion assign = new AssignExpresion(variable,token,expression);
            body.Add(assign);
            if(!Check(TokenType.RightBrace)) Consume(TokenType.Comma,"Se esperaba ','");
         }
         else
         {
            Error.Report(ErrorType.SyntaxError,"Se esperaba la implementacion del los elementos de una expresion PostAction");
         }
       }
       Consume(TokenType.RightBrace,"Se esperaba '}'");
       if(type == null || selector == null)
       {
          Error.Report(ErrorType.SyntaxError,"No se definio un type o un selector");
          return null;
       }
       return new PostActionExpression(type,selector);
    }
    private PredicateExpression ParsePredicate()//Parsea una expresion de Predicate
    {
        Consume(TokenType.LeftParenthesis,"Se esperaba '('");
        VariableExpression variable = ParseVariable();
        Consume(TokenType.RightParenthesis,"Se esperaba ')'");
        Consume(TokenType.Lambda,"Se esperaba '=>' ");
        Expression condition = Expression();
        return new PredicateExpression(variable,condition);
    }
    private ActionExpression ParseAction()//Parsea una expresion de Accion
    {
       Consume(TokenType.LeftParenthesis,"Se esperaba '('");
       VariableExpression targets = ParseVariable();
       Consume(TokenType.Comma,"Se esperaba ','");
       VariableExpression context = ParseVariable();
       Consume(TokenType.RightParenthesis,"Se esperaba '('");
       Consume(TokenType.Lambda,"Se esperaba '=>'");
       Consume(TokenType.LeftBrace,"Se esperaba '{'");
       StatementBlockExpression body = ParseStatementBlock();
       Consume(TokenType.RightBrace,"Se esperaba '}'");
       return new ActionExpression(targets,context,body);
    }
    private AssignExpresion ParseAssign(VariableExpression variable)//Parsea una expresion de asignacion
    {
       Token Operator = Advance();
       Expression expression = Expression();
       Consume(TokenType.SemiColon,"Se esperaba ';'");
       return new AssignExpresion(variable,Operator,expression);
    }
    private StatementBlockExpression ParseStatementBlock()//Parsea un bloque de instrucciones
    {
       StatementBlockExpression statementBlock = new StatementBlockExpression();
       while(!Check(TokenType.RightBrace) && !IsAtEnd())
       {
         statementBlock.expressions.Add(ParseStatement());
       }
       return statementBlock;
    }
    private StatementExpression ParseStatement()//Parsea una instruccion
    {
        if(Match(TokenType.For))
        {
            return ParseFor();
        }
        else if(Match(TokenType.While))
        {
            return ParseWhile();
        }
        else if(Match(TokenType.Identifiers))
        {
            VariableExpression variable = ParseVariable();
            if(variable.GetType() == typeof(VariableCompoundExpression) && Check(TokenType.SemiColon))
            {
                VariableCompoundExpression variableCompound = variable as VariableCompoundExpression;
                if(variableCompound.Argument.Params[variableCompound.Argument.Params.Count-1].GetType() == typeof(FunctionExpression))
                {
                    FunctionExpression function = variableCompound.Argument.Params[variableCompound.Argument.Params.Count-1] as FunctionExpression;
                    if(function.Type != VariableExpression.Type.VOID)
                    {
                        Error.Report(ErrorType.SyntaxError,"");
                        return null;
                    }
                }
                Consume(TokenType.SemiColon,"Se esperaba ';'");
                return variable as VariableCompoundExpression;
            }
            else
            {
                return ParseAssign(variable);
            }
        }
        else if(Match(TokenType.Function))
        {
            return ParseFunction(Previous().Value);
        }
        else
        {
            Error.Report(ErrorType.SyntaxError,"");
            return null;
        }

    }
    private StatementExpression ParseWhile()//Parsea ciclos while
    {
        Consume(TokenType.LeftParenthesis,"Se esperaba '('");
        Expression condition = Expression();
        Consume(TokenType.RightParenthesis,"Se esperaba ')'");
        StatementBlockExpression body = ParseStatementBlock();
        return new WhileExpression(condition,body);
    }
    private StatementExpression ParseFor()//Parsea ciclos for
    {
        VariableExpression target = ParseVariable();
        Consume(TokenType.In,"Se esperaba 'in'");
        VariableExpression targets = ParseVariable();
        Consume(TokenType.LeftBrace,"Se esperaba '{'");
        StatementBlockExpression body = ParseStatementBlock();
        Consume(TokenType.RightBrace,"Se esperaba '}'");
        Consume(TokenType.SemiColon,"Se esperaba ';'");
        return new ForExpression(target,targets,body);
    }
    private FunctionExpression ParseFunction(string value)//Parsea una Funcion
    {
        Consume(TokenType.LeftParenthesis,"Se esperaba '(' ");
        ParamsExpression Params = new ParamsExpression();
        while(!Check(TokenType.RightParenthesis) && !IsAtEnd())
        {
            if(Check(TokenType.Identifiers))
            {
                Params.Params.Add(ParseVariable());
            }
            else if (Check(TokenType.Function))
            {
                Params.Params.Add(ParseFunction(Advance().Value));
            }
            else
            {
                Params.Params.Add(Expression());
            }
            if(!Check(TokenType.RightParenthesis)) Consume(TokenType.Comma,"Se esperaba ','");
        }
        Consume(TokenType.RightParenthesis,"Se esperaba ')'");
        FunctionExpression function = new FunctionExpression(value,Params);
        return function;
    }
    private VariableExpression ParseVariable()//Parsea una Variable
    {
       VariableExpression variable = new VariableExpression(Advance());
       if(Check(TokenType.Dot))
       {
         VariableCompoundExpression variableCompound = new VariableCompoundExpression(variable.ID);
         while(Match(TokenType.Dot) && !IsAtEnd())
         {
            if(Match(TokenType.Function))
            {
                FunctionExpression function = ParseFunction(Previous().Value);
                variableCompound.Argument.Params.Add(function);
            }
            else
            {
                if(Match(TokenType.Type))
                {
                    TypeExpression type = new TypeExpression(new StringExpression(Previous().Value));
                    variableCompound.Argument.Params.Add(type);
                }
                else if(Match(TokenType.Name))
                {
                    NameExpression name = new NameExpression(new StringExpression(Previous().Value));
                    variableCompound.Argument.Params.Add(name);
                }
                else if(Match(TokenType.Faction))
                {
                    FactionExpression faction = new FactionExpression(new StringExpression(Previous().Value));
                    variableCompound.Argument.Params.Add(faction);
                }
                else if(Match(TokenType.Power))
                {
                    PowerExpression power = new PowerExpression(new NumberExpression(Convert.ToInt32(Previous().Value)));
                    variableCompound.Argument.Params.Add(power);
                }
                else if(Match(TokenType.Range))
                {
                    RangeExpression range = new RangeExpression(Previous().Value);
                    variableCompound.Argument.Params.Add(range);
                }
                else if(Match(TokenType.Pointer))
                {
                    PointerExpression pointer = new PointerExpression(Previous().Value);
                    variableCompound.Argument.Params.Add(pointer); 
                }
            }
         }
         variable = variableCompound;
       }
       return variable;
    }
    private Expression Expression()//Parsea una expresion
    {
        return Logical();
    }
    private Expression Logical()//Parsea una espresion logica(&&,||)
    {
       Expression expression = Equality();
       while(Match(TokenType.And,TokenType.Or))
       {
         Token Operator = Previous();
         Expression right = Equality();
         expression = new BinaryExpression(expression, Operator, right);
       }
       return expression;
    }
    private Expression Equality()//Parsea una expresion de igualda(==,!=)
    {
        Expression expression = Comparison();
        while(Match(TokenType.Equal,TokenType.NotEqual))
        {
            Token Operator = Previous();
            Expression right = Comparison();
            expression = new BinaryExpression(expression,Operator,right); 
        }
        return expression;
    }
    private Expression Comparison()//Parsea una expresion de Comparacion(>=,>,<,<=)
    {
        Expression expression = Concatenation();
        while(Match(TokenType.GreatEqualThan,TokenType.GreaterThan,TokenType.LessThan,TokenType.LessEqualThan))
        {
            Token Operator = Previous();
            Expression right = Concatenation();
            expression = new BinaryExpression(expression,Operator,right);  
        }
        return expression;
    }
    private Expression Concatenation()//Parsea una expresion de concatenacion
    {
        Expression expression = Term();
        while(Match(TokenType.Concatenation,TokenType.SpaceConcatenation))
        {
            Token Operator = Previous();
            Expression right = Concatenation();
            expression = new BinaryExpression(expression,Operator,right);
        }
        return expression;
    }
    private Expression Term()//Parsea una expression de un termino(+,-)
    {
         Expression expression = Factor();
         while(Match(TokenType.Plus,TokenType.Less))
         {
            Token Operator = Previous();
            Expression right = Factor();
            expression = new BinaryExpression(expression,Operator,right);
         }
         return expression;
    }
    private Expression Factor()//Parsea una expresion de un factor(*,/,%)
    {
       Expression expression = Pow();
       while(Match(TokenType.Divide,TokenType.Multiply,TokenType.Modulus))
       {
           Token Operator = Previous();
           Expression right = Pow();
           expression = new BinaryExpression(expression,Operator,right);
       }
       return expression;
    }
    private Expression Pow()//Parsea una expresion de potencia
    {
        Expression expression = Unary();
        if(Match(TokenType.Pow))
        {
            Token Operator = Previous();
            Expression right = Unary();
            return new BinaryExpression(expression,Operator,right); 
        }
        return expression;
    }
    private Expression Unary()//Parsea una expresion Unaria(!,-)
    {
        if(Match(TokenType.Less,TokenType.Not,TokenType.PlusPlus))
        {
            Token Operator = Previous();
            Expression right = Unary();
            return new UnaryExpression(Operator,right);
        }
        else if(Check(TokenType.Identifiers) && Continue(TokenType.PlusPlus))
        {
             Expression left = ParseVariable();
             Token token = Advance();
             return new UnaryExpression(token,left);
        }
        return Primary();
    }
    private Expression Primary()//Parsea una expresion primaria(Variables)
    {
        if(Match(TokenType.False)) return new BoolExpression(false);
        if(Match(TokenType.True)) return new BoolExpression(true);
        if(Match(TokenType.Numbers)) return new NumberExpression(Convert.ToInt32(Previous().Value));
        if(Match(TokenType.Strings)) return new StringExpression(Previous().Value);
        if(Match(TokenType.LeftParenthesis))
        {
            Expression expression = Expression();
            Consume(TokenType.RightParenthesis,"Se esperaba ')' despues de la expresion");
            return new GroupingExpresion(expression);
        }
        if(Check(TokenType.Identifiers)) return ParseVariable();
        Error.Report(ErrorType.SyntaxError,"Expresion inesperada");
        return null;
    }
    private bool Match(params TokenType[] types)
    {
        foreach(TokenType type in types)
        {
            if(Check(type))
            {
                Advance();
                return true;
            }
        }
        return false;
    }
    private bool Continue(TokenType type)
    {
       if(IsAtEnd()) return false;
       return Tokens[CurrentPosition + 1].Type == type;
    }
    private bool Check(TokenType type)
    {
        if(IsAtEnd()) return false;
        return Peek().Type == type;
    }
    private bool IsAtEnd()
    {
        return Peek().Type == TokenType.EOF;
    }
    private Token Advance()
    {
       if(!IsAtEnd()) CurrentPosition++;
       return Previous();   
    }
    private Token Peek()
    {
        return Tokens[CurrentPosition];
    }
    private Token Previous()
    {
        return Tokens[CurrentPosition - 1];
    }
    private Token Consume(TokenType type,string message)
    {
       if(Check(type)) return Advance();
       Error.Report(ErrorType.SyntaxError,message);
       return null;
    }
}