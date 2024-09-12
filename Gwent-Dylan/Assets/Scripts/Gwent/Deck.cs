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
        GameObject prefabCard = deck[0];
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
   public void ChangeCards()//Permite enviar descartar 2 cartas y robar 2 desde el deck 
   {
    if(!player.isMyTurn)//Veriica si no es el turno del jugador
    {
      Debug.Log("No es tu turno");
    }
    else if(player.playedCards != 0)//Verifica si el jugador ya jugo una carta
    {
       Debug.Log("Ya no puedes usar el cambio de cartas");
    }
    else if(player.ChangedCards)//Verifica si ya se uso el cambio de cartas
    {
      Debug.Log("Ya usaste el cambio de cartas");
    }
    else if(!player.ChangedCards)//Verifica si no se ha usado el cambio de cartas
    {
       int randomIndex_1 = Random.Range(0,5);//Selecciona dos cartas  al azar de la mano
       int randomIndex_2 = Random.Range(6,player.cardsInHand.Count - 1);
       GameObject card1 = player.cardsInHand[randomIndex_1];
       GameObject card2 = player.cardsInHand[randomIndex_2];
       player.cardsInHand.RemoveAt(randomIndex_1);
       emptyZones[randomIndex_1] = false;
       player.cardsInHand.RemoveAt(randomIndex_2);
       emptyZones[randomIndex_2] = false;
       Destroy(card1);                        //Descarta esas 2 cartas
       Destroy(card2);
       for(int i = 0;i<2;i++)//Roba 2 cartas
       {
        DrawCard();
       }
       player.ChangedCards = true;//ya se uso el cambio de cartas
    }
   }
   public bool CheckEffectLure()
   {
     return player.EffectLureIsActive;//Para poder saber si esta activo el efecto senuelo(Carta Lider) 
   }
   public void IncrementPlayedCards()
   {
     player.playedCards++; //Para poder aumentar el numero de cartas jugadas(Carta Lider);
   }
}