using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
public class CompileCards : MonoBehaviour
{
    public TMP_InputField inputText;
    public Scope scope;
    public GameObject PrefabCreatedCard;
    public GameManager gameManager;
    public TMP_InputField errorText;
    public GameObject MainCanvas;
    public GameObject Board;
    public GameObject CompileCanvas;
    public void ShowError(string error)
    {
       errorText.text = error;
    }
    public void PlayGame()
    {
       CompileCanvas.SetActive(false);
       MainCanvas.SetActive(true);
       Board.SetActive(true);
    }
    public void Compile()
    {
         string input = inputText.text;
         ProcessText(input);
    }
    public void ProcessText(string input)
    {
         Lexer lexer = new Lexer(input);
         lexer.errorText = errorText;
         List<Token> tokens = lexer.GetTokens();
         Parser parser = new Parser(tokens);
         parser.errorText = errorText;
         Node AST = parser.ParseProgram();
         Semantic semantic = new Semantic();
         semantic.errorText = errorText;
         semantic.CheckProgram(AST);
         if(AST is ProgramExpression program)
         {
            foreach(EffectExpression effect in program.CompiledEffects)
            {
               scope.PushEffect(effect.Name.Name.Evaluate(scope).ToString(),effect);
            }
            foreach(CardExpression card in program.CompiledCards)
            {
               string type = card.Type.Type.Evaluate(scope).ToString();
               switch(type)
               {
                    case @"""Silver""":
                    PrefabCreatedCard.AddComponent<Silver>(); 
                    Silver SilverCard = PrefabCreatedCard.GetComponent<Silver>();
                    SilverCard.Type = "Silver";
                    SilverCard.name = card.Name.Name.Evaluate(scope).ToString();
                    SilverCard.faction = card.Faction.Faction.Evaluate(scope).ToString();
                    SilverCard.power = Convert.ToInt32(card.Power.Power.Evaluate(scope));
                    if(SilverCard.power < 0)
                    {
                         string error5 = "Error Semantico. No se puede asignar un poder negativo";
                         ShowError(error5);
                         throw new Error("No se puede asignar un poder negativo",ErrorType.SemanticError);
                    }
                    int j = 0;
                    foreach(var range in card.Range.Ranges)
                    {
                      SilverCard.range[j++] = range.Evaluate(scope) as string;
                    }
                     SilverCard.atkType = SilverCard.range[0];
                     //Falta el owner
                     SilverCard.invoked = false;
                     SilverCard.destroyed = false;
                     SilverCard.EffectActivated = false;
                     SilverCard.onActivation = card.OnActivation;
                     gameManager.MyDeck.deck.Add(SilverCard.gameObject);
                     gameManager.RivalDeck.deck.Add(SilverCard.gameObject);
                    break;
                    case @"""Gold""":
                    PrefabCreatedCard.AddComponent<Gold>();
                    Gold GoldCard = PrefabCreatedCard.GetComponent<Gold>();
                    GoldCard.Type = "Gold";
                    GoldCard.name = card.Name.Name.Evaluate(scope).ToString();
                    GoldCard.faction = card.Faction.Faction.Evaluate(scope).ToString();
                    GoldCard.power = Convert.ToInt32(card.Power.Power.Evaluate(scope));
                    if(GoldCard.power < 0)
                    {
                         string error4 = "Error Semantico. No se puede asignar un poder negativo";
                         ShowError(error4);
                         throw new Error("No se puede asignar un poder negativo",ErrorType.SemanticError);
                    }
                    int k = 0;
                    foreach(var range in card.Range.Ranges)
                    {
                         GoldCard.range[k++] = range.Evaluate(scope) as string; 
                    }
                    GoldCard.atkType = GoldCard.range[0];
                    //Falta el owner
                    GoldCard.invoked = false;
                    GoldCard.destroyed = false;
                    GoldCard.EffectActivated = false;
                    GoldCard.onActivation = card.OnActivation;
                    gameManager.MyDeck.deck.Add(GoldCard.gameObject);
                    gameManager.RivalDeck.deck.Add(GoldCard.gameObject);
                    break;
                    case @"""Weather""":
                    PrefabCreatedCard.AddComponent<Weather>();
                    Weather WeatherCard = PrefabCreatedCard.GetComponent<Weather>();
                    WeatherCard.Type = "Weather";
                    WeatherCard.name = card.Name.Name.Evaluate(scope).ToString();
                    WeatherCard.faction = card.Faction.Faction.Evaluate(scope).ToString();
                    WeatherCard.power = Convert.ToInt32(card.Power.Power.Evaluate(scope));
                    if(WeatherCard.power!=0)
                    {
                         string error3 = "Error Semantico. No se puede asignar un poder distinto de 0";
                         ShowError(error3);
                         throw new Error("No se puede asignar un poder distinto de 0",ErrorType.SemanticError);
                    }
                    //Falta el owner
                    WeatherCard.invoked = false;
                    WeatherCard.destroyed = false;
                    WeatherCard.onActivation = card.OnActivation;
                    gameManager.MyDeck.deck.Add(WeatherCard.gameObject);
                    gameManager.RivalDeck.deck.Add(WeatherCard.gameObject);
                    break;
                    case @"""Boost""":
                    PrefabCreatedCard.AddComponent<Boost>();
                    Boost BoostCard = PrefabCreatedCard.GetComponent<Boost>();
                    BoostCard.Type = "Boost";
                    BoostCard.name = card.Name.Name.Evaluate(scope).ToString();
                    BoostCard.faction = card.Faction.Faction.Evaluate(scope).ToString();
                    BoostCard.power = Convert.ToInt32(card.Power.Power.Evaluate(scope));
                    if(BoostCard.power!=0)
                    {
                         string error2 = "Error Semantico. No se puede asignar un poder distinto de 0";
                         ShowError(error2);
                         throw new Error("No se puede asignar un poder distinto de 0",ErrorType.SemanticError);
                    }
                    //Falta el owner
                    BoostCard.invoked = false;
                    BoostCard.destroyed = false;
                    BoostCard.onActivation = card.OnActivation;
                    gameManager.MyDeck.deck.Add(BoostCard.gameObject);
                    gameManager.RivalDeck.deck.Add(BoostCard.gameObject);
                    break;
                    case @"""Clear""":
                    PrefabCreatedCard.AddComponent<Clear>();
                    Clear ClearCard = PrefabCreatedCard.GetComponent<Clear>();
                    ClearCard.Type = "Clear";
                    ClearCard.name = card.Name.Name.Evaluate(scope).ToString();
                    ClearCard.faction = card.Faction.Faction.Evaluate(scope).ToString();
                    ClearCard.power = Convert.ToInt32(card.Power.Power.Evaluate(scope));
                    if(ClearCard.power!=0)
                    {
                         string Errors = "Error Semantico. No se puede asignar un poder distinto de 0";
                         ShowError(Errors);
                         throw new Error("No se puede asignar un poder distinto de 0",ErrorType.SemanticError);
                    }
                    //Falta el owner
                    ClearCard.invoked = false;
                    ClearCard.destroyed = false;
                    ClearCard.onActivation = card.OnActivation;
                    gameManager.MyDeck.deck.Add(ClearCard.gameObject);
                    gameManager.RivalDeck.deck.Add(ClearCard.gameObject);
                    break;
                    case @"""Lure""":
                    PrefabCreatedCard.AddComponent<Lure>();
                    Lure LureCard = PrefabCreatedCard.GetComponent<Lure>();
                    LureCard.Type = "Lure";
                    LureCard.name = card.Name.Name.Evaluate(scope).ToString();
                    LureCard.faction = card.Faction.Faction.Evaluate(scope).ToString();
                    LureCard.power = Convert.ToInt32(card.Power.Power.Evaluate(scope));
                    if(LureCard.power!=0)
                    {
                         string Error = "Error Semantico. No se puede asignar un poder distinto de 0";
                         ShowError(Error);
                         throw new Error("No se puede asignar un poder distinto de 0",ErrorType.SemanticError);
                    }
                    //Falta el owner
                    LureCard.invoked = false;
                    LureCard.destroyed = false;
                    LureCard.onActivation = card.OnActivation;
                    gameManager.MyDeck.deck.Add(LureCard.gameObject);
                    gameManager.RivalDeck.deck.Add(LureCard.gameObject);
                    break;
                    case @"""Leader""":
                    PrefabCreatedCard.AddComponent<Leader>();
                    Leader LeaderCard = PrefabCreatedCard.GetComponent<Leader>();
                    LeaderCard.Type = "Leader";
                    LeaderCard.name = card.Name.Name.Evaluate(scope).ToString();
                    LeaderCard.faction = card.Faction.Faction.Evaluate(scope).ToString();
                    LeaderCard.power = Convert.ToInt32(card.Power.Power.Evaluate(scope));
                    if(LeaderCard.power!=0)
                    {
                         string errors = "Error Semantico. No se puede asignar un poder distinto de 0";
                         ShowError(errors);
                         throw new Error("No se puede asignar un poder distinto de 0",ErrorType.SemanticError);
                    }
                    //Falta el owner
                    LeaderCard.EffectActivated = false;
                    LeaderCard.onActivation = card.OnActivation;
                    gameManager.MyDeck.deck.Add(LeaderCard.gameObject);
                    gameManager.RivalDeck.deck.Add(LeaderCard.gameObject);
                    break;
                    default:
                    string error = $"Error Semantico. El tipo de carta declarado no existe";
                    ShowError(error);
                    throw new Error("El tipo de carta declarado no existe",ErrorType.SemanticError);
               }
            }
         }
    }
}