using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public Deck MyDeck;//El deck del jugador
   public Deck RivalDeck;//El deck del rival
   public GameObject PrefabLeaderCard;//Prefab de la carta lider
   public GameObject MyLeaderZone;//La zona de la carta lider del jugador
   public GameObject RivalLeaderZone;//La zona de la carta lider del rival
   public Player MyPlayer;//El jugador
   public Player RivalPlayer;//El Rival
   public Melee MyMeleeZone;//Zona Cuerpo a cuerpo del jugaor
   public Melee RivalMeleeZone;//Zona Cuerpo a cuerpo del rival
   public FromDistance MyFromDistanceZone;//Zona A Distancia del jugador
   public FromDistance RivalFromDistanceZone;//Zona A Distancia del rival
   public Siege MySiegeZone;//Zona Asedio del jugador
   public Siege RivalSiegeZone;//Zona Asedio del rival
   public Graveyard MyGraveyard;//Cementerio del jugador;
   public Graveyard RivalGraveyard;//Cementerio del rival
   public int myPoints;//Puntos del jugador
   public int rivalPoints;//Puntos del rival
   public int myRounds;//Rondas ganadas por el jugador 
   public int rivalRounds;//Rondas ganadas por el rival
   void Start()
   {//Comienza la ronda 1
    Debug.Log("Ronda 1");
    MyDeck.InvokeLeaderCard(PrefabLeaderCard,MyLeaderZone);//Se invocan las cartas lideres
    RivalDeck.InvokeLeaderCard(PrefabLeaderCard,RivalLeaderZone);
    for (int i = 0; i < 10; i++)
    {//Se roban las cartas iniciales
      MyDeck.DrawCard();
      RivalDeck.DrawCard();
    }
    MyPlayer.isMyTurn = true;//Comienza jugando el jugador 1
    Debug.Log("Es el turno del jugador 1");
   }
   void Update()
   {//Todas las cartas del cementerio se declaran como destruidas
      if(MyGraveyard.cardsInGraveyard.Count != 0)
        {
             foreach(var card in MyGraveyard.cardsInGraveyard)
          {
            Gold GoldComponent = card.GetComponent<Gold>();
            Silver SilverComponent = card.GetComponent<Silver>();
            Boost BoostComponent = card.GetComponent<Boost>();
            Clear ClearComponent = card.GetComponent<Clear>();
            Lure LureComponent = card.GetComponent<Lure>();
            Weather WeatherComponent = card.GetComponent<Weather>();
            if(GoldComponent != null) GoldComponent.destroyed = true;
            else if(SilverComponent != null) SilverComponent.destroyed = true;
            else if(BoostComponent != null) BoostComponent.destroyed = true;
            else if(ClearComponent != null) ClearComponent.destroyed = true;
            else if(LureComponent != null) LureComponent.destroyed = true;
            else if(WeatherComponent != null) WeatherComponent.destroyed = true;
           }
        }
      if(RivalGraveyard.cardsInGraveyard.Count != 0)
      {
          foreach(var card in MyGraveyard.cardsInGraveyard)
          {
            Gold GoldComponent = card.GetComponent<Gold>();
            Silver SilverComponent = card.GetComponent<Silver>();
            Boost BoostComponent = card.GetComponent<Boost>();
            Clear ClearComponent = card.GetComponent<Clear>();
            Lure LureComponent = card.GetComponent<Lure>();
            Weather WeatherComponent = card.GetComponent<Weather>();
            if(GoldComponent != null) GoldComponent.destroyed = true;
            else if(SilverComponent != null) SilverComponent.destroyed = true;
            else if(BoostComponent != null) BoostComponent.destroyed = true;
            else if(ClearComponent != null) ClearComponent.destroyed = true;
            else if(LureComponent != null) LureComponent.destroyed = true;
            else if(WeatherComponent != null) WeatherComponent.destroyed = true;
           }
      }
    myPoints = MyPlayer.CountTotalPoints();// Se cuentan los puntos del jugador y del rival
    rivalPoints = RivalPlayer.CountTotalPoints();
    if(MyPlayer.round == 1)
    {
      if(MyPlayer.IPass && RivalPlayer.IPass)//Verifica si ambos jugador cedieron la ronda
      {
        DecideWinner();//Decide el ganador de la ronda
        StartRound();//Inicia la siguiente ronda
        Debug.Log("Ronda 2");
      }
    }
    else if(MyPlayer.round == 2)
    {
       if(MyPlayer.IPass && RivalPlayer.IPass)//Verifica si ambos jugadores cedieron la ronda
       {
        DecideWinner();//Decide el ganador de la ronda
        if(MyPlayer.RoundsWin == 2) SceneManager.LoadScene("Player 1 Win");//Verifica si ya ahi algun ganador del juego
        else if(RivalPlayer.RoundsWin == 2) SceneManager.LoadScene("Player 2 Win");
        StartRound();//Inicia la siguiente ronda
        Debug.Log("Ronda 3");
       }
    }
    else if(MyPlayer.round == 3)
    {
      if(MyPlayer.IPass && RivalPlayer.IPass)//verifica si ambos jugadores cedieron la ronda
      {
         DecideWinner();//Decide el ganador  de la ronda
         if(MyPlayer.RoundsWin > RivalPlayer.RoundsWin) SceneManager.LoadScene("Player 1 Win");//Decide el ganador de la partida
         else if(MyPlayer.RoundsWin < RivalPlayer.RoundsWin) SceneManager.LoadScene("Player 2 Win");
         else if(MyPlayer.RoundsWin == RivalPlayer.RoundsWin) SceneManager.LoadScene("Tie");
      }
    }
   }
   public void MyChangeTurn()//Cambio de turno del jugador 1 al 2
    {
      if(MyPlayer.playedCards != 0 && !RivalPlayer.IPass)//Verifica si el rival no ha cedido su ronda y si el jugador ya jugo una carta en el turno
      {
        MyPlayer.isMyTurn = false;
        RivalPlayer.isMyTurn = true;
        RivalPlayer.playedCards = 0;
        Debug.Log("Es el turno del jugador 2");
      }
      else if(RivalPlayer.IPass)//Verifica si el rival ya cedio su ronda
      {
        Debug.Log("No puede pasar turno, ya el jugador contrario termino su ronda");
      }
      else
      {
        Debug.Log("Debe jugar al menos una carta");
      }
    }
    
    public void RivalChangeTurn()//Cambia de turno del jugador 2 al 1
    {
      if(RivalPlayer.playedCards != 0 && !MyPlayer.IPass)//Verifica si  el jugador no ha cedido su ronda y si el rival ya jugo una carta
      {
       MyPlayer.isMyTurn = true;
       RivalPlayer.isMyTurn = false;
       MyPlayer.playedCards = 0;
       Debug.Log("Es el turno del jugador 1");
      }
      else if(MyPlayer.IPass)//Verifica si el jugador ya cedio su ronda
      {
        Debug.Log("No puede pasar el turno ,ya el jugador contrario termino su ronda");
      }
      else
      {
        Debug.Log("Debe jugar al menos una carta");
      }
    }
    public void MyPassRound()//Cede la ronda del jugador 1
    {
      if(MyPlayer.playedCards == 0 || MyPlayer.cardsInHand.Count==0 || RivalPlayer.IPass)
      {//Verfica si no se han jugado cartas en el turno, si quedan cartas en la mano y si el rival cedio la ronda
        Debug.Log("Jugador 1 cedio su turno");
        MyPlayer.IPass = true;
        MyPlayer.isMyTurn = false;
        RivalPlayer.isMyTurn = true;
        RivalPlayer.playedCards = 0;
        RivalPlayer.ICanStillSummoning = true;//Permite al rival convocar cartas hasta que decida ceder su ronda
      }
      else
      {
        Debug.Log("No puede pasar de ronda en este turno");
      }
    }
    public void RivalPassRound()//Cede  la ronda del jugador 2
    {
      if(RivalPlayer.playedCards == 0 || RivalPlayer.cardsInHand.Count==0 || MyPlayer.IPass)
      {//Verifica si no se han jugado cartas en el turno, si quedan cartas en la mano y si el jugador cedio la ronda
        Debug.Log("Jugador 2 cedio su turno");
        RivalPlayer.IPass = true;
        MyPlayer.isMyTurn = true;
        RivalPlayer.isMyTurn = false;
        MyPlayer.playedCards = 0;
        MyPlayer.ICanStillSummoning = true;//Permite al jugador convocar cartas hasta que decida ceder su ronda
      }
      else
      {
        Debug.Log("No puede pasar de ronda en este turno");
      }
    }
    public void DecideWinner()//Decide el ganador entre rondas  
    {
       Debug.Log("Se acabo la ronda");
        MyPlayer.IPass = false;
        RivalPlayer.IPass = false;
        MyPlayer.playedCards = 0;
        RivalPlayer.playedCards = 0;
        MyPlayer.ICanStillSummoning = false;
        RivalPlayer.ICanStillSummoning = false;
        Debug.Log("Calculemos los puntos para ver al ganador");
        if(myPoints>rivalPoints)//Copmpara las puntuaciones entre jugadores
        {
          Debug.Log("La Ronda fue ganada por el Jugador 1");
          MyPlayer.RoundsWin++;
          MyPlayer.round++;
          RivalPlayer.round++;
          MyPlayer.isMyTurn = true;
          RivalPlayer.isMyTurn = false;
        }
        else if(myPoints<rivalPoints)
        {
          Debug.Log("La Ronda fue ganada por el Jugador 2");
          RivalPlayer.RoundsWin++;
          MyPlayer.round++;
          RivalPlayer.round++;
          MyPlayer.isMyTurn = false;
          RivalPlayer.isMyTurn = true;
        }
        else if(myPoints == rivalPoints)
        {
          Debug.Log("La Ronda termino en empate");
          MyPlayer.round++;
          RivalPlayer.round++;
        }
    }
    public void StartRound()//Permite comenzar las rondas
    {
        MyPlayer.CleanBoard();//Limpia los tableros de ambos jugaores
        RivalPlayer.CleanBoard();
        MyPlayer.UpdateScore();//Actualiza las puntuaciones y rondas ganadas de ambos jugadores
        RivalPlayer.UpdateScore();
        MyPlayer.UpdateRoundScore();
        RivalPlayer.UpdateRoundScore();
        for(int i =0;i<2;i++)//Roba 2 cartas al inicio de cada ronda
        {
          MyDeck.DrawCard();
          RivalDeck.DrawCard();
        }
        if(MyPlayer.isMyTurn && !RivalPlayer.isMyTurn) Debug.Log("Es el turno del jugador 1");//Decide quien comienza la ronda
        if(!MyPlayer.isMyTurn && RivalPlayer.isMyTurn) Debug.Log("Es el turno del jugador 2"); 
    }
    public int TriggerPlayer()//Devuelve el id del jugador actual
    {
      if(MyPlayer.isMyTurn) return 0;
      return 1;
    }
    public List<GameObject> Board()
    {
        List<GameObject> BoardCards = new List<GameObject>();
        for(int i = 0;i<MyPlayer.meleesZones.cardsInZones.Count;i++)
        {
           if(MyPlayer.meleesZones.cardsInZones[i]!=null)
           {
             BoardCards.Add(MyPlayer.meleesZones.cardsInZones[i]);
           }
        }
        for(int i = 0; i<MyPlayer.fromDistanceZones.cardsInZones.Count;i++)
        {
          if(MyPlayer.fromDistanceZones.cardsInZones[i]!=null)
          {
            BoardCards.Add(MyPlayer.fromDistanceZones.cardsInZones[i]);
          }
        }
        for(int i = 0;i<MyPlayer.siegeZones.cardsInZones.Count;i++)
        {
          if(MyPlayer.siegeZones.cardsInZones[i]!=null)
          {
            BoardCards.Add(MyPlayer.siegeZones.cardsInZones[i]);
          }
        }
        for(int i = 0;i<MyPlayer.weatherZones.cardsInZones.Count;i++)
        {
          if(MyPlayer.weatherZones.cardsInZones[i]!=null)
          {
            BoardCards.Add(MyPlayer.weatherZones.cardsInZones[i]);
          }
        }
        if(MyPlayer.meleesZones.cardInMeleeBoostZone!=null) BoardCards.Add(MyPlayer.meleesZones.cardInMeleeBoostZone);
        if(MyPlayer.fromDistanceZones.cardInFromDistanceBoostZone!=null) BoardCards.Add(MyPlayer.fromDistanceZones.cardInFromDistanceBoostZone);
        if(MyPlayer.siegeZones.cardInSiegeBoostZone!=null) BoardCards.Add(MyPlayer.siegeZones.cardInSiegeBoostZone);
        for(int i =0;i<RivalPlayer.meleesZones.cardsInZones.Count;i++)
        {
          if(RivalPlayer.meleesZones.cardsInZones[i]!=null)
          {
            BoardCards.Add(RivalPlayer.meleesZones.cardsInZones[i]);
          }
        }
         for(int i =0;i<RivalPlayer.fromDistanceZones.cardsInZones.Count;i++)
        {
          if(RivalPlayer.fromDistanceZones.cardsInZones[i]!=null)
          {
            BoardCards.Add(RivalPlayer.fromDistanceZones.cardsInZones[i]);
          }
        }
         for(int i =0;i<RivalPlayer.siegeZones.cardsInZones.Count;i++)
        {
          if(RivalPlayer.siegeZones.cardsInZones[i]!=null)
          {
            BoardCards.Add(RivalPlayer.siegeZones.cardsInZones[i]);
          }
        }
        if(RivalPlayer.meleesZones.cardInMeleeBoostZone!=null) BoardCards.Add(RivalPlayer.meleesZones.cardInMeleeBoostZone);
        if(RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone!=null) BoardCards.Add(RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone);
        if(RivalPlayer.siegeZones.cardInSiegeBoostZone!=null) BoardCards.Add(RivalPlayer.siegeZones.cardInSiegeBoostZone);
        return BoardCards;
    }
    public List<GameObject> HandOfPlayer(int ID)
    {
      List<GameObject> HandCards = new List<GameObject>();
       if(ID == 0)
       {
         for(int i = 0;i<MyPlayer.cardsInHand.Count;i++)
         {
             if(MyPlayer.cardsInHand[i]!=null)
             {
                HandCards.Add(MyPlayer.cardsInHand[i]);
             }
         }
         return HandCards;
       }
       else
       {
          for(int i = 0;i<RivalPlayer.cardsInHand.Count;i++)
         {
           if(RivalPlayer.cardsInHand[i]!=null)
            {
             HandCards.Add(RivalPlayer.cardsInHand[i]);
            }
         }
          return HandCards;
       }
    }
    public List<GameObject> FieldOfPlayer(int ID)
    {
      List<GameObject> FieldCards = new List<GameObject>();
      if(ID == 0)
      {
        for(int i = 0;i<MyPlayer.meleesZones.cardsInZones.Count;i++)
        {
           if(MyPlayer.meleesZones.cardsInZones[i]!=null)
           {
             FieldCards.Add(MyPlayer.meleesZones.cardsInZones[i]);
           }
        }
        for(int i = 0; i<MyPlayer.fromDistanceZones.cardsInZones.Count;i++)
        {
          if(MyPlayer.fromDistanceZones.cardsInZones[i]!=null)
          {
            FieldCards.Add(MyPlayer.fromDistanceZones.cardsInZones[i]);
          }
        }
        for(int i = 0;i<MyPlayer.siegeZones.cardsInZones.Count;i++)
        {
          if(MyPlayer.siegeZones.cardsInZones[i]!=null)
          {
            FieldCards.Add(MyPlayer.siegeZones.cardsInZones[i]);
          }
        }
        for(int i = 0;i<MyPlayer.weatherZones.cardsInZones.Count;i++)
        {
          if(MyPlayer.weatherZones.cardsInZones[i]!=null)
          {
            FieldCards.Add(MyPlayer.weatherZones.cardsInZones[i]);
          }
        }
        if(MyPlayer.meleesZones.cardInMeleeBoostZone!=null) FieldCards.Add(MyPlayer.meleesZones.cardInMeleeBoostZone);
        if(MyPlayer.fromDistanceZones.cardInFromDistanceBoostZone!=null) FieldCards.Add(MyPlayer.fromDistanceZones.cardInFromDistanceBoostZone);
        if(MyPlayer.siegeZones.cardInSiegeBoostZone!=null) FieldCards.Add(MyPlayer.siegeZones.cardInSiegeBoostZone);
        return FieldCards;
      }
      else
      {
         for(int i = 0;i<RivalPlayer.meleesZones.cardsInZones.Count;i++)
        {
           if(RivalPlayer.meleesZones.cardsInZones[i]!=null)
           {
             FieldCards.Add(RivalPlayer.meleesZones.cardsInZones[i]);
           }
        }
        for(int i = 0; i<RivalPlayer.fromDistanceZones.cardsInZones.Count;i++)
        {
          if(RivalPlayer.fromDistanceZones.cardsInZones[i]!=null)
          {
            FieldCards.Add(RivalPlayer.fromDistanceZones.cardsInZones[i]);
          }
        }
        for(int i = 0;i<RivalPlayer.siegeZones.cardsInZones.Count;i++)
        {
          if(RivalPlayer.siegeZones.cardsInZones[i]!=null)
          {
            FieldCards.Add(RivalPlayer.siegeZones.cardsInZones[i]);
          }
        }
        for(int i = 0;i<RivalPlayer.weatherZones.cardsInZones.Count;i++)
        {
          if(RivalPlayer.weatherZones.cardsInZones[i]!=null)
          {
            FieldCards.Add(RivalPlayer.weatherZones.cardsInZones[i]);
          }
        }
        if(RivalPlayer.meleesZones.cardInMeleeBoostZone!=null) FieldCards.Add(RivalPlayer.meleesZones.cardInMeleeBoostZone);
        if(RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone!=null) FieldCards.Add(RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone);
        if(RivalPlayer.siegeZones.cardInSiegeBoostZone!=null) FieldCards.Add(RivalPlayer.siegeZones.cardInSiegeBoostZone);
        return FieldCards;
      }
    }
    public List<GameObject> GraveyardOfPlayer(int ID)
    {   List<GameObject> GraveyardCards = new List<GameObject>();
       if(ID==0)
       {
          for(int i = 0;i<MyPlayer.graveyard.cardsInGraveyard.Count;i++)
          {
             if(MyPlayer.graveyard.cardsInGraveyard[i]!=null)
             {
                GraveyardCards.Add(MyPlayer.graveyard.cardsInGraveyard[i]);
             }
          }
          return GraveyardCards;
       }
       else
       {
           for(int i = 0;i<RivalPlayer.graveyard.cardsInGraveyard.Count;i++)
          {
             if(RivalPlayer.graveyard.cardsInGraveyard[i]!=null)
             {
                GraveyardCards.Add(RivalPlayer.graveyard.cardsInGraveyard[i]);
             }
          }
          return GraveyardCards;
       }
    }
    public List<GameObject> DeckOfPlayer(int ID)
    {
      List<GameObject> DeckCards = new List<GameObject>();
      if(ID==0)
      {
        for(int i = 0;i<MyPlayer.deck.deck.Count;i++)
        {
          if(MyPlayer.deck.deck[i]!=null)
          {
            DeckCards.Add(MyPlayer.deck.deck[i]);
          }
        }
        return DeckCards;  
      }
      else
      {
        for(int i =0;i<RivalPlayer.deck.deck.Count;i++)
        {
          if(RivalPlayer.deck.deck[i]!=null)
          {
            DeckCards.Add(RivalPlayer.deck.deck[i]);
          }
        }
        return DeckCards;
      }
    }
}