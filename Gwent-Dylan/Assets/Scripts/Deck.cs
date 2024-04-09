using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
   public  List<GameObject> deck = new List<GameObject>();
   public Hand hand;
   public void InvokeLeaderCard(GameObject PrefabLeaderCard,GameObject LeaderZone)
   {
     GameObject LeaderCard = Instantiate(PrefabLeaderCard,LeaderZone.transform.position,Quaternion.identity);
   }
   public void DrawCard(Hand hand,int index)
   {
     int randomIndex = Random.Range(0,deck.Count);
     GameObject card = deck[randomIndex];
     deck.RemoveAt(randomIndex);
     Instantiate(card,hand.HandZones[index].transform.position,Quaternion.identity);
     hand.CardsInHand.Add(card);
   }
   public void OnMouseDown()
   {
     DrawCard(hand,hand.CardsInHand.Count);
   }
}