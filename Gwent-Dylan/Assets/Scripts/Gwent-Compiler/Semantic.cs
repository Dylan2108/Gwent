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
    public TMP_InputField errorText;
    public void ShowError(string error)
    {
       errorText.text = error;
    }
    public VariableExpression GetVariable(string name)//Verifica si ya una variable ya fue definida
    {
        if(!variables.TryGetValue(name,out var variable))
        {
            string error = $"Error Semantico. La variable {name} no esta definida";
            ShowError(error);
            throw new Error($"La variable {name} no esta definida",ErrorType.SemanticError);
        }
        return variable;
    }
    public void DefineEffect(EffectExpression effect)//Define un efecto
    {
        if(effects.ContainsKey(effect.Name.Name))
        {
            string error = $"Error Semantico. Ya el efecto {effect.Name} fue definido";
            ShowError(error);
            throw new Error($"Ya el efecto {effect.Name} fue definido",ErrorType.SemanticError);
        }
        effects[effect.Name.Name] = effect;
    }
    public EffectExpression GetEffect(Expression name)//Verifica si ya un efecto fue definido
    {
       if(!effects.TryGetValue(name, out var effect))
       {
          string error = $"Error Semantico. El efecto {effect.Name} no esta definido";
          ShowError(error);
          throw new Error($"El efecto {effect.Name} no esta definido",ErrorType.SemanticError);
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
                        string Errors = $"Error Semantico. La variable {variableValue.Name} no esta definida en los parametros del efecto";
                        ShowError(Errors);
                        throw new Error($"La variable {variableValue.Name} no esta definida en los parametros del efecto",ErrorType.SemanticError);
                    }
                  }
                  else if(!(assign.Value is NumberExpression))
                  {
                     string errorss = $"Error Semantico. La propiedad Power debe contener un valor de tipo Number";
                     ShowError(errorss);
                     throw new Error($"La propiedad Power debe contener un valor de tipo Number",ErrorType.SemanticError);
                  }
                  break;
                  case "type":
                  case "name":
                  case "faction":
                  if(assign.Value is VariableExpression variableVal)
                  {
                    if(!IsVariableDeclaredInParams(variableVal.Name))
                    {
                        string errors = $"Error Semantico. La variable {variableVal.Name} no esta definida en los parametros";
                        ShowError(errors);
                        throw new Error($"La variable {variableVal.Name} no esta definida en los parametros",ErrorType.SemanticError);
                    }
                  }
                  else if(!(assign.Value is StringExpression))
                  {
                    string Error = $"Error Semantico. La propiedad Name debe contener un valor de tipo String";
                    ShowError(Error);
                    throw new Error($"La propiedad Name debe contener un valor de tipo String",ErrorType.SemanticError);
                  }
                  break;
                  default:
                  string error = $"Error Semantico. La asignacion a la propiedad {propertyName} no permitida";
                  ShowError(error);
                  throw new Error($"La asignacion a la propiedad {propertyName} no permitida",ErrorType.SemanticError);
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
            string error = $"Error Semantico. La variable {variable.Name} ya esta definida";
            ShowError(error);
            throw new Error($"La variable {variable.Name} ya esta definida",ErrorType.SemanticError);
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
                string error = $"Error Semantico. Se esta accediendo a la propiedad {propertyName} sin asignarle un valor";
                ShowError(error);
                throw new Error($"Se esta accediendo a la propiedad {propertyName} sin asignarle un valor",ErrorType.SemanticError);
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
                string error = $"Error Semantico. La variable {variable.Name} no esta declarada";
                ShowError(error);
                throw new Error($"La variable {variable.Name} no esta declarada",ErrorType.SemanticError);
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
                        string error = $"Error Semantico. El efecto {effectName} mencionado en la carta {cardName} no fue definido";
                        ShowError(error);
                        throw new Error($"El efecto {effectName} mencionado en la carta {cardName} no fue definido",ErrorType.SemanticError);
                    }
                    EffectExpression declaredEffect = effectsName[effectName];
                    foreach(var param in elemento.EffectCall.Params)
                    {
                        var declaredParam = declaredEffect.Params.Params.FirstOrDefault(p=>(p as VariableExpression)?.Name==param.Variable.Name) as VariableExpression;
                        if(declaredParam == null)
                        {
                            string error = $"Error Semantico. El parametro {param.Variable.Name} no esta definido en el efecto {effectName}";
                            ShowError(error);
                            throw new Error($"El parametro {param.Variable.Name} no esta definido en el efecto {effectName}",ErrorType.SemanticError);
                        }
                        if(param.Value is BoolExpression && declaredParam.type!=VariableExpression.Type.BOOL)
                        {
                            string error = $"Error Semantico. El parametro {param.Variable.Name} se le asigno un valor incorrecto";
                            ShowError(error);
                            throw new Error($"El parametro {param.Variable.Name} se le asigno un valor incorrecto",ErrorType.SemanticError);
                        }
                        else if(param.Value is StringExpression && declaredParam.type!=VariableExpression.Type.STRING)
                        {
                            string error = $"Error Semantico. El parametro {param.Variable.Name} se le asigno un valor incorrecto";
                            ShowError(error);
                            throw new Error($"El parametro {param.Variable.Name} se le asigno un valor incorrecto",ErrorType.SemanticError);
                        }
                        else if(param.Value is NumberExpression && declaredParam.type!=VariableExpression.Type.INT)
                        {
                            string error = $"Error Semantico. El parametro {param.Variable.Name} se le asigno un valor incorrecto";
                            ShowError(error);
                            throw new Error($"El parametro {param.Variable.Name} se le asigno un valor incorrecto",ErrorType.SemanticError);
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
                            string error = $"Error Semantico. Tipo de PostAction invalido en la carta {cardName}, se experaba una expresion de cadena o una variable";
                            ShowError(error);
                            throw new Error($"Tipo de PostAction invalido en la carta {cardName}, se experaba una expresion de cadena o una variable",ErrorType.SemanticError);
                        }
                        if(!effectsName.ContainsKey(postActionEffectName))
                        {
                            string error = $"Error Semantico. El efecto {postActionEffectName} mencionado en el PostAction de la carta {cardName} no esta definido";
                            ShowError(error);
                            throw new Error($"El efecto {postActionEffectName} mencionado en el PostAction de la carta {cardName} no esta definido",ErrorType.SemanticError);
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
                if(effectsName.ContainsKey(effectName))
                {
                    string error = $"El efecto {effectName} ya esta definido";
                    ShowError(error);
                    throw new Error($"El efecto {effectName} ya esta definido",ErrorType.SemanticError);
                }
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
        if(effects.ContainsKey(effect.Name.Name))
        {
            string error = $"Error Semantico. El efecto {effect.Name} ya esta definido";
            ShowError(error);
            throw new Error($"El efecto {effect.Name} ya esta definido",ErrorType.SemanticError);
        }
    }
}