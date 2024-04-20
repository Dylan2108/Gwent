using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public Deck MyDeck;
   public Deck RivalDeck;
   public GameObject PrefabLeaderCard;
   public GameObject MyLeaderZone;
   public GameObject RivalLeaderZone;
   public Hand MyHand;
   public Hand RivalHand;
   void Start()
   {
    Debug.Log("Ronda 1");
    MyDeck.InvokeLeaderCard(PrefabLeaderCard,MyLeaderZone);//Se invocan las cartas lideres
    RivalDeck.InvokeLeaderCard(PrefabLeaderCard,RivalLeaderZone);
    for (int i = 0; i < 10; i++)
    {//Se roban las cartas iniciales
      MyDeck.DrawCard(MyHand);
      RivalDeck.DrawCard(RivalHand);
    }
    MyHand.isMyTurn = true;
       Debug.Log("Es el turno del jugador 1");
   }
   public void MyChangeTurn()
    {
        MyHand.isMyTurn = false;
        RivalHand.isMyTurn = true;
        RivalHand.playedCards = 0;
        Debug.Log("Es el turno del jugador 2");
    }
    
    public void RivalChangeTurn()
    {
      MyHand.isMyTurn = true;
      RivalHand.isMyTurn = false;
      MyHand.playedCards = 0;
      Debug.Log("Es el turno del jugador 1");
    }
    public void MyPassRound()
    {
      if(MyHand.playedCards == 0 || MyHand.cardsInHand.Count==0)
      {
        Debug.Log("Jugador 1 cedio su turno");
        MyHand.IPass = true;
      }
      else
      {
        Debug.Log("No puede pasar de ronda en este turno");
      }
      if(MyHand.IPass && RivalHand.IPass)
      {
        Debug.Log("se acabo la ronda");
        MyHand.IPass = false;
        RivalHand.IPass = false;
        MyHand.playedCards = 0;
        RivalHand.playedCards = 0;
      }
    }
    public void MyRivalPass()
    {
      if(RivalHand.playedCards == 0 || RivalHand.cardsInHand.Count==0)
      {
        Debug.Log("Jugador 2 cedio su turno");
        RivalHand.IPass = true;
      }
      else
      {
        Debug.Log("No puede pasar de ronda en este turno");
      }
    }
}