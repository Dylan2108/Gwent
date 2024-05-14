using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Player RivalPlayer; //El jugador rival
    public List<GameObject> handZones = new List<GameObject>();//Las zonas de la mano donde van a estar las cartas
    public List<GameObject> cardsInHand = new List<GameObject>();//Para saber las cartas que estan en la mano actualmente
    public Deck deck; //El deck del jugador
    public Melee meleesZones; //Zona Cuerpo a Cuerpo
    public FromDistance fromDistanceZones; //Zona A Distance
    public Siege siegeZones; //Zona Asedio
    public WeatherZone weatherZones; //Zona Clima
    public Graveyard graveyard; //Cementerio
    public bool[] emptyMeleeZones = new bool[7];      //Zonas disponibles para convocar(Cuerpo a cuerpo)
    public bool[] emptyFromDistanceZones = new bool[7]; //Zonas disponibles para convocar(A Distancia)
    public bool[] emptySiegeZones = new bool[7]; //Zonas disponibles para convocar(Asedio)
    public bool[] emptyWeatherZones = new bool[3]; //Zonas disponibles para convocar(Clima)
    public bool emptyMeleeBoost;  //Zonas disponibles para convocar(Aumento de Cuerpo a Cuerpo)
    public bool emptyFromDistanceBoost; //Zonas disponibles para convocar(Aumento de A Distancia)
    public bool emptySiegeBoost; //Zonas disponibles para convocar(Aumento de Asedio)
    public GameObject canvas; // El canvas donde se van a guardar todos los botones y paneles
    public GameObject summonBoostMenuPrefab; //Panel para decidir que fila voy a afectar con la carta Aumento
    public GameObject effectWeatherMenuPrefab; //Panel para decidir que fila voy a afectar con la carta Clima
    public TMP_Text Score; //El marcador del jugador
    public TMP_Text RoundScore; //El marcador de las Rondas ganadas por el jugador
    public bool isMyTurn; //Para saber si es el turno del jugador
    public int playedCards = 0; //Para saber las cartas jugadas por el jugador en cada turno
    public int round = 1; // Para saber en que ronda estamos actualmente
    public int RoundsWin = 0; //Las Rondas ganadas por el jugador
    public bool IPass; //Para saber si el jugador ya quiere terminar su ronda
    public bool ICanStillSummoning; //Para saber si el jugador puede seguir convocando cartas(Cuando el rival cede su turno)
    public bool EffectLureIsActive; //Para saber si se quiere activar el efecto de la carta Senuelo
    public bool ChangedCards; //Para saber si el jugador ya utilizo el intercambio de cartas
    public void SummonGoldCard(Gold card)
    {//Invoca la carta Oro
        if(card.atkType == "Melee")
        {
            for(int i = 0;i<emptyMeleeZones.Length;i++)
            {
                if(!emptyMeleeZones[i])
                {
                    card.transform.position = meleesZones.meleesZones[i].transform.position;//Se cambia la posicion de la carta
                    meleesZones.cardsInMeleeZone[i] = card.gameObject;//Se actualizan las filas para saber donde esta cada carta
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
                fromDistanceZones.cardsInFromDistanceZones[i] = card.gameObject;
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
                    siegeZones.cardsInSiegeZones[i] = card.gameObject;
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
                    meleesZones.cardsInMeleeZone[i] = card.gameObject;
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
                fromDistanceZones.cardsInFromDistanceZones[i] = card.gameObject;
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
                    siegeZones.cardsInSiegeZones[i] = card.gameObject;
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
    public void SummonWeatherCard(Weather card)//Para invocar las cartas clima
    {
        for(int i =0;i<emptyWeatherZones.Length;i++)
        {
            if(!emptyWeatherZones[i])//Se verifica si es posible convocar la carta
            {
                card.transform.position = weatherZones.weatherZones[i].transform.position;//Se cambia la posicion de la carta hacia la zona clima
                weatherZones.cardsInWeatherZone[i] = card.gameObject; //Se actualiza la lista de cartas clima
                int index = cardsInHand.IndexOf(card.gameObject);
                cardsInHand.RemoveAt(index); // Se actualizan los espacios en blanco de la mano
                cardsInHand.Insert(index,null);
                deck.emptyZones.RemoveAt(index);
                deck.emptyZones.Insert(index,false);
                emptyWeatherZones[i] = true; // Se marcan los espacios de la carta clima como ocupados
                RivalPlayer.emptyWeatherZones[i] = true;
                break;
            }
            else if(i == 2)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la zona clima");
            }
        }
    }
    public void ShowMenuSummonBoost(GameObject card, int increase)// Para mostrar el panel para elegir la zona que queremos afectar 
    {
       GameObject summonMenu = Instantiate(summonBoostMenuPrefab,card.transform.position,Quaternion.identity);
       summonMenu.SetActive(true);//Se instancia el panel
       summonMenu.transform.SetParent(canvas.transform,false);
       Button button1 = summonMenu.transform.Find("Button1").GetComponent<Button>();//Se le agrega la funcionalidad a los botones
       button1.onClick.AddListener(() => SummonBoostCard(card,"Melee",summonMenu));
       button1.onClick.AddListener(() => EffectBoost("Melee",increase));
       Button button2 = summonMenu.transform.Find("Button2").GetComponent<Button>();
       button2.onClick.AddListener(() => SummonBoostCard(card,"From Distance",summonMenu));
       button2.onClick.AddListener(() => EffectBoost("From Distance",increase));
       Button button3 = summonMenu.transform.Find("Button3").GetComponent<Button>();
       button3.onClick.AddListener(() => SummonBoostCard(card,"Siege",summonMenu));
       button3.onClick.AddListener(() => EffectBoost("Siege",increase));
    }
    public void SummonBoostCard(GameObject card,string row,GameObject summonMenu) //Para invocar cartas Aumento
    {
          if(row == "Melee") //Boton Cuerpo a cuerpo
    {
        if(!emptyMeleeBoost) // Se verifica si es posible invocar la carta
        {
            int index = cardsInHand.IndexOf(card);//Se retira la carta de la mano
            cardsInHand.RemoveAt(index);
            cardsInHand.Insert(index,null);
            deck.emptyZones.RemoveAt(index);
            deck.emptyZones.Insert(index,false);
            card.transform.position = meleesZones.meleeBoostZone.transform.position; //Se invoca en el campo
            meleesZones.cardInMeleeBoostZone = card;
            emptyMeleeBoost = true;
            if(summonMenu != null)Destroy(summonMenu);
        }
        else
        {
            Debug.Log("Ya el espacio de la carta en la fila cuerpo a cuerpo esta ocupado");
        }
    }
    else if(row == "From Distance") //Boton a distancia 
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
    else if(row == "Siege") //Boton asedio
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
    public void SummonClearCard(GameObject card) //Para invocar cartas despeje
    {
        card.transform.position = graveyard.transform.position; //Se lleva al cementerio
        graveyard.cardsInGraveyard.Add(card);                   //como es una carta de efecto rapido
        int index = cardsInHand.IndexOf(card.gameObject);       //no es necesario llevarla al cementerio
        cardsInHand.RemoveAt(index); //Se retira la carta de la mano
        cardsInHand.Insert(index,null); 
        deck.emptyZones.RemoveAt(index);
        deck.emptyZones.Insert(index,false);
    }
    public int CountMeleePoints() //Para sumar el ataque de las cartas
    {                             //de la zona Cuerpo a cuerpo
        int counter = 0;
      for(int i = 0;i<meleesZones.cardsInMeleeZone.Count;i++)
      {
        if(meleesZones.cardsInMeleeZone[i] != null)
        {
         GameObject card = meleesZones.cardsInMeleeZone[i];
         Gold GoldComponent = card.GetComponent<Gold>(); //Cartas Oro
         Silver SilverComponent = card.GetComponent<Silver>();//Cartas Plata
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
    public int CountFromDistancePoints()//Para sumar el ataque de las cartas
    {                                   //de la zona A Distancia
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
    public int CountSiege()//Para sumar el ataque de las cartas de la zona Asedio
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
    public int CountTotalPoints()//Para sumar el ataque de todas las zonas
    {
        int meleepoints = CountMeleePoints();//Los puntos de Cuerpo a cuerpo
        int adistancepoints = CountFromDistancePoints();//Los puntos de A Distancia
        int asediopoints = CountSiege();//Los puntos de ASedio
        return meleepoints + adistancepoints + asediopoints;
    }
    public void UpdateScore()//Actualiza el marcador de los puntos del jugador
    {
        int totalpoints = CountTotalPoints();
        Score.text = totalpoints.ToString();
    }
    public void UpdateRoundScore()//Actualiza el marcador de las rondas del jugador
    {
        RoundScore.text = RoundsWin.ToString();
    }
    public void CleanBoard()//Se encarga de vaciar el campo
    {
      for (int i =0;i<meleesZones.cardsInMeleeZone.Count;i++)//Zona Cuerpo a cuerpo
       {
         if(meleesZones.cardsInMeleeZone[i] != null)
         {//Envia todas las cartas de la zona Cuerpo a cuerpo al cementerio
           meleesZones.cardsInMeleeZone[i].transform.position = graveyard.transform.position;
           graveyard.cardsInGraveyard.Add(meleesZones.cardsInMeleeZone[i]);
           meleesZones.cardsInMeleeZone[i] = null;
         }
       }
       for(int i =0;i<emptyMeleeZones.Length;i++)
       {//Pone todos los espacios en blanco
         emptyMeleeZones[i] = false;
       }
       if (meleesZones.cardInMeleeBoostZone!=null)
       {//Se envia la carta aumento al cementerio
         meleesZones.cardInMeleeBoostZone.transform.position = graveyard.transform.position;
         meleesZones.cardInMeleeBoostZone = null;
         emptyMeleeBoost = false;
       }
       for(int i = 0;i<fromDistanceZones.cardsInFromDistanceZones.Count;i++)//Zona A Distancia
       {//Envia todas las cartas de la zona A Distancia al cementerio
          if(fromDistanceZones.cardsInFromDistanceZones[i] != null)
          {
            fromDistanceZones.cardsInFromDistanceZones[i].transform.position = graveyard.transform.position;
            graveyard.cardsInGraveyard.Add(fromDistanceZones.cardsInFromDistanceZones[i]);
            fromDistanceZones.cardsInFromDistanceZones[i] = null;
          }
       }
       for (int i = 0; i <emptyFromDistanceZones.Length; i++)
       {//Pone todos los espacios en blanco
         emptyFromDistanceZones[i] = false;
       }
       if(fromDistanceZones.cardInFromDistanceBoostZone!=null)
       {//Se envia la carta Aumento al cementerio
        fromDistanceZones.cardInFromDistanceBoostZone.transform.position = graveyard.transform.position;
        fromDistanceZones.cardInFromDistanceBoostZone = null;
        emptyFromDistanceBoost = false;
       }
       for(int i = 0;i <siegeZones.cardsInSiegeZones.Count;i++)//Zona Asedio
       {//Envia al cementerio todas las cartas de la zona Asedio
         if(siegeZones.cardsInSiegeZones[i] != null)
         {
           siegeZones.cardsInSiegeZones[i].transform.position = graveyard.transform.position;
           graveyard.cardsInGraveyard.Add(siegeZones.cardsInSiegeZones[i]);
           siegeZones.cardsInSiegeZones[i] = null;
         }
       }
       for(int i =0;i<emptySiegeZones.Length;i++)
       {//Pone todos los espacios en blanco
        emptySiegeZones[i] = false;
       }
       if(siegeZones.cardInSiegeBoostZone!=null)
       {//Envia la carta aumento al cementerio
        siegeZones.cardInSiegeBoostZone.transform.position = graveyard.transform.position;
        siegeZones.cardInSiegeBoostZone = null;
        emptySiegeBoost = false;
       }
       for(int i = 0;i<weatherZones.cardsInWeatherZone.Count;i++)//Zona Clima
       {
          if(weatherZones.cardsInWeatherZone[i] != null)
          {//Envia al cementerio todas las cartas Clima
           weatherZones.cardsInWeatherZone[i].transform.position = graveyard.transform.position;
           graveyard.cardsInGraveyard.Add(weatherZones.cardsInWeatherZone[i]);
           weatherZones.cardsInWeatherZone[i] = null;
          }
       }
       for(int i =0;i<emptyWeatherZones.Length;i++)
       {//Pone todos los espacios en blanco
        emptyWeatherZones[i] = false;
       }
    }
    //Efectos
       public void EffectWeather(string row,int decrease,GameObject EffectMenu,Weather cardWeather)//Cartas clima
       {
         if(row == "Melee")
         {
            for(int i = 0 ;i<meleesZones.cardsInMeleeZone.Count;i++)
            {// Recorre la fila cuerpo a cuerpo
                if(meleesZones.cardsInMeleeZone[i] != null)
                {//Le disminuye el ataque a las cartas plata en la fila
                    GameObject card = meleesZones.cardsInMeleeZone[i];
                    Silver SilverComponent = card.GetComponent<Silver>();
                    if(SilverComponent != null)
                   {
                    SilverComponent.atk -= decrease;
                    cardWeather.affectedCards.Add(SilverComponent);
                   } 
                }   
            }
            for (int i = 0; i <RivalPlayer.meleesZones.cardsInMeleeZone.Count;i++)
            {//Recorre la fila Cuerpo a cuerpo del rival
                if(RivalPlayer.meleesZones.cardsInMeleeZone[i] != null)
                {//Se disminuye el ataque a las cartas plata en la fila
                   GameObject card = RivalPlayer.meleesZones.cardsInMeleeZone[i];
                   Silver SilverComponent = card.GetComponent<Silver>();
                   if(SilverComponent != null)
                   {
                      SilverComponent.atk -= decrease;
                      cardWeather.affectedCards.Add(SilverComponent);
                   } 
                }
            }
            Destroy(EffectMenu);
            UpdateScore();
            RivalPlayer.UpdateScore();
         }
         else if(row == "From Distance")
         {
            for(int i = 0;i<fromDistanceZones.cardsInFromDistanceZones.Count;i++)
            {//Recorre la fila A Distancia del jugador
                if(fromDistanceZones.cardsInFromDistanceZones[i] != null)
                {//Se disminuye el ataque a las cartas plata en la fila
                    GameObject card = fromDistanceZones.cardsInFromDistanceZones[i];
                    Silver SilverComponent = card.GetComponent<Silver>();
                    if(SilverComponent != null)
                    {
                       SilverComponent.atk -= decrease;
                       cardWeather.affectedCards.Add(SilverComponent);
                    }
                }
            }
            for(int i = 0;i<RivalPlayer.fromDistanceZones.cardsInFromDistanceZones.Count;i++)
            {//Recorre la fila A Distancia del rival
               if(RivalPlayer.fromDistanceZones.cardsInFromDistanceZones[i] != null)
               {//Se disminuye el ataque de las cartas plata en la fila
                  GameObject card = RivalPlayer.fromDistanceZones.cardsInFromDistanceZones[i];
                  Silver SilverComponent = card.GetComponent<Silver>();
                  if(SilverComponent != null)
                  {
                    SilverComponent.atk -= decrease;
                    cardWeather.affectedCards.Add(SilverComponent);
                  } 
               }
            }
            Destroy(EffectMenu);
            UpdateScore();
            RivalPlayer.UpdateScore();
         }
         else if(row == "Siege")
         {
            for(int i =0;i<siegeZones.cardsInSiegeZones.Count;i++)
            {//Recorre la fila Asedio del jugador
                if(siegeZones.cardsInSiegeZones[i] != null)
                {//Se disminuye el ataque de las cartas plata en la fila
                    GameObject card = siegeZones.cardsInSiegeZones[i];
                    Silver SilverComponent = card.GetComponent<Silver>();
                    if(SilverComponent != null)
                    {
                       SilverComponent.atk -= decrease;
                       cardWeather.affectedCards.Add(SilverComponent);
                    }
                }
            }
            for(int i =0;i<RivalPlayer.siegeZones.cardsInSiegeZones.Count;i++)
            {//Se recorre la fila Asedio del rival
                if(RivalPlayer.siegeZones.cardsInSiegeZones[i] != null)
                {//Se disminuye el ataque de las cartas plata de la fila
                    GameObject card = RivalPlayer.siegeZones.cardsInSiegeZones[i];
                    Silver SilverComponent = card.GetComponent<Silver>();
                    if(SilverComponent != null)
                    {
                       SilverComponent.atk -= decrease;
                       cardWeather.affectedCards.Add(SilverComponent);
                    }
                }
            }
            Destroy(EffectMenu);
            UpdateScore();
            RivalPlayer.UpdateScore();
         }
       }
       public void ShowMenuWeatherEffect(Weather card,int decrease)//Muestra el panel que nos permite seleccionar que zona afectar
       {
          GameObject EffectMenu = Instantiate(effectWeatherMenuPrefab,card.transform.position,Quaternion.identity);
          EffectMenu.SetActive(true);//Instancia el panel
          EffectMenu.transform.SetParent(canvas.transform, false);
          Button button1 = EffectMenu.transform.Find("Button1").GetComponent<Button>();//Le da la funcionalidad a los botones
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
            {//Recorre la fila Cuerpo a cuerpo del jugador
                if(meleesZones.cardsInMeleeZone[i] != null)
                {//Aumenta el ataque de las cartas de la fila
                  GameObject card = meleesZones.cardsInMeleeZone[i];
                  Silver SilverComponent = card.GetComponent<Silver>();
                  if(SilverComponent != null) SilverComponent.atk += increase;
                }
            }
            UpdateScore();
          }
          else if(row == "From Distance")
          {
            for (int i = 0; i <fromDistanceZones.cardsInFromDistanceZones.Count; i++)
            {//Recorre la fila A Distancia del jugador
                if(fromDistanceZones.cardsInFromDistanceZones[i] != null)
                {//Aumenta el ataque de las cartas de la fila
                    GameObject card = fromDistanceZones.cardsInFromDistanceZones[i];
                    Silver SilverComponent = card.GetComponent<Silver>();
                    if(SilverComponent != null)SilverComponent.atk += increase;
                }
            }
            UpdateScore();
          }
          else if(row == "Siege")
          {
            for(int i = 0;i<siegeZones.cardsInSiegeZones.Count;i++)
            {//Recorre la fila Asedio del jugador
                if(siegeZones.cardsInSiegeZones[i] != null)
                {//AUmenta el ataque de las cartas de la fila
                   GameObject card = siegeZones.cardsInSiegeZones[i];
                   Silver SilverComponent = card.GetComponent<Silver>();
                   if(SilverComponent != null) SilverComponent.atk += increase;
                }
            }
            UpdateScore();
          }
       }
       public void EffectClear(int cardsToClear)//Efecto despeje
       {
          if(cardsToClear == 1)//Verifica la cantidad de cartas a eliminar
          {
            for(int i =0;i<RivalPlayer.weatherZones.cardsInWeatherZone.Count;i++)
            {//Recorre la Zona Clima del rival
                if(RivalPlayer.weatherZones.cardsInWeatherZone[i] != null)
                {//Se destruyen las cartas clima
                    RivalPlayer.weatherZones.cardsInWeatherZone[i].transform.position = RivalPlayer.graveyard.transform.position;
                    RivalPlayer.graveyard.cardsInGraveyard.Add(RivalPlayer.weatherZones.cardsInWeatherZone[i]);
                    Weather WeatherComponent = RivalPlayer.weatherZones.cardsInWeatherZone[i].GetComponent<Weather>();
                    RivalPlayer.weatherZones.cardsInWeatherZone[i] = null;
                    if(WeatherComponent.Name == "Kirin")//Se le devuelve el ataque quitado a las cartas afectadas
                    {
                        for(int j = 0;j<WeatherComponent.affectedCards.Count;j++)
                        {
                            WeatherComponent.affectedCards[j].atk += 2;
                        }
                        WeatherComponent.affectedCards.Clear();
                        UpdateScore();
                        RivalPlayer.UpdateScore();
                        return;
                    }
                    else if(WeatherComponent.Name == "Jutsu Bola de Fuego")
                    {
                        for(int j = 0;j<WeatherComponent.affectedCards.Count;j++)
                        {
                            WeatherComponent.affectedCards[j].atk += 1;
                        }
                        WeatherComponent.affectedCards.Clear();
                        UpdateScore();
                        RivalPlayer.UpdateScore();
                        return;
                    }
                }
            }
            for(int i = 0 ;i<weatherZones.cardsInWeatherZone.Count;i++)
            {//Se recorre la zona clima del jugador
                if(weatherZones.cardsInWeatherZone[i] != null)
                {//Se eliminan las cartas climas
                    weatherZones.cardsInWeatherZone[i].transform.position = graveyard.transform.position;
                    graveyard.cardsInGraveyard.Add(weatherZones.cardsInWeatherZone[i]);
                    Weather WeatherComponent = weatherZones.cardsInWeatherZone[i].GetComponent<Weather>();
                    weatherZones.cardsInWeatherZone[i] = null;
                    if(WeatherComponent.Name == "Kirin")//Se le devuelve el ataque a las cartas modificadas
                    {
                        for(int j = 0;j<WeatherComponent.affectedCards.Count;j++)
                        {
                            WeatherComponent.affectedCards[j].atk += 2;
                        }
                        WeatherComponent.affectedCards.Clear();
                        UpdateScore();
                        RivalPlayer.UpdateScore();
                        return;
                    }
                    else if(WeatherComponent.Name == "Jutsu Bola de Fuego")
                    {
                        for (int j = 0; j <WeatherComponent.affectedCards.Count; j++)
                        {
                            WeatherComponent.affectedCards[j].atk += 1;
                        }
                        WeatherComponent.affectedCards.Clear();
                        UpdateScore();
                        RivalPlayer.UpdateScore();
                        return;
                    }
                }
            }
          }
          else if(cardsToClear == 3)//Se verifican la cantidad de cartas a eliminar
          {
            for(int i = 0;i<weatherZones.cardsInWeatherZone.Count;i++)
            { //Se recorre la zona clima del jugador
                  if(weatherZones.cardsInWeatherZone[i] != null)
                {//Se eliminan las cartas climas
                     weatherZones.cardsInWeatherZone[i].transform.position = graveyard.transform.position;
                     graveyard.cardsInGraveyard.Add(weatherZones.cardsInWeatherZone[i]);
                     Weather WeatherComponent = weatherZones.cardsInWeatherZone[i].GetComponent<Weather>();
                     weatherZones.cardsInWeatherZone [i] = null;
                     if(WeatherComponent.Name == "Kirin")//Se les devuelve el ataque a las cartas afectadas
                     {
                       for(int j = 0 ;j<WeatherComponent.affectedCards.Count;j++)
                       {
                          WeatherComponent.affectedCards[j].atk += 2;
                       }
                       WeatherComponent.affectedCards.Clear();
                     }
                     else if(WeatherComponent.Name == "Jutsu Bola de Fuego")
                     {
                       for(int j =0;j<WeatherComponent.affectedCards.Count;j++)
                      {
                         WeatherComponent.affectedCards[j].atk += 1;
                      }
                      WeatherComponent.affectedCards.Clear();
                     } 
                }
            }
            for(int i = 0 ;i<RivalPlayer.weatherZones.cardsInWeatherZone.Count;i++)
            {//Se recorre la zona clima del rival
                if(RivalPlayer.weatherZones.cardsInWeatherZone[i] != null)
                {//Se eliminan las cartas clima
                      RivalPlayer.weatherZones.cardsInWeatherZone[i].transform.position = RivalPlayer.graveyard.transform.position;
                      RivalPlayer.graveyard.cardsInGraveyard.Add(RivalPlayer.weatherZones.cardsInWeatherZone[i]);
                      Weather WeatherComponent = RivalPlayer.weatherZones.cardsInWeatherZone[i].GetComponent<Weather>();
                      RivalPlayer.weatherZones.cardsInWeatherZone[i] = null;
                      if(WeatherComponent.Name == "Kirin")//Se les devuelve el ataque a las cartas afectadas
                    {
                       for(int j =0;j<WeatherComponent.affectedCards.Count;j++)
                       {
                          WeatherComponent.affectedCards[j].atk += 2;
                       }
                      WeatherComponent.affectedCards.Clear();
                    }
                    else if(WeatherComponent.Name == "Jutsu Bola de Fuego")
                   {
                    for(int j = 0;j<WeatherComponent.affectedCards.Count;j++)
                    {
                        WeatherComponent.affectedCards[j].atk += 1;
                    }
                    WeatherComponent.affectedCards.Clear();
                   }
                }
            }
          }
          UpdateScore();
          RivalPlayer.UpdateScore();
       }
       public void EffectLure(Silver choosencard,Lure LureCard)//Efecto cartas senuelo
       {
                if(choosencard != null)
                {
                   int indexOfLureCard = cardsInHand.IndexOf(LureCard.gameObject);//ELimina la carta senuelo de la mano
                   cardsInHand.RemoveAt(indexOfLureCard);
                   cardsInHand.Insert(indexOfLureCard,choosencard.gameObject);
                   if(meleesZones.cardsInMeleeZone.Contains(choosencard.gameObject))//Verifica en que zona se encuentra la carta plata
                   {//Intercambia las posiciones entre la carta senuelo y la carta plata
                     int indexOfChoosenCards = meleesZones.cardsInMeleeZone.IndexOf(choosencard.gameObject);
                     meleesZones.cardsInMeleeZone.RemoveAt(indexOfChoosenCards);
                     meleesZones.cardsInMeleeZone.Insert(indexOfChoosenCards,LureCard.gameObject);
                     choosencard.transform.position = handZones[indexOfLureCard].transform.position;
                     LureCard.transform.position = meleesZones.meleesZones[indexOfChoosenCards].transform.position;
                   }
                   else if(fromDistanceZones.cardsInFromDistanceZones.Contains(choosencard.gameObject))
                   {
                    int indexOfChoosenCards = fromDistanceZones.cardsInFromDistanceZones.IndexOf(choosencard.gameObject);
                    fromDistanceZones.cardsInFromDistanceZones.RemoveAt(indexOfChoosenCards);
                    fromDistanceZones.cardsInFromDistanceZones.Insert(indexOfChoosenCards,LureCard.gameObject);
                    choosencard.transform.position = handZones[indexOfLureCard].transform.position;
                    LureCard.transform.position = fromDistanceZones.fromDistanceZones[indexOfChoosenCards].transform.position;
                   }
                   else if(siegeZones.cardsInSiegeZones.Contains(choosencard.gameObject))
                   {
                    int indexOfChoosenCards = siegeZones.cardsInSiegeZones.IndexOf(choosencard.gameObject);
                    siegeZones.cardsInSiegeZones.RemoveAt(indexOfChoosenCards);
                    siegeZones.cardsInSiegeZones.Insert(indexOfChoosenCards,LureCard.gameObject);
                    choosencard.transform.position = handZones[indexOfLureCard].transform.position;
                    LureCard.transform.position = siegeZones.siegeZones[indexOfChoosenCards].transform.position;
                   }
                }
       }
       public int AmountOfSilverCards()//Para saber cuantas cartas plata tiene el jugador en el campo
       {
         int amountofsilvercards = 0;
         for(int i =0;i<meleesZones.cardsInMeleeZone.Count;i++)
         {//Recorre la zona Cuerpo a cuerpo
            if(meleesZones.cardsInMeleeZone[i] != null)
            {
                Silver SilverComponent = meleesZones.cardsInMeleeZone[i].GetComponent<Silver>();
                if(SilverComponent != null)amountofsilvercards++;
            }
         }
         for(int i =0;i<fromDistanceZones.cardsInFromDistanceZones.Count;i++)
         {//Recorre la zona A Distancia
            if(fromDistanceZones.cardsInFromDistanceZones[i] != null)
            {
                Silver SilverComponent = fromDistanceZones.cardsInFromDistanceZones[i].GetComponent<Silver>();
                if(SilverComponent != null)amountofsilvercards++;
            }
         }
         for(int i =0;i<siegeZones.cardsInSiegeZones.Count;i++)
         {//Recorre la zona Asedio
            if(siegeZones.cardsInSiegeZones[i] != null)
            {
                Silver SilverComponent = siegeZones.cardsInSiegeZones[i].GetComponent<Silver>();
                if(SilverComponent != null)amountofsilvercards++;
            }
         }
         return amountofsilvercards;//Devuelve el total de cartas plata
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