using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
   public  List<GameObject> deck = new List<GameObject>();
   public Hand hand;
   public List<bool> emptyZones = new List<bool>();//Las zonas de la mano donde se pueden robar cartas
   public void InvokeLeaderCard(GameObject PrefabLeaderCard,GameObject LeaderZone)
   {//Invoca la carta lider
     GameObject LeaderCard = Instantiate(PrefabLeaderCard,LeaderZone.transform.position,Quaternion.identity);
   }
   public void DrawCard(Hand hand)
   {//Roba cartas del deck hacia la mano
      if(deck.Count>0)
      {
        int randomIndex = Random.Range(0,deck.Count);//Elije una carta al azar(Barajea)
        GameObject prefabCard = deck[randomIndex];
        deck.RemoveAt(randomIndex);
        for(int i = 0;i<emptyZones.Count;i++)
        {
          if(!emptyZones[i])//Verifica si en esa zona puede robarse una carta
          {
            GameObject instantiatedCard = Instantiate(prefabCard,hand.handZones[i].transform.position,Quaternion.identity);
            instantiatedCard.transform.SetParent(hand.transform);//Establece a la mano como padre de la carta
            hand.cardsInHand.Insert(i,instantiatedCard);//Actualiza la lista para saber que cartas tenemos en la mano actualmente
            emptyZones[i] = true;//ACtualiza la mano para saber que esa zona esta ocupada
            break;
          }
          else if(i==13)
          {
            Debug.Log("No hay mas espacio para cartas en la mano");
          }
        }
      }
      else
      {
        Debug.Log("No quedan cartas en el Deck");
      }
   }
   public void OnMouseDown()
   {//Roba una carta al hacer click sobre el deck
     DrawCard(hand);
   }
}