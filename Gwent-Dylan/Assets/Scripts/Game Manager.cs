using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public GameObject PrefabLeaderCard;
   public GameObject MyLeaderZone;
   public GameObject RivalLeaderZone;
   void Start()
   {
    Deck.InvokeLeaderCard(PrefabLeaderCard,MyLeaderZone);
    Deck.InvokeLeaderCard(PrefabLeaderCard,RivalLeaderZone);
   }
}