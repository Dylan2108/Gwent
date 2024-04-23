using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public Deck MyDeck;
   public Deck RivalDeck;
   public GameObject PrefabLeaderCard;
   public GameObject MyLeaderZone;
   public GameObject RivalLeaderZone;
   public Player MyPlayer;
   public Player RivalPlayer;
   public Melee MyMeleeZone;
   public Melee RivalMeleeZone;
   public FromDistance MyFromDistanceZone;
   public FromDistance RivalFromDistanceZone;
   public Siege MySiegeZone;
   public Siege RivalSiegeZone;
   public Graveyard MyGraveyard;
   public Graveyard RivalGraveyard;
   public int myPoints;
   public int rivalPoints;
   public int myRounds;
   public int rivalRounds;
   void Start()
   {
    Debug.Log("Ronda 1");
    MyDeck.InvokeLeaderCard(PrefabLeaderCard,MyLeaderZone);//Se invocan las cartas lideres
    RivalDeck.InvokeLeaderCard(PrefabLeaderCard,RivalLeaderZone);
    for (int i = 0; i < 10; i++)
    {//Se roban las cartas iniciales
      MyDeck.DrawCard();
      RivalDeck.DrawCard();
    }
    MyPlayer.isMyTurn = true;
    Debug.Log("Es el turno del jugador 1");
   }
   void Update()
   {
    myPoints = MyPlayer.CountTotalPoints();
    rivalPoints = RivalPlayer.CountTotalPoints();
    if(Player.round == 1)
    {
      if(MyPlayer.IPass && RivalPlayer.IPass)
      {
        DecideWinner();
        StartRound();
        Debug.Log("Ronda 2");
      }
    }
    else if(Player.round == 2)
    {
       if(MyPlayer.IPass && RivalPlayer.IPass)
       {
        DecideWinner();
        if(MyPlayer.RoundsWin == 2) SceneManager.LoadScene("Player 1 Win");
        else if(RivalPlayer.RoundsWin == 2) SceneManager.LoadScene("Player 2 Win");
        StartRound();
        Debug.Log("Ronda 3");
       }
    }
    else if(Player.round == 3)
    {
      if(MyPlayer.IPass && RivalPlayer.IPass)
      {
         DecideWinner();
         if(MyPlayer.RoundsWin > RivalPlayer.RoundsWin) SceneManager.LoadScene("Player 1 Win");
         else if(MyPlayer.RoundsWin < RivalPlayer.RoundsWin) SceneManager.LoadScene("Player 2 Win");
         else if(MyPlayer.RoundsWin == RivalPlayer.RoundsWin) SceneManager.LoadScene("Tie");
      }
    }
   }
   public void MyChangeTurn()
    {
      if(MyPlayer.playedCards != 0 && !RivalPlayer.IPass)
      {
        MyPlayer.isMyTurn = false;
        RivalPlayer.isMyTurn = true;
        RivalPlayer.playedCards = 0;
        Debug.Log("Es el turno del jugador 2");
      }
      else if(RivalPlayer.IPass)
      {
        Debug.Log("No puede pasar turno, ya el jugador contrario termino su ronda");
      }
      else
      {
        Debug.Log("Debe jugar al menos una carta");
      }
    }
    
    public void RivalChangeTurn()
    {
      if(RivalPlayer.playedCards != 0 && !MyPlayer.IPass)
      {
       MyPlayer.isMyTurn = true;
       RivalPlayer.isMyTurn = false;
       MyPlayer.playedCards = 0;
       Debug.Log("Es el turno del jugador 1");
      }
      else if(MyPlayer.IPass)
      {
        Debug.Log("No puede pasar el turno ,ya el jugador contrario termino su ronda");
      }
      else
      {
        Debug.Log("Debe jugar al menos una carta");
      }
    }
    public void MyPassRound()
    {
      if(MyPlayer.playedCards == 0 || MyPlayer.cardsInHand.Count==0 || RivalPlayer.IPass)
      {
        Debug.Log("Jugador 1 cedio su turno");
        MyPlayer.IPass = true;
        MyPlayer.isMyTurn = false;
        RivalPlayer.isMyTurn = true;
        RivalPlayer.playedCards = 0;
        RivalPlayer.ICanStillSummoning = true;
      }
      else
      {
        Debug.Log("No puede pasar de ronda en este turno");
      }
    }
    public void RivalPassRound()
    {
      if(RivalPlayer.playedCards == 0 || RivalPlayer.cardsInHand.Count==0 || MyPlayer.IPass)
      {
        Debug.Log("Jugador 2 cedio su turno");
        RivalPlayer.IPass = true;
        MyPlayer.isMyTurn = true;
        RivalPlayer.isMyTurn = false;
        MyPlayer.playedCards = 0;
        MyPlayer.ICanStillSummoning = true;
      }
      else
      {
        Debug.Log("No puede pasar de ronda en este turno");
      }
    }
    public void DecideWinner()
    {
       Debug.Log("Se acabo la ronda");
        MyPlayer.IPass = false;
        RivalPlayer.IPass = false;
        MyPlayer.playedCards = 0;
        RivalPlayer.playedCards = 0;
        MyPlayer.ICanStillSummoning = false;
        RivalPlayer.ICanStillSummoning = false;
        Debug.Log("Calculemos los puntos para ver al ganador");
        if(myPoints>rivalPoints)
        {
          Debug.Log("La Ronda fue ganada por el Jugador 1");
          MyPlayer.RoundsWin++;
          Player.round++;
          MyPlayer.isMyTurn = false;
          RivalPlayer.isMyTurn = true;
        }
        else if(myPoints<rivalPoints)
        {
          Debug.Log("La Ronda fue ganada por el Jugador 2");
          RivalPlayer.RoundsWin++;
          Player.round++;
          MyPlayer.isMyTurn = true;
          RivalPlayer.isMyTurn = false;
        }
        else if(myPoints == rivalPoints)
        {
          Debug.Log("La Ronda termino en empate");
          Player.round++;
        }
    }
    public void StartRound()
    {
        MyPlayer.CleanBoard();
        RivalPlayer.CleanBoard();
        MyPlayer.UpdateScore();
        RivalPlayer.UpdateScore();
        MyPlayer.UpdateRoundScore();
        RivalPlayer.UpdateRoundScore();
        for(int i =0;i<3;i++)
        {
          MyDeck.DrawCard();
          RivalDeck.DrawCard();
        }
        if(MyPlayer.isMyTurn && !RivalPlayer.isMyTurn) Debug.Log("Es el turno del jugador 1");
        if(!MyPlayer.isMyTurn && RivalPlayer.isMyTurn) Debug.Log("Es el turno del jugador 2"); 
    }
}