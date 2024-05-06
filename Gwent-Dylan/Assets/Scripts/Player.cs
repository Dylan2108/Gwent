using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Player RivalPlayer;
    public List<GameObject> handZones = new List<GameObject>();//Las zonas de la mano donde van a estar las cartas
    public List<GameObject> cardsInHand = new List<GameObject>();//Para saber las cartas que estan en la mano actualmente
    public Deck deck;
    public Melee meleesZones;
    public FromDistance fromDistanceZones;
    public Siege siegeZones;
    public WeatherZone weatherZones;
    public Graveyard graveyard;
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
    public GameObject effectWeatherMenuPrefab;
    public TMP_Text Score;
    public TMP_Text RoundScore;
    public bool isMyTurn;
    public int playedCards = 0;
    public static int round = 1;
    public int RoundsWin = 0;
    public bool IPass;
    public bool ICanStillSummoning;
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
                    UpdateScore();
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
                UpdateScore();
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
                    UpdateScore();
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
                    UpdateScore();
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
                UpdateScore();
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
                    UpdateScore();
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
    public void ShowMenuSummonBoost(GameObject card, int increase)
    {
       GameObject summonMenu = Instantiate(summonBoostMenuPrefab,card.transform.position,Quaternion.identity);
       summonMenu.SetActive(true);
       summonMenu.transform.SetParent(canvas.transform,false);
       Button button1 = summonMenu.transform.Find("Button1").GetComponent<Button>();
       button1.onClick.AddListener(() => SummonBoostCard(card,"Melee",summonMenu));
       button1.onClick.AddListener(() => EffectBoost("Melee",increase));
       Button button2 = summonMenu.transform.Find("Button2").GetComponent<Button>();
       button2.onClick.AddListener(() => SummonBoostCard(card,"From Distance",summonMenu));
       button2.onClick.AddListener(() => EffectBoost("From Distance",increase));
       Button button3 = summonMenu.transform.Find("Button3").GetComponent<Button>();
       button3.onClick.AddListener(() => SummonBoostCard(card,"Siege",summonMenu));
       button3.onClick.AddListener(() => EffectBoost("Siege",increase));
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
            if(summonMenu != null)Destroy(summonMenu);
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
            if(summonMenu != null)Destroy(summonMenu);
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
            if(summonMenu != null)Destroy(summonMenu);   
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
    public int CountMeleePoints()
    {
        int counter = 0;
      for(int i = 0;i<meleesZones.cardsInMeleeZone.Count;i++)
      {
        if(meleesZones.cardsInMeleeZone[i] != null)
        {
         GameObject card = meleesZones.cardsInMeleeZone[i];
         Gold GoldComponent = card.GetComponent<Gold>();
         Silver SilverComponent = card.GetComponent<Silver>();
         if(GoldComponent!=null)
         {
            counter += GoldComponent.atk;
         }
         else if(SilverComponent!=null)
         {
            counter += SilverComponent.atk;
         }
        }
      }
      return counter;
    }
    public int CountFromDistancePoints()
    {
        int counter = 0;
        for(int i =0;i<fromDistanceZones.cardsInFromDistanceZones.Count;i++)
        {
            if(fromDistanceZones.cardsInFromDistanceZones[i] != null)
            {
                GameObject card = fromDistanceZones.cardsInFromDistanceZones[i];
            Gold GoldComponent = card.GetComponent<Gold>();
            Silver SilverComponent = card.GetComponent<Silver>();
            if(GoldComponent!=null)
            {
                counter += GoldComponent.atk;
            }
            else if(SilverComponent != null)
            {
              counter += SilverComponent.atk;
            }
            }
        }
        return counter;
    }
    public int CountSiege()
    {
        int counter = 0;
        for(int i = 0; i<siegeZones.cardsInSiegeZones.Count;i++)
        {
            if(siegeZones.cardsInSiegeZones[i] != null)
            {
                GameObject card = siegeZones.cardsInSiegeZones[i];
               Gold GoldComponent = card.GetComponent<Gold>();
               Silver SilverComponent = card.GetComponent<Silver>();
               if(GoldComponent!=null)
               {
                counter += GoldComponent.atk;
               }
               else if(SilverComponent!=null)
               {
                 counter += SilverComponent.atk;
                }
            }
        }
        return counter;
    }
    public int CountTotalPoints()
    {
        int meleepoints = CountMeleePoints();
        int adistancepoints = CountFromDistancePoints();
        int asediopoints = CountSiege();
        return meleepoints + adistancepoints + asediopoints;
    }
    public void UpdateScore()
    {
        int totalpoints = CountTotalPoints();
        Score.text = totalpoints.ToString();
    }
    public void UpdateRoundScore()
    {
        RoundScore.text = RoundsWin.ToString();
    }
    public void CleanBoard()
    {
      foreach (var card in meleesZones.cardsInMeleeZone)//Zona Cuerpo a cuerpo
       {
        card.transform.position = graveyard.transform.position;
        graveyard.cardsInGraveyard.Add(card);
       }
       meleesZones.cardsInMeleeZone.Clear();
       for(int i =0;i<emptyMeleeZones.Length;i++)
       {
         emptyMeleeZones[i] = false;
       }
       if (meleesZones.cardInMeleeBoostZone!=null)
       {
         meleesZones.cardInMeleeBoostZone.transform.position = graveyard.transform.position;
         meleesZones.cardInMeleeBoostZone = null;
         emptyMeleeBoost = false;
       }
       foreach(var card in fromDistanceZones.cardsInFromDistanceZones)//Zona A Distancia
       {
        card.transform.position = graveyard.transform.position;
        graveyard.cardsInGraveyard.Add(card);
       }
       fromDistanceZones.cardsInFromDistanceZones.Clear();
       for (int i = 0; i <emptyFromDistanceZones.Length; i++)
       {
         emptyFromDistanceZones[i] = false;
       }
       if(fromDistanceZones.cardInFromDistanceBoostZone!=null)
       {
        fromDistanceZones.cardInFromDistanceBoostZone.transform.position = graveyard.transform.position;
        fromDistanceZones.cardInFromDistanceBoostZone = null;
        emptyFromDistanceBoost = false;
       }
       foreach(var card in siegeZones.cardsInSiegeZones)//Zona Asedio
       {
        card.transform.position = graveyard.transform.position;
        graveyard.cardsInGraveyard.Add(card);
       }
       siegeZones.cardsInSiegeZones.Clear();
       for(int i =0;i<emptySiegeZones.Length;i++)
       {
        emptySiegeZones[i] = false;
       }
       if(siegeZones.cardInSiegeBoostZone!=null)
       {
        siegeZones.cardInSiegeBoostZone.transform.position = graveyard.transform.position;
        siegeZones.cardInSiegeBoostZone = null;
        emptySiegeBoost = false;
       }
       foreach(var card in weatherZones.cardsInWeatherZone)//Zona Clima
       {
         card.transform.position = graveyard.transform.position;
         graveyard.cardsInGraveyard.Add(card);
       }
       weatherZones.cardsInWeatherZone.Clear();
       for(int i =0;i<emptyWeatherZones.Length;i++)
       {
        emptyWeatherZones[i] = false;
       }
    }
    //Efectos
       public void EffectWeather(string row,int decrease,GameObject EffectMenu,Weather cardWeather)//Cartas clima
       {
         if(row == "Melee")
         {
            for(int i = 0 ;i<meleesZones.cardsInMeleeZone.Count;i++)
            {
                GameObject card = meleesZones.cardsInMeleeZone[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null) SilverComponent.atk -= decrease;
            }
            for (int i = 0; i <RivalPlayer.meleesZones.cardsInMeleeZone.Count;i++)
            {
                GameObject card = RivalPlayer.meleesZones.cardsInMeleeZone[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null) SilverComponent.atk -= decrease;
            }
            cardWeather.affectedZone = "Melee";
            Destroy(EffectMenu);
            UpdateScore();
            RivalPlayer.UpdateScore();
         }
         else if(row == "From Distance")
         {
            for(int i = 0;i<fromDistanceZones.cardsInFromDistanceZones.Count;i++)
            {
                GameObject card = fromDistanceZones.cardsInFromDistanceZones[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null)SilverComponent.atk -= decrease;
            }
            for(int i = 0;i<RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Count;i++)
            {
               GameObject card = RivalPlayer.fromDistanceZones.cardsInFromDistanceZones[i];
               Silver SilverComponent = card.GetComponent<Silver>();
               if(SilverComponent != null) SilverComponent.atk -= decrease;
            }
            cardWeather.affectedZone = "From Distance";
            Destroy(EffectMenu);
            UpdateScore();
            RivalPlayer.UpdateScore();
         }
         else if(row == "Siege")
         {
            for(int i =0;i<siegeZones.cardsInSiegeZones.Count;i++)
            {
                GameObject card = siegeZones.cardsInSiegeZones[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null)SilverComponent.atk -= decrease;
            }
            for(int i =0;i<RivalPlayer.siegeZones.cardsInSiegeZones.Count;i++)
            {
                GameObject card = RivalPlayer.siegeZones.cardsInSiegeZones[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null) SilverComponent.atk -= decrease;
            }
            cardWeather.affectedZone = "Siege";
            Destroy(EffectMenu);
            UpdateScore();
            RivalPlayer.UpdateScore();
         }
       }
       public void ShowMenuWeatherEffect(Weather card,int decrease)
       {
          GameObject EffectMenu = Instantiate(effectWeatherMenuPrefab,card.transform.position,Quaternion.identity);
          EffectMenu.SetActive(true);
          EffectMenu.transform.SetParent(canvas.transform, false);
          Button button1 = EffectMenu.transform.Find("Button1").GetComponent<Button>();
          button1.onClick.AddListener(() => EffectWeather("Melee",decrease,EffectMenu,card));
          Button button2 = EffectMenu.transform.Find("Button2").GetComponent<Button>();
          button2.onClick.AddListener(() => EffectWeather("From Distance",decrease,EffectMenu,card));
          Button button3 = EffectMenu.transform.Find("Button3").GetComponent<Button>();
          button3.onClick.AddListener(() => EffectWeather("Siege",decrease,EffectMenu,card));
       }
       public void EffectBoost(string row,int increase)//Cartas aumento
       {
          if(row == "Melee")
          {
            for(int i =0;i<meleesZones.cardsInMeleeZone.Count;i++)
            {
                GameObject card = meleesZones.cardsInMeleeZone[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null) SilverComponent.atk += increase;
            }
            UpdateScore();
          }
          else if(row == "From Distance")
          {
            for (int i = 0; i <fromDistanceZones.cardsInFromDistanceZones.Count; i++)
            {
                GameObject card = fromDistanceZones.cardsInFromDistanceZones[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null)SilverComponent.atk += increase;
            }
            UpdateScore();
          }
          else if(row == "Siege")
          {
            for(int i = 0;i<siegeZones.cardsInSiegeZones.Count;i++)
            {
                GameObject card = siegeZones.cardsInSiegeZones[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null) SilverComponent.atk += increase;
            }
            UpdateScore();
          }
       }
       public void EffectClear(int cardsToClear)//Efecto despeje
       {
          if(cardsToClear == 1)
          {
            
          }
          else if(cardsToClear == 3)
          {
            for(int i = 0;i<weatherZones.cardsInWeatherZone.Count;i++)
            {
              weatherZones.cardsInWeatherZone[i].transform.position = graveyard.transform.position;
              graveyard.cardsInGraveyard.Add(weatherZones.cardsInWeatherZone[i]);
              Weather WeatherComponent = weatherZones.cardsInWeatherZone[i].GetComponent<Weather>();
              if(WeatherComponent.Name == "Kirin")
              {
                 if(WeatherComponent.affectedZone == "Melee")
                 {
                    for(int j =0;j<meleesZones.cardsInMeleeZone.Count;j++)
                    {
                        if(meleesZones.cardsInMeleeZone[j] != null)
                        {
                            GameObject card = meleesZones.cardsInMeleeZone[j];
                            Silver SilverComponent = card.GetComponent<Silver>();
                            if(SilverComponent != null)
                            {
                                SilverComponent.atk += SilverComponent.modAtk;
                                SilverComponent.modAtk -= 2;
                            } 
                        }
                    }
                    for(int j =0;j<RivalPlayer.meleesZones.cardsInMeleeZone.Count;j++)
                    {
                        if(meleesZones.cardsInMeleeZone[j] != null)
                        {
                            GameObject card = meleesZones.cardsInMeleeZone[j];
                            Silver SilverComponent = card.GetComponent<Silver>();
                            if(SilverComponent != null)
                            {
                                SilverComponent.atk += SilverComponent.modAtk;
                                SilverComponent.modAtk -= 2;
                            } 
                        }
                    }
                 }
              }
              else if(WeatherComponent.Name == "Jutsu Bola de Fuego")
              {

              } 
            }
            weatherZones.cardsInWeatherZone.Clear();
            for(int i = 0 ;i<RivalPlayer.weatherZones.cardsInWeatherZone.Count;i++)
            {
                RivalPlayer.weatherZones.cardsInWeatherZone[i].transform.position = RivalPlayer.graveyard.transform.position;
                RivalPlayer.graveyard.cardsInGraveyard.Add(RivalPlayer.weatherZones.cardsInWeatherZone[i]);
            }
            RivalPlayer.weatherZones.cardsInWeatherZone.Clear();
          }
       }
       public void EffectLure()//Efecto cartas senuelo
       {

       }
       //Efectos Carta Unidad
       public void EliminateCardLessAtk()//Efecto que elimina la carta con menos atk del rival
       {
        GameObject CardToEliminate = null;
        int lessAtk = int.MaxValue;
        for(int i = 0;i<RivalPlayer.meleesZones.cardsInMeleeZone.Count;i++)
        {
          GameObject card = RivalPlayer.meleesZones.cardsInMeleeZone[i];
          Gold GoldComponent = card.GetComponent<Gold>();
          Silver SilverComponent = card.GetComponent<Silver>();
          if(GoldComponent != null)
          {
            if(GoldComponent.atk < lessAtk)
            {
                lessAtk = GoldComponent.atk;
                CardToEliminate = card;
            }
          }
          else if(SilverComponent != null)
          {
            if(SilverComponent.atk < lessAtk)
            {
                lessAtk = SilverComponent.atk;
                CardToEliminate = card;
            }
          }  
        }
        for(int i = 0;i<RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Count;i++)
        {
            GameObject card = RivalPlayer.fromDistanceZones.cardsInFromDistanceZones[i];
            Gold GoldComponent = card.GetComponent<Gold>();
            Silver SilverComponent = card.GetComponent<Silver>();
            if(GoldComponent != null)
            {
                if(GoldComponent.atk < lessAtk)
                {
                    lessAtk = GoldComponent.atk;
                    CardToEliminate = card;
                }
            }
            else if(SilverComponent != null)
            {
                if(SilverComponent.atk < lessAtk)
                {
                    lessAtk = SilverComponent.atk;
                    CardToEliminate = card;
                }
            }
        }
        for(int i = 0 ;i<RivalPlayer.siegeZones.cardsInSiegeZones.Count;i++)
        {
            GameObject card = RivalPlayer.siegeZones.cardsInSiegeZones[i];
            Gold GoldComponent = card.GetComponent<Gold>();
            Silver SilverComponent = card.GetComponent<Silver>();
            if(GoldComponent != null)
            {
                if(GoldComponent.atk < lessAtk)
                {
                    lessAtk = GoldComponent.atk;
                    CardToEliminate = card;
                }
            }
            else if(SilverComponent != null)
            {
                if(SilverComponent.atk < lessAtk)
                {
                    lessAtk = SilverComponent.atk;
                    CardToEliminate = card;
                }
            }
        }
        if(CardToEliminate != null)
        {
          CardToEliminate.transform.position = RivalPlayer.graveyard.transform.position;
          RivalPlayer.graveyard.cardsInGraveyard.Add(CardToEliminate);
          if(RivalPlayer.meleesZones.cardsInMeleeZone.Contains(CardToEliminate))
          {
            int index = RivalPlayer.meleesZones.cardsInMeleeZone.IndexOf(CardToEliminate);
            RivalPlayer.meleesZones.cardsInMeleeZone.RemoveAt(index);
            RivalPlayer.meleesZones.cardsInMeleeZone.Insert(index,null);
            emptyMeleeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
          else if(RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.IndexOf(CardToEliminate);
            RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.RemoveAt(index);
            RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Insert(index,null);
            emptyFromDistanceZones[index] = false;
            RivalPlayer.UpdateScore(); 
          }
          else if(RivalPlayer.siegeZones.cardsInSiegeZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.siegeZones.cardsInSiegeZones.IndexOf(CardToEliminate);
            RivalPlayer.siegeZones.cardsInSiegeZones.RemoveAt(index);
            RivalPlayer.siegeZones.cardsInSiegeZones.Insert(index,null);
            emptySiegeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
        }
       }
       public void EliminateCardHigherAtk()//Efecto que elimina la carta con mas atk del rival
       {
         GameObject CardToEliminate = null;
         int higheratk = int.MinValue;
         for(int i = 0;i<RivalPlayer.meleesZones.cardsInMeleeZone.Count;i++)
        {
          GameObject card = RivalPlayer.meleesZones.cardsInMeleeZone[i];
          Gold GoldComponent = card.GetComponent<Gold>();
          Silver SilverComponent = card.GetComponent<Silver>();
          if(GoldComponent != null)
          {
            if(GoldComponent.atk > higheratk)
            {
                higheratk = GoldComponent.atk;
                CardToEliminate = card;
            }
          }
          else if(SilverComponent != null)
          {
            if(SilverComponent.atk > higheratk)
            {
                higheratk = SilverComponent.atk;
                CardToEliminate = card;
            }
          }  
        }
        for(int i = 0;i<RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Count;i++)
        {
            GameObject card = RivalPlayer.fromDistanceZones.cardsInFromDistanceZones[i];
            Gold GoldComponent = card.GetComponent<Gold>();
            Silver SilverComponent = card.GetComponent<Silver>();
            if(GoldComponent != null)
            {
                if(GoldComponent.atk > higheratk)
                {
                    higheratk = GoldComponent.atk;
                    CardToEliminate = card;
                }
            }
            else if(SilverComponent != null)
            {
                if(SilverComponent.atk > higheratk)
                {
                    higheratk = SilverComponent.atk;
                    CardToEliminate = card;
                }
            }
        }
        for(int i = 0 ;i<RivalPlayer.siegeZones.cardsInSiegeZones.Count;i++)
        {
            GameObject card = RivalPlayer.siegeZones.cardsInSiegeZones[i];
            Gold GoldComponent = card.GetComponent<Gold>();
            Silver SilverComponent = card.GetComponent<Silver>();
            if(GoldComponent != null)
            {
                if(GoldComponent.atk > higheratk)
                {
                    higheratk = GoldComponent.atk;
                    CardToEliminate = card;
                }
            }
            else if(SilverComponent != null)
            {
                if(SilverComponent.atk > higheratk)
                {
                    higheratk = SilverComponent.atk;
                    CardToEliminate = card;
                }
            }
        }
        if(CardToEliminate != null)
        {
          CardToEliminate.transform.position = RivalPlayer.graveyard.transform.position;
          RivalPlayer.graveyard.cardsInGraveyard.Add(CardToEliminate);
          if(RivalPlayer.meleesZones.cardsInMeleeZone.Contains(CardToEliminate))
          {
            int index = RivalPlayer.meleesZones.cardsInMeleeZone.IndexOf(CardToEliminate);
            RivalPlayer.meleesZones.cardsInMeleeZone.RemoveAt(index);
            RivalPlayer.meleesZones.cardsInMeleeZone.Insert(index,null);
            emptyMeleeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
          else if(RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.IndexOf(CardToEliminate);
            RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.RemoveAt(index);
            RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Insert(index,null);
            emptyFromDistanceZones[index] = false;
            RivalPlayer.UpdateScore(); 
          }
          else if(RivalPlayer.siegeZones.cardsInSiegeZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.siegeZones.cardsInSiegeZones.IndexOf(CardToEliminate);
            RivalPlayer.siegeZones.cardsInSiegeZones.RemoveAt(index);
            RivalPlayer.siegeZones.cardsInSiegeZones.Insert(index,null);
            emptySiegeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
        }
       }
       public void InvokeWeatherCardEffect()
       {
        for(int i = 0;i<cardsInHand.Count;i++)
        {
            if(cardsInHand[i] != null)
            {
             GameObject card = cardsInHand[i];
             Weather WeatherComponent = card.GetComponent<Weather>();
             if(WeatherComponent != null)
             {
                SummonWeatherCard(WeatherComponent);
                WeatherComponent.invoked = true;
                if(WeatherComponent.Name == "Kirin")
               {
                 ShowMenuWeatherEffect(WeatherComponent,2);
               }
               else if(WeatherComponent.Name == "Jutsu Bola de Fuego")
               {
                 ShowMenuWeatherEffect(WeatherComponent,1);
               }
               return;
             }
            else if(i == cardsInHand.Count - 1) Debug.Log("No hay cartas clima en la mano");
            }
        }
       }
       public void InvokeBoostCardEffect(string row)
       {
          for(int i = 0;i<cardsInHand.Count;i++)
          {
            if(cardsInHand[i] != null)
            {
                GameObject card = cardsInHand[i];
                Boost BoostComponent = card.GetComponent<Boost>();
                if(BoostComponent != null)
                {
                    SummonBoostCard(BoostComponent.gameObject,row,null);
                    BoostComponent.invoked = true;
                    if(BoostComponent.Name == "Pildoras Ninjas") 
                    {
                       EffectBoost(row,1);
                    }
                    else if(BoostComponent.Name == "Jutsu de la Alianza Shinobi")
                    {
                       EffectBoost(row,2);
                    } 
                    return; 
                }
                else if(i == cardsInHand.Count - 1) Debug.Log("No hay cartas aumento en la mano");
            }
          }
       }
       public int CalculateProm()//Calcula el promedio de las cartas del campo
       {
          int prom = 0;//Lo hago con un entero para tener el promedio redondeado y no tener numeros con coma
          int totalcards = 0;
          int totalatk = 0;
          for(int i = 0;i < meleesZones.cardsInMeleeZone.Count;i++)
          {
            if(meleesZones.cardsInMeleeZone[i] != null)
            {
                GameObject card = meleesZones.cardsInMeleeZone[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
                {
                    totalcards++;
                    totalatk += GoldComponent.atk;
                }
                else if(SilverComponent != null)
                {
                    totalcards++;
                    totalatk += SilverComponent.atk;
                }
            }
          }
          for(int i = 0;i<fromDistanceZones.cardsInFromDistanceZones.Count;i++)
          {
            if(fromDistanceZones.cardsInFromDistanceZones[i] != null)
            {
                GameObject card = fromDistanceZones.cardsInFromDistanceZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
                {
                    totalcards++;
                    totalatk += GoldComponent.atk;
                }
                else if(SilverComponent != null)
                {
                    totalcards++;
                    totalatk += SilverComponent.atk;
                }
            }
          }
          for(int i = 0;i<siegeZones.cardsInSiegeZones.Count;i++)
          {
            if(siegeZones.cardsInSiegeZones[i] != null)
            {
                GameObject card = siegeZones.cardsInSiegeZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
                {
                    totalcards++;
                    totalatk += GoldComponent.atk;
                }
                else if(SilverComponent != null)
                {
                    totalcards++;
                    totalatk += SilverComponent.atk;
                }
            }
          }
          for(int i = 0;i<RivalPlayer.meleesZones.cardsInMeleeZone.Count;i++)
          {
            if(RivalPlayer.meleesZones.cardsInMeleeZone[i] != null)
            {
                GameObject card = RivalPlayer.meleesZones.cardsInMeleeZone[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
                {
                    totalcards++;
                    totalatk += GoldComponent.atk;
                }
                else if(SilverComponent != null)
                {
                    totalcards++;
                    totalatk += SilverComponent.atk;
                }
            }
          }
          for(int i = 0; i<RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Count;i++)
          {
            if(RivalPlayer.fromDistanceZones.cardsInFromDistanceZones[i] != null)
            {
                GameObject card = RivalPlayer.fromDistanceZones.cardsInFromDistanceZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
                {
                    totalcards++;
                    totalatk += GoldComponent.atk;
                } 
                else if(SilverComponent != null)
                {
                    totalcards++;
                    totalatk += SilverComponent.atk;
                }
            }
          }
          for(int i =0;i<RivalPlayer.siegeZones.cardsInSiegeZones.Count;i++)
          {
            if(RivalPlayer.siegeZones.cardsInSiegeZones[i] != null)
            {
                GameObject card = RivalPlayer.siegeZones.cardsInSiegeZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
                {
                    totalcards++;
                    totalatk += GoldComponent.atk;
                }
                else if(SilverComponent != null)
                {
                    totalcards++;
                    totalatk += SilverComponent.atk;
                }
            }
          }
          prom = totalatk/totalcards;
          return prom;
       }
       public void EffectProm()//Efecto que iguala mis cartas al promedio de las cartas en el campo
       {
         int prom = CalculateProm();
         for(int i = 0 ;i<meleesZones.cardsInMeleeZone.Count;i++)
         {
            if(meleesZones.cardsInMeleeZone[i] != null)
            {
                GameObject card = meleesZones.cardsInMeleeZone[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null) GoldComponent.atk = prom;
                else if(SilverComponent != null) SilverComponent.atk = prom;
            }
         }
         for(int i =0;i<fromDistanceZones.cardsInFromDistanceZones.Count;i++)
         {
            if(fromDistanceZones.cardsInFromDistanceZones[i] != null)
            {
                GameObject card = fromDistanceZones.cardsInFromDistanceZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null) GoldComponent.atk = prom;
                else if(SilverComponent != null) SilverComponent.atk = prom;
            }
         }
         for(int i =0;i<siegeZones.cardsInSiegeZones.Count;i++)
         {
            if(siegeZones.cardsInSiegeZones[i] != null)
            {
                GameObject card = siegeZones.cardsInSiegeZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null) GoldComponent.atk = prom;
                else if(SilverComponent != null) SilverComponent.atk = prom; 
            }
         }
         UpdateScore();
       }
       public void EffectDrawCard()//Efecto que permite robar una carta
       {
          if(deck.deck.Count>0)
        {
          int randomIndex = Random.Range(0,deck.deck.Count);//Elije una carta al azar(Barajea)
          GameObject prefabCard = deck.deck[randomIndex];
          deck.deck.RemoveAt(randomIndex);
           for(int i = 0;i<deck.emptyZones.Count;i++)
           {
              if(!deck.emptyZones[i])//Verifica si en esa zona puede robarse una carta
             {
               GameObject instantiatedCard = Instantiate(prefabCard,handZones[i].transform.position,Quaternion.identity);
               instantiatedCard.transform.SetParent(this.transform);//Establece a la mano como padre de la carta
                cardsInHand.Insert(i,instantiatedCard);//Actualiza la lista para saber que cartas tenemos en la mano actualmente
               deck.emptyZones[i] = true;//ACtualiza la mano para saber que esa zona esta ocupada
               break;
             }
              else if(i==9)
             {
               GameObject instantiatedCard = Instantiate(prefabCard,graveyard.transform.position,Quaternion.identity);
               instantiatedCard.transform.SetParent(this.transform);
               graveyard.cardsInGraveyard.Add(instantiatedCard);
             }
            }
         }
           else
         {
            Debug.Log("No quedan cartas en el Deck");
         }
       }
       public void EliminateRow()//Efecto que elimina todas las cartas de la fila con menor cantidad de cartas
       {
        int min = Mathf.Min(RivalPlayer.meleesZones.cardsInMeleeZone.Count,Mathf.Min(RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Count,RivalPlayer.siegeZones.cardsInSiegeZones.Count));
        if(min == RivalPlayer.meleesZones.cardsInMeleeZone.Count)
        {
            foreach(var card in RivalPlayer.meleesZones.cardsInMeleeZone)
            {
                card.transform.position = graveyard.transform.position;
                graveyard.cardsInGraveyard.Add(card);
            }
            RivalPlayer.meleesZones.cardsInMeleeZone.Clear();
            for(int i = 0 ;i<RivalPlayer.emptyMeleeZones.Length;i++)
            {
                RivalPlayer.emptyMeleeZones[i] = false;
            }
            if(RivalPlayer.meleesZones.cardInMeleeBoostZone != null)
            {
                RivalPlayer.meleesZones.cardInMeleeBoostZone.transform.position = graveyard.transform.position;
                RivalPlayer.meleesZones.cardInMeleeBoostZone = null;
                RivalPlayer.emptyMeleeBoost = false;
            }
        }
        else if(min == RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Count)
        {
            foreach(var card in RivalPlayer.fromDistanceZones.cardsInFromDistanceZones)
            {
                card.transform.position = graveyard.transform.position;
                graveyard.cardsInGraveyard.Add(card);
            }
            RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Clear();
            for(int i = 0 ;i<RivalPlayer.emptyFromDistanceZones.Length;i++)
            {
                RivalPlayer.emptyFromDistanceZones[i] = false;
            }
            if(RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone != null)
            {
                RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone.transform.position = graveyard.transform.position;
                RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone = null;
                RivalPlayer.emptyFromDistanceBoost = false;
            }
        }
        else if(min == RivalPlayer.siegeZones.cardsInSiegeZones.Count)
        {
            foreach(var card in RivalPlayer.siegeZones.cardsInSiegeZones)
            {
                card.transform.position = graveyard.transform.position;
                graveyard.cardsInGraveyard.Add(card);
            }
            RivalPlayer.siegeZones.cardsInSiegeZones.Clear();
            for(int i =0;i<RivalPlayer.emptySiegeZones.Length;i++)
            {
                RivalPlayer.emptySiegeZones[i] = false;
            }
            if(RivalPlayer.siegeZones.cardInSiegeBoostZone != null)
            {
                RivalPlayer.siegeZones.cardInSiegeBoostZone.transform.position = graveyard.transform.position;
                RivalPlayer.siegeZones.cardInSiegeBoostZone = null;
                RivalPlayer.emptySiegeBoost = false;
            }
        }
        RivalPlayer.UpdateScore();
       } 
}