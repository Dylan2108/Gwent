using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
   public  List<GameObject> deck = new List<GameObject>();
   public Player player;
   public List<bool> emptyZones = new List<bool>();//Las zonas de la mano donde se pueden robar cartas
   public void InvokeLeaderCard(GameObject PrefabLeaderCard,GameObject LeaderZone)
   {//Invoca la carta lider
     GameObject LeaderCard = Instantiate(PrefabLeaderCard,LeaderZone.transform.position,Quaternion.identity);
     LeaderCard.transform.SetParent(this.transform);
   }
   public void DrawCard()
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
            GameObject instantiatedCard = Instantiate(prefabCard,player.handZones[i].transform.position,Quaternion.identity);
            instantiatedCard.transform.SetParent(player.transform);//Establece a la mano como padre de la carta
            player.cardsInHand.Insert(i,instantiatedCard);//Actualiza la lista para saber que cartas tenemos en la mano actualmente
            emptyZones[i] = true;//ACtualiza la mano para saber que esa zona esta ocupada
            break;
          }
          else if(i==9)
          {
             GameObject instantiatedCard = Instantiate(prefabCard,player.graveyard.transform.position,Quaternion.identity);
             instantiatedCard.transform.SetParent(player.transform);
             player.graveyard.cardsInGraveyard.Add(instantiatedCard);
          }
        }
      }
      else
      {
        Debug.Log("No quedan cartas en el Deck");
      }
   }
}