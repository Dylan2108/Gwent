using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
   public  List<GameObject> deck = new List<GameObject>();
   public static void InvokeLeaderCard(GameObject PrefabLeaderCard,GameObject LeaderZone)
   {
     GameObject LeaderCard = Instantiate(PrefabLeaderCard,LeaderZone.transform.position,Quaternion.identity);
   }
}