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
    MyDeck.InvokeLeaderCard(PrefabLeaderCard,MyLeaderZone);
    RivalDeck.InvokeLeaderCard(PrefabLeaderCard,RivalLeaderZone);
    for (int i = 0; i < 10; i++)
    {
      MyDeck.DrawCard(MyHand,i);
      RivalDeck.DrawCard(RivalHand,i);
    }
   }
}