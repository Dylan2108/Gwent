using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class Semantic : MonoBehaviour
{
    private Dictionary<string,VariableExpression> variables = new Dictionary<string, VariableExpression>();//Las variables definidas
    private Stack<Dictionary<string,VariableExpression>> scopes = new Stack<Dictionary<string,VariableExpression>>();//Los scopes
    private Dictionary<Expression,EffectExpression> effects = new Dictionary<Expression, EffectExpression>();//Los efectos definidos
    private Dictionary<string,EffectExpression> effectsName = new Dictionary<string, EffectExpression>();//Los nombres de los efectos
    private Dictionary<string,CardExpression> cardsName = new Dictionary<string, CardExpression>();//Los nombres de las cartas
    private  EffectExpression currentEffect;//El efecto actual que se esta chequeando
    public VariableExpression GetVariable(string name)//Verifica si ya una variable ya fue definida
    {
        if(!variables.TryGetValue(name,out var variable))
        {
            Error.Report(ErrorType.SemanticError,$"La variable {name} no esta definida");
            return null;
        }
        return variable;
    }
    public void DefineEffect(EffectExpression effect)//Define un efecto
    {
        if(effects.ContainsKey(effect.Name.Name))
        {
            Error.Report(ErrorType.SemanticError,$"Ya el efecto {effect.Name} fue definido");
            return;
        }
        effects[effect.Name.Name] = effect;
    }
    public EffectExpression GetEffect(Expression name)//Verifica si ya un efecto fue definido
    {
       if(!effects.TryGetValue(name, out var effect))
       {
          Error.Report(ErrorType.SemanticError,$"El efecto {effect.Name} no esta definido");
          return null;
       }
       return effect;
    }
    public void CheckEffect(EffectExpression effect)//Chequea la definicion del efecto
    {
       currentEffect = effect;
       PushScope();
       if(effect.Params != null)
       {
          foreach(var param in effect.Params.Params)
          {
            if(param is VariableExpression variable)
            {
                DefineVariable(variable);
            }
          }
       }
       CheckAction(effect.Action);
       PopScope();
       currentEffect = null;
    }
    private void PushScope()//Agrega el diccionario a la pila
    {
        scopes.Push(new Dictionary<string,VariableExpression>());
    }
    private void PopScope()//Elimina el diccionario de la pila 
    {
        scopes.Pop();
    }
    public void DefineVariable(VariableExpression variable,bool IsConstant = false)//Define una variable
    {
        if(scopes.Count == 0)
        {
            PushScope();
        }
        variable.IsConstant = IsConstant;
        scopes.Peek()[variable.Name] = variable;
    }
    private void CheckAction(ActionExpression action)//Chequea la definicion de Action
    {
       PushScope();
       DefineVariable(new VariableExpression(new Token("targets",TokenType.Identifiers,"targets",0)));
       DefineVariable(new VariableExpression(new Token("context",TokenType.Identifiers,"context",0)));
       foreach(var statement in action.Body.expressions)
       {
         if(statement is AssignExpresion assign)
         {
            CheckAssign(assign);
         }
         else if(statement is ForExpression forExpression)
         {
            CheckFor(forExpression);
         }
       }
       PopScope();
    }
    private void CheckAssign(AssignExpresion assign)//Chequea la definicion de Asignamiento
    {
        if(assign.Variable is VariableCompoundExpression variableCompound)
        {
             string propertyName = variableCompound.Argument.Params.Last().ToString().ToLower();
             switch(propertyName)
             {
                case "power":
                case "pow":
                  if(assign.Value is VariableExpression variableValue)
                  {
                    if(!IsVariableDeclaredInParams(variableValue.Name))
                    {
                        Error.Report(ErrorType.SemanticError,$"La variable {variableValue.Name} no esta definida en los parametros del efecto");
                    }
                  }
                  else if(!(assign.Value is NumberExpression))
                  {
                     Error.Report(ErrorType.SemanticError,$"La propiedad Power debe contener un valor de tipo Number");
                  }
                  break;
                  case "type":
                  case "name":
                  case "faction":
                  if(assign.Value is VariableExpression variableVal)
                  {
                    if(!IsVariableDeclaredInParams(variableVal.Name))
                    {
                        Error.Report(ErrorType.SemanticError,$"La variable {variableVal.Name} no esta definida en los parametros");
                    }
                  }
                  else if(!(assign.Value is StringExpression))
                  {
                    Error.Report(ErrorType.SemanticError,$"La propiedad Name debe contener un valor de tipo String");
                  }
                  break;
                  default:
                  Error.Report(ErrorType.SemanticError,$"La asignacion a la propiedad {propertyName} no permitida");
                  break;
             }
        }
    }
    private void CheckFor(ForExpression @for)//Chequea una expresion for
    {
       PushScope();
       DefineVariable(@for.Variable);
       CheckVariable(@for.Target);
       CheckStatementBlock(@for.Body);
       PopScope();
    }
    private bool IsVariableDeclaredInParams(string variableName)//Verifica si una variable ya fue definida en los parametros
    {
        if(currentEffect == null || currentEffect.Params == null) return false;
        return currentEffect.Params.Params.Any(param => param is VariableExpression variable && variable.Name == variableName);
    }
    private void CheckVariable(VariableExpression variable)//Chequea una expresion de una variable
    {
        if(!IsVariableDeclared(variable.Name))
        {
            Error.Report(ErrorType.SemanticError,$"La variable {variable.Name} ya esta definida");
        }
        if(variable is VariableCompoundExpression variableCompound)
        {
            foreach(var param in variableCompound.Argument.Params)
            {
                if(param is FunctionExpression function)
                {
                    CheckFunction(function);
                }
                else if(param is Expression expression)
                {
                    CheckExpression(expression);
                }
            }
        }
    }
    private void CheckStatementBlock(StatementBlockExpression block)//Chequea un bloque de instrucciones
    {
       foreach(var statement in block.expressions)
       {
           if(statement is AssignExpresion assign)
           {
              CheckAssign(assign);
           }
           else if(statement is ForExpression @for)
           {
             CheckFor(@for);
           }
           else if(statement is VariableCompoundExpression variableCompound)
           {
              string propertyName = variableCompound.Argument.Params.Last().ToString().ToLower();
              if(propertyName=="power"||propertyName=="pow"||propertyName=="name"||propertyName=="type"||propertyName=="faction"||propertyName=="range")
              {
                Error.Report(ErrorType.SemanticError,$"Se esta accediendo a la propiedad {propertyName} sin asignarle un valor");
              }
           }
       } 
    }
    private bool IsVariableDeclared(string name)//Verifica si una variable ya fue declarada
    {
        foreach(var scope in scopes)
        {
            if(scope.ContainsKey(name))
            {
                return true;
            }
        }
        return false;
    }
    private void CheckFunction(FunctionExpression function)//Chequea una funcion
    {
       foreach(var param in function.ParamsExpression.Params)
       {
           if(param is Expression expression)
           {
              CheckExpression(expression);
           }
           else if(param is VariableExpression variable)
           {
              CheckVariable(variable);
           }
       }
    }
    public void CheckExpression(Expression expression)//Chequea una expresion
    {
        if(expression is VariableExpression variable)
        {
            if(!IsVariableDeclared(variable.Name))
            {
                Error.Report(ErrorType.SemanticError,$"La variable {variable.Name} no esta declarada");
            }
        }
        else if(expression is BinaryExpression binary)
        {
            CheckExpression(binary.Left);
            CheckExpression(binary.Right);
        }
    }
    public void CheckDeclaration(VariableExpression variable,Expression value)//Chequea una declaracion
    {
       DefineVariable(variable);
       CheckExpression(value);
    }
    public void CheckCard(CardExpression card)//Chequea una carta
    {
         string cardName = ((StringExpression)card.Name.Name).Value;
         if(card.OnActivation != null)
         {
            foreach(var elemento in card.OnActivation.OnActivation)
            {
                if(elemento.EffectCall != null)
                {
                    string effectName = elemento.EffectCall.Name;
                    if(!effectsName.ContainsKey(effectName))
                    {
                        Error.Report(ErrorType.SemanticError,$"El efecto {effectName} mencionado en la carta {cardName} no fue definido");
                    }
                    EffectExpression declaredEffect = effectsName[effectName];
                    foreach(var param in elemento.EffectCall.Params)
                    {
                        var declaredParam = declaredEffect.Params.Params.FirstOrDefault(p=>(p as VariableExpression)?.Name==param.Variable.Name) as VariableExpression;
                        if(declaredParam == null)
                        {
                            Error.Report(ErrorType.SemanticError,$"El parametro {param.Variable.Name} no esta definido en el efecto {effectName}");
                        }
                        if(param.Value is BoolExpression && declaredParam.type!=VariableExpression.Type.BOOL)
                        {
                            Error.Report(ErrorType.SemanticError,$"El parametro {param.Variable.Name} se le asigno un valor incorrecto");
                        }
                        else if(param.Value is StringExpression && declaredParam.type!=VariableExpression.Type.STRING)
                        {
                            Error.Report(ErrorType.SemanticError,$"El parametro {param.Variable.Name} se le asigno un valor incorrecto");
                        }
                        else if(param.Value is NumberExpression && declaredParam.type!=VariableExpression.Type.INT)
                        {
                            Error.Report(ErrorType.SemanticError,$"El parametro {param.Variable.Name} se le asigno un valor incorrecto");
                        }
                    }
                }
                else if(elemento.PostAction != null)
                {
                    foreach(var postAction in elemento.PostAction)
                    {
                        string postActionEffectName = null;
                        if(postAction.Type is Expression stringExpr)
                        {
                            postActionEffectName = stringExpr.ToString();
                        }
                        else if(postAction.Type is StringExpression stringExpression)
                        {
                            postActionEffectName = stringExpression.Evaluate(new Scope()).ToString();
                        }
                        else if(postAction.Type is VariableExpression variable)
                        {
                            postActionEffectName = variable.Name;
                        }
                        else
                        {
                            Error.Report(ErrorType.SemanticError,$"Tipo de PostAction invalido en la carta {cardName}, se experaba una expresion de cadena o una variable");
                        }
                        if(!effectsName.ContainsKey(postActionEffectName))
                        {
                            Error.Report(ErrorType.SemanticError,$"El efecto {postActionEffectName} mencionado en el PostAction de la carta {cardName} no esta definido");
                        }
                    }
                }
            }
         }
    }
    private bool IsAssignPower(AssignExpresion assign)//Verifica si una asignacion es de la propiedad Power
    {
       if(assign.Variable is VariableCompoundExpression variableCompound)
       {
          string propertyName = variableCompound.Argument.Params.Last().ToString().ToLower();
          return propertyName == "power";
       }
       return false;
    }
    public void CheckProgram(Node ast)//Chequea el programa
    {
        if(ast is ProgramExpression program)
        {
            foreach(EffectExpression effect in program.CompiledEffects)
            {
                string effectName = ((StringExpression)effect.Name.Name).Value;
                if(effectsName.ContainsKey(effectName)) Error.Report(ErrorType.SemanticError,$"El efecto {effectName} ya esta definido");
                effectsName[effectName] = effect;
                CheckEffect(effect);
                Debug.Log("Analisis semantico del efecto completado");
            }
            foreach(CardExpression card in program.CompiledCards)
            {
                string cardName = ((StringExpression)card.Name.Name).Value;
                cardsName[cardName] = card;
                CheckCard(card);
                Debug.Log("Analisis semantico de la carta completado");
            }
            if(program.CompiledEffects.Count == 0 && program.CompiledCards.Count == 0) Debug.Log("El programa analizado no contiene cartas ni efectos");
        }
        else
        {
            Debug.Log("El ast analizado no es valido. Se omitira el analisis semantico");
        }
    }
    private bool IsNumberType(Expression expression)//Verifica si una variable es de tipo Number
    {
       if(expression is VariableExpression variable) return variable.type == VariableExpression.Type.INT || variable.type == VariableExpression.Type.NULL;
       return false; 
    }
    private bool IsStringType(Expression expression)//Verifica si una variable es de tipo String
    {
        return expression is VariableExpression variable && variable.type == VariableExpression.Type.STRING;
    }
    private void CheckStatement(StatementExpression statementExpression)//Chequea una instruccion
    {
        if(statementExpression is AssignExpresion assign)
        {
              if(!IsVariableDeclared(assign.Variable.Name))
              {
                CheckDeclaration(assign.Variable,assign.Value);
              }
              else
              {
                CheckAssign(assign);
              }
        }
        else if(statementExpression is ForExpression @for)
        {
            PushScope();
            DefineVariable(@for.Variable);
            DefineVariable(@for.Target);
            CheckStatementBlock(@for.Body);
            PopScope();
        }
    }
    public void CheckEffectCall(EffectExpression effect)
    {
        if(effects.ContainsKey(effect.Name.Name))Error.Report(ErrorType.SemanticError,$"El efecto {effect.Name} ya esta definido");
    }
}