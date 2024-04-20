using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public bool emptyMeleeBoost;
    public bool emptyFromDistanceBoost;
    public bool emptySiegeBoost;
    public GameObject canvas;
    public GameObject summonBoostMenuPrefab;
    public GameObject summonLureMenuPrefab;
    public GameObject summonClearMenuPrefab;
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
    public void ShowMenuSummonBoost(GameObject card)
    {
       GameObject summonMenu = Instantiate(summonBoostMenuPrefab,card.transform.position,Quaternion.identity);
       summonMenu.SetActive(true);
       summonMenu.transform.SetParent(canvas.transform,false);
       Button button1 = summonMenu.transform.Find("Button1").GetComponent<Button>();
       button1.onClick.AddListener(() => SummonBoostCard(card,"Melee",summonMenu));
       Button button2 = summonMenu.transform.Find("Button2").GetComponent<Button>();
       button2.onClick.AddListener(() => SummonBoostCard(card,"From Distance",summonMenu));
       Button button3 = summonMenu.transform.Find("Button3").GetComponent<Button>();
       button3.onClick.AddListener(() => SummonBoostCard(card,"Siege",summonMenu));
    }
    public void SummonBoostCard(GameObject card,string row,GameObject summonMenu)
    {
          if(row == "Melee")
    {
        if(!emptyMeleeBoost)
        {
            int index = cardsInHand.IndexOf(card);
            cardsInHand.RemoveAt(index);
            cardsInHand.Insert(index,null);
            deck.emptyZones.RemoveAt(index);
            deck.emptyZones.Insert(index,false);
            card.transform.position = meleesZones.meleeBoostZone.transform.position;
            meleesZones.cardInMeleeBoostZone = card;
            emptyMeleeBoost = true;
            Destroy(summonMenu);
        }
        else
        {
            Debug.Log("Ya el espacio de la carta en la fila cuerpo a cuerpo esta ocupado");
        }
    }
    else if(row == "From Distance")
    {
        if(!emptyFromDistanceBoost)
        {
            int index = cardsInHand.IndexOf(card);
            cardsInHand.RemoveAt(index);
            cardsInHand.Insert(index,null);
            deck.emptyZones.RemoveAt(index);
            deck.emptyZones.Insert(index,false);
            card.transform.position = fromDistanceZones.fromDistanceBoostZone.transform.position;
            fromDistanceZones.cardInFromDistanceBoostZone = card;
            emptyFromDistanceBoost = true;
            Destroy(summonMenu);
        }
        else
        {
            Debug.Log("Ya el espacio de la carta en la fila A distancia esta ocupado");
        }
    }
    else if(row == "Siege")
    {
        if(!emptySiegeBoost)
        {
            int index = cardsInHand.IndexOf(card);
            cardsInHand.RemoveAt(index);
            cardsInHand.Insert(index,null);
            deck.emptyZones.RemoveAt(index);
            deck.emptyZones.Insert(index,false);
            card.transform.position = siegeZones.siegeBoostZone.transform.position;
            siegeZones.cardInSiegeBoostZone = card;
            emptySiegeBoost = true;
            Destroy(summonMenu);
        }
        else
        {
            Debug.Log("Ya el espacio de la carta en la fila asedio esta ocupado");
        }
    }
    }
    public void ShowMenuSummonLure(GameObject card)
    {
       GameObject summonMenu = Instantiate(summonLureMenuPrefab,card.transform.position,Quaternion.identity);
       summonMenu.SetActive(true);
       summonMenu.transform.SetParent(canvas.transform, false);
       Button button1 = summonMenu.transform.Find("Button1").GetComponent<Button>();
       button1.onClick.AddListener(() => SummonLureCard(card,"Melee",summonMenu));
       Button button2 = summonMenu.transform.Find("Button2").GetComponent<Button>();
       button2.onClick.AddListener(() => SummonLureCard(card,"From Distance",summonMenu));
       Button button3 = summonMenu.transform.Find("Button3").GetComponent<Button>();
       button3.onClick.AddListener(() => SummonLureCard(card,"Siege",summonMenu));
    }
    public void SummonLureCard(GameObject card,string row,GameObject summonMenu)
    {
         if(row == "Melee")
    {
        for(int i =0;i<emptyMeleeZones.Length;i++)
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
                    Destroy(summonMenu);
                    break;
                }
                else if(i==6)
                {
                    Debug.Log("Se alcanzo el mayor numero de cartas en la fila Cuerpo a cuerpo");
                    Destroy(summonMenu);
                }
            }
    }
    else if(row == "From Distance")
    {
        for(int i =0;i<emptyFromDistanceZones.Length;i++)
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
                Destroy(summonMenu);
                break;
            }
            else if(i==6)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la fila A Distancia");
                Destroy(summonMenu);
            }
    }
    }
    else if(row == "Siege")
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
                Destroy(summonMenu);
                break;
            }
            else if(i==6)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la fila Asedio");
                Destroy(summonMenu);
            }
        }
    } 
    }
    public void ShowMenuSummonClear(GameObject card)
    {
        GameObject summonMenu = Instantiate(summonClearMenuPrefab,card.transform.position,Quaternion.identity);
       summonMenu.SetActive(true);
       summonMenu.transform.SetParent(canvas.transform, false);
       Button button1 = summonMenu.transform.Find("Button1").GetComponent<Button>();
       button1.onClick.AddListener(() => SummonClearCard(card,"Melee",summonMenu));
       Button button2 = summonMenu.transform.Find("Button2").GetComponent<Button>();
       button2.onClick.AddListener(() => SummonClearCard(card,"From Distance",summonMenu));
       Button button3 = summonMenu.transform.Find("Button3").GetComponent<Button>();
       button3.onClick.AddListener(() => SummonClearCard(card,"Siege",summonMenu));
    }
    public void SummonClearCard(GameObject card,string row,GameObject summonMenu)
    {
          if(row == "Melee")
    {
        for(int i =0;i<emptyMeleeZones.Length;i++)
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
                    Destroy(summonMenu);
                    break;
                }
                else if(i==6)
                {
                    Debug.Log("Se alcanzo el mayor numero de cartas en la fila Cuerpo a cuerpo");
                    Destroy(summonMenu);
                }
            }
    }
    else if(row == "From Distance")
    {
        for(int i =0;i<emptyFromDistanceZones.Length;i++)
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
                Destroy(summonMenu);
                break;
            }
            else if(i==6)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la fila A Distancia");
                Destroy(summonMenu);
            }
    }
    }
    else if(row == "Siege")
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
                Destroy(summonMenu);
                break;
            }
            else if(i==6)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la fila Asedio");
                Destroy(summonMenu);
            }
        }
    } 
    }
}