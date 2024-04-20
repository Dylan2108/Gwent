using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public List<GameObject> handZones = new List<GameObject>();//Las zonas de la mano donde van a estar las cartas
    public List<GameObject> cardsInHand = new List<GameObject>();//Para saber las cartas que estan en la mano actualmente
    public Deck deck;
    public Melee meleesZones;
    public FromDistance fromDistanceZones;
    public Siege siegeZones;
    public WeatherZone weatherZones;
    public bool[] emptyMeleeZones = new bool[7];        //Para saber las zonas disponibles en cada fila
    public bool[] emptyFromDistanceZones = new bool[7];
    public bool[] emptySiegeZones = new bool[7];
    public static bool[] emptyWeatherZones = new bool[3];
    public void SummonGoldCard(Gold card)
    {//Invoca la carta Oro
        if(card.atkType == "Melee")
        {
            for(int i = 0;i<emptyMeleeZones.Length;i++)
            {
                if(!emptyMeleeZones[i])
                {
                    card.transform.position = meleesZones.meleesZones[i].transform.position;//Se cambia la posicion de la carta
                    meleesZones.cardsInMeleeZone.Add(card.gameObject);//Se actualizan las filas para saber donde esta cada carta
                    int index = cardsInHand.IndexOf(card.gameObject);
                    cardsInHand.RemoveAt(index); //Se elimina la carta de la mano y se reemplaza por un nuevo espacio vacio
                    cardsInHand.Insert(index,null); 
                    deck.emptyZones.RemoveAt(index);
                    deck.emptyZones.Insert(index,false);
                    emptyMeleeZones[i] = true;
                    break;
                }
                else if(i == 6)
                {
                    Debug.Log("Se alcanzo el mayor numero de cartas en la fila cuerpo a cuerpo");
                }
            }
        }
        else if(card.atkType == "From Distance")
        {
           for(int i = 0 ;i<emptyFromDistanceZones.Length;i++)
           {
            if(!emptyFromDistanceZones[i])
            {
                card.transform.position = fromDistanceZones.fromDistanceZones[i].transform.position;
                fromDistanceZones.cardsInFromDistanceZones.Add(card.gameObject);
                int index = cardsInHand.IndexOf(card.gameObject);
                cardsInHand.RemoveAt(index);
                cardsInHand.Insert(index,null);
                deck.emptyZones.RemoveAt(index);
                deck.emptyZones.Insert(index,false);
                emptyFromDistanceZones[i] = true;
                break;
            }
            else if(i == 6)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la fila de A Distancia");
            }
           }
        }
        else if(card.atkType == "Siege")
        {
            for(int i =0;i<emptySiegeZones.Length;i++)
            {
                if(!emptySiegeZones[i])
                {
                    card.transform.position = siegeZones.siegeZones[i].transform.position;
                    siegeZones.cardsInSiegeZones.Add(card.gameObject);
                    int index = cardsInHand.IndexOf(card.gameObject);
                    cardsInHand.RemoveAt(index);
                    cardsInHand.Insert(index,null);
                    deck.emptyZones.RemoveAt(index);
                    deck.emptyZones.Insert(index,false);
                    emptySiegeZones[i] = true;
                    break;
                }
                else if(i == 6)
                {
                    Debug.Log("Se alcanzo el mayor numero de cartas en la fila de Asedio");
                }
            }
        }
    }
    public void SummonSilverCard(Silver card)
    {//Invoca la carta plata
         if(card.atkType == "Melee")
        {
            for(int i = 0;i<emptyMeleeZones.Length;i++)
            {
                if(!emptyMeleeZones[i])
                {
                    card.transform.position = meleesZones.meleesZones[i].transform.position;
                    meleesZones.cardsInMeleeZone.Add(card.gameObject);
                    int index = cardsInHand.IndexOf(card.gameObject);
                    cardsInHand.RemoveAt(index);
                    cardsInHand.Insert(index,null);
                    deck.emptyZones.RemoveAt(index);
                    deck.emptyZones.Insert(index,false);
                    emptyMeleeZones[i] = true;
                    break;
                }
                else if(i == 6)
                {
                    Debug.Log("Se alcanzo el mayor numero de cartas en la fila cuerpo a cuerpo");
                }
            }
        }
        else if(card.atkType == "From Distance")
        {
           for(int i = 0 ;i<emptyFromDistanceZones.Length;i++)
           {
            if(!emptyFromDistanceZones[i])
            {
                card.transform.position = fromDistanceZones.fromDistanceZones[i].transform.position;
                fromDistanceZones.cardsInFromDistanceZones.Add(card.gameObject);
                int index = cardsInHand.IndexOf(card.gameObject);
                cardsInHand.RemoveAt(index);
                cardsInHand.Insert(index,null);
                deck.emptyZones.RemoveAt(index);
                deck.emptyZones.Insert(index,false);
                emptyFromDistanceZones[i] = true;
                break;
            }
            else if(i == 6)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la fila de A Distancia");
            }
           }
        }
        else if(card.atkType == "Siege")
        {
            for(int i =0;i<emptySiegeZones.Length;i++)
            {
                if(!emptySiegeZones[i])
                {
                    card.transform.position = siegeZones.siegeZones[i].transform.position;
                    siegeZones.cardsInSiegeZones.Add(card.gameObject);
                    int index = cardsInHand.IndexOf(card.gameObject);
                    cardsInHand.RemoveAt(index);
                    cardsInHand.Insert(index,null);
                    deck.emptyZones.RemoveAt(index);
                    deck.emptyZones.Insert(index,false);
                    emptySiegeZones[i] = true;
                    break;
                }
                else if(i == 6)
                {
                    Debug.Log("Se alcanzo el mayor numero de cartas en la fila de Asedio");
                }
            }
        }
    }
    public void SummonWeatherCard(Weather card)
    {
        for(int i =0;i<emptyWeatherZones.Length;i++)
        {
            if(!emptyWeatherZones[i])
            {
                card.transform.position = weatherZones.weatherZones[i].transform.position;
                weatherZones.cardsInWeatherZone.Add(card.gameObject);
                int index = cardsInHand.IndexOf(card.gameObject);
                cardsInHand.RemoveAt(index);
                cardsInHand.Insert(index,null);
                deck.emptyZones.RemoveAt(index);
                deck.emptyZones.Insert(index,false);
                emptyWeatherZones[i] = true;
                break;
            }
            else if(i == 2)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la zona clima");
            }
        }
    }
}