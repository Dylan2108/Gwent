using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public Player RivalPlayer; //El jugador rival
    public List<GameObject> handZones = new List<GameObject>();//Las zonas de la mano donde van a estar las cartas
    public List<Card> cardsInHand = new List<Card>();//Para saber las cartas que estan en la mano actualmente
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
    public List<Card>WeathersCardsInField = new List<Card>();//Para saber las cartas clima que estan actualmente en el campo
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
    public void SummonUnitCard(UnitCard card)
    {//Invoca la carta Oro
        if(card.atkType == "Melee")
        {
            for(int i = 0;i<emptyMeleeZones.Length;i++)
            {
                if(!emptyMeleeZones[i])
                {
                    card.transform.position = meleesZones.zones[i].transform.position;//Se cambia la posicion de la carta
                    meleesZones.cardsInZones[i] = card;//Se actualizan las filas para saber donde esta cada carta
                    int index = cardsInHand.IndexOf(card);
                    cardsInHand[index] = null; //Se elimina la carta de la mano y se reemplaza por un nuevo espacio vacio 
                    deck.emptyZones[index] = false;
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
                card.transform.position = fromDistanceZones.zones[i].transform.position;
                fromDistanceZones.cardsInZones[i] = card;
                int index = cardsInHand.IndexOf(card);
                cardsInHand[index] = null;
                deck.emptyZones[index] = false;
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
                    card.transform.position = siegeZones.zones[i].transform.position;
                    siegeZones.cardsInZones[i] = card;
                    int index = cardsInHand.IndexOf(card);
                    cardsInHand[index] = null;
                    deck.emptyZones[index]  = false;
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
                card.transform.position = weatherZones.zones[i].transform.position;//Se cambia la posicion de la carta hacia la zona clima
                weatherZones.cardsInZones[i] = card; //Se actualiza la lista de cartas clima
                int index = cardsInHand.IndexOf(card);
                cardsInHand[index] = null; // Se actualizan los espacios en blanco de la mano
                deck.emptyZones[index] = false;
                emptyWeatherZones[i] = true; // Se marcan los espacios de la carta clima como ocupados
                RivalPlayer.emptyWeatherZones[i] = true;
                WeathersCardsInField[i] = card;
                RivalPlayer.WeathersCardsInField[i] = card;
                break;
            }
            else if(i == 2)
            {
                Debug.Log("Se alcanzo el mayor numero de cartas en la zona clima");
            }
        }
    }
    public void ShowMenuSummonBoost(Card card, int increase)// Para mostrar el panel para elegir la zona que queremos afectar 
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
    public void SummonBoostCard(Card card,string row,GameObject summonMenu) //Para invocar cartas Aumento
    {
          if(row == "Melee") //Boton Cuerpo a cuerpo
    {
        if(!emptyMeleeBoost) // Se verifica si es posible invocar la carta
        {
            int index = cardsInHand.IndexOf(card);//Se retira la carta de la mano
            cardsInHand[index] = null;
            deck.emptyZones[index] = false;
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
            cardsInHand[index] = null;
            deck.emptyZones[index] = false;
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
            cardsInHand[index] = null;
            deck.emptyZones.RemoveAt(index);
            deck.emptyZones[index] = false;
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
    public void SummonClearCard(Card card) //Para invocar cartas despeje
    {
        card.transform.position = graveyard.transform.position; //Se lleva al cementerio
        graveyard.cardsInGraveyard.Add(card);                   //como es una carta de efecto rapido
        int index = cardsInHand.IndexOf(card);       //no es necesario llevarla al cementerio
        cardsInHand[index] = null; //Se retira la carta de la mano 
        deck.emptyZones[index] = false;
    }
    public int CountZonePoints(Zone zone) //Para sumar el ataque de las cartas
    {                             //de la zona Cuerpo a cuerpo
        int counter = 0;
      for(int i = 0;i<zone.cardsInZones.Count;i++)
      {
        if(zone.cardsInZones[i] != null)
        {
         Card card = zone.cardsInZones[i];
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
    public int CountTotalPoints()//Para sumar el ataque de todas las zonas
    {
        int meleepoints = CountZonePoints(meleesZones);//Los puntos de Cuerpo a cuerpo
        int adistancepoints = CountZonePoints(fromDistanceZones);//Los puntos de A Distancia
        int asediopoints = CountZonePoints(siegeZones);//Los puntos de ASedio
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
    public void CleanZone(Zone zone,bool[] emptyZones,Card boostCard,bool emptyBoost)//Vacia una zona
    {
        for(int i =0;i<zone.cardsInZones.Count;i++)
        {
            if(zone.cardsInZones[i]!=null)
            {//Envia todas las cartas de la zona al cementerio
                zone.cardsInZones[i].transform.position = graveyard.transform.position;
                graveyard.cardsInGraveyard.Add(zone.cardsInZones[i]);
                zone.cardsInZones[i] = null;
            }
        }
        for(int i =0;i<emptyZones.Length;i++)
        {//Pone todos los espacios en blanco
            emptyZones[i] = false;
        }
        if(boostCard != null)
        {//Se envia la carta aumento al cementerio
            boostCard.transform.position = graveyard.transform.position;
            boostCard = null;
            emptyBoost = false;
        }
    }
    public void CleanBoard()//Se encarga de vaciar el campo
    {
        CleanZone(meleesZones,emptyMeleeZones,meleesZones.cardInMeleeBoostZone,emptyMeleeBoost);
        CleanZone(fromDistanceZones,emptyFromDistanceZones,fromDistanceZones.cardInFromDistanceBoostZone,emptyFromDistanceBoost);
        CleanZone(siegeZones,emptySiegeZones,siegeZones.cardInSiegeBoostZone,emptySiegeBoost);
       for(int i = 0;i<weatherZones.cardsInZones.Count;i++)//Zona Clima
       {
          if(weatherZones.cardsInZones[i] != null)
          {//Envia al cementerio todas las cartas Clima
           weatherZones.cardsInZones[i].transform.position = graveyard.transform.position;
           graveyard.cardsInGraveyard.Add(weatherZones.cardsInZones[i]);
           weatherZones.cardsInZones[i] = null;
          }
          if(WeathersCardsInField[i] != null) WeathersCardsInField[i] = null;
       }
       for(int i =0;i<emptyWeatherZones.Length;i++)
       {//Pone todos los espacios en blanco
        emptyWeatherZones[i] = false;
       }
    }
    //Efectos
    public void DecreaseZone(Zone zone,Zone rivalZone,string row,int decrease,GameObject EffectMenu,Weather cardWeather)
    {
        for(int i =0;i<zone.cardsInZones.Count;i++)
        {//Recorre la fila
            if(zone.cardsInZones[i] != null)
            {//Le disminuye el ataque a las cartas plata en la fila
               Card card = zone.cardsInZones[i];
               Silver SilverComponent = card.GetComponent<Silver>();
               if(SilverComponent != null)
               {
                 SilverComponent.atk -= decrease;
                 cardWeather.affectedCards.Add(SilverComponent);
               }
            }
        }
        for(int i =0;i<rivalZone.cardsInZones.Count;i++)
        {//Recorre la fila del rival
            if(rivalZone.cardsInZones[i]!=null)
            {//Se disminuye el ataque a las cartas plata en la fila
                Card card = rivalZone.cardsInZones[i];
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
       public void EffectWeather(string row,int decrease,GameObject EffectMenu,Weather cardWeather)//Cartas clima
       {
         if(row == "Melee")
         {
            DecreaseZone(meleesZones,RivalPlayer.meleesZones,row,decrease,EffectMenu,cardWeather);
         }
         else if(row == "From Distance")
         {
            DecreaseZone(fromDistanceZones,RivalPlayer.fromDistanceZones,row,decrease,EffectMenu,cardWeather);
         }
         else if(row == "Siege")
         {
            DecreaseZone(siegeZones,RivalPlayer.siegeZones,row,decrease,EffectMenu,cardWeather);
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
       public void IncrementZone(Zone zone,string row,int increase)
       {
          for(int i = 0;i<zone.cardsInZones.Count;i++)
          {//Recorre la fila del jugador
             if(zone.cardsInZones[i]!= null)
             {//Aumenta el ataque de las cartas de la fila
                Card card = zone.cardsInZones[i];
                Silver SilverComponent = card.GetComponent<Silver>();
                if(SilverComponent != null) SilverComponent.atk +=increase;
             }
          }
          UpdateScore();
       }
       public void EffectBoost(string row,int increase)//Cartas aumento
       {
          if(row == "Melee")
          {
            IncrementZone(meleesZones,row,increase);
          }
          else if(row == "From Distance")
          {
            IncrementZone(fromDistanceZones,row,increase);
          }
          else if(row == "Siege")
          {
            IncrementZone(siegeZones,row,increase);
          }
       }
       public void EliminateWeatherCards(Zone zone)
       {
          for(int i =0;i<zone.cardsInZones.Count;i++)
          {//Se recorre la zona clima del jugador
             if(zone.cardsInZones[i]!=null)
             {//Se eliminan las cartas clima
                 zone.cardsInZones[i].transform.position = graveyard.transform.position;
                 int index = WeathersCardsInField.IndexOf(zone.cardsInZones[i]);
                 WeathersCardsInField[index] = null;
                 RivalPlayer.WeathersCardsInField[index] = null;
                 graveyard.cardsInGraveyard.Add(zone.cardsInZones[i]);
                 Weather WeatherComponent = zone.cardsInZones[i].GetComponent<Weather>();
                 zone.cardsInZones[i] = null;
                 if(WeatherComponent.name == "Kirin")//Se les devuelve el ataque a las cartas afectadas
                 {
                    for(int j =0;j<WeatherComponent.affectedCards.Count;j++)
                    {
                        WeatherComponent.affectedCards[j].atk += 2;
                    }
                    WeatherComponent.affectedCards.Clear();
                 }
                 else if(WeatherComponent.name == "Jutsu Bola de Fuego")
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
       public void EffectClear(int cardsToClear)//Efecto despeje
       {
          if(cardsToClear == 1)//Verifica la cantidad de cartas a eliminar
          {
            for(int i =0;i<RivalPlayer.weatherZones.cardsInZones.Count;i++)
            {//Recorre la Zona Clima del rival
                if(RivalPlayer.weatherZones.cardsInZones[i] != null)
                {//Se destruyen las cartas clima
                    RivalPlayer.weatherZones.cardsInZones[i].transform.position = RivalPlayer.graveyard.transform.position;
                    int index = RivalPlayer.WeathersCardsInField.IndexOf(RivalPlayer.weatherZones.cardsInZones[i]);
                    RivalPlayer.WeathersCardsInField[index] = null;
                    WeathersCardsInField[index] = null;
                    RivalPlayer.graveyard.cardsInGraveyard.Add(RivalPlayer.weatherZones.cardsInZones[i]);
                    Weather WeatherComponent = RivalPlayer.weatherZones.cardsInZones[i].GetComponent<Weather>();
                    RivalPlayer.weatherZones.cardsInZones[i] = null;
                    if(WeatherComponent.name == "Kirin")//Se le devuelve el ataque quitado a las cartas afectadas
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
                    else if(WeatherComponent.name == "Jutsu Bola de Fuego")
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
            for(int i = 0 ;i<weatherZones.cardsInZones.Count;i++)
            {//Se recorre la zona clima del jugador
                if(weatherZones.cardsInZones[i] != null)
                {//Se eliminan las cartas climas
                    weatherZones.cardsInZones[i].transform.position = graveyard.transform.position;
                    int index = WeathersCardsInField.IndexOf(weatherZones.cardsInZones[i]);
                    WeathersCardsInField[index] = null;
                    RivalPlayer.WeathersCardsInField[index] = null;
                    graveyard.cardsInGraveyard.Add(weatherZones.cardsInZones[i]);
                    Weather WeatherComponent = weatherZones.cardsInZones[i].GetComponent<Weather>();
                    weatherZones.cardsInZones[i] = null;
                    if(WeatherComponent.name == "Kirin")//Se le devuelve el ataque a las cartas modificadas
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
                    else if(WeatherComponent.name == "Jutsu Bola de Fuego")
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
            EliminateWeatherCards(weatherZones);
            EliminateWeatherCards(RivalPlayer.weatherZones);
          }
          UpdateScore();
          RivalPlayer.UpdateScore();
       }
       public void FindSilverCard(Zone zone,Silver choosenCard,Lure lureCard,int indexOfLureCard)
       {
           if(zone.cardsInZones.Contains(choosenCard))//Verifica en que zona se encuentra la carta plata
           {//Intercambia las posiciones entre la carta senuelo y la carta plata
               int indexOfChoosenCards = zone.cardsInZones.IndexOf(choosenCard);
               zone.cardsInZones[indexOfChoosenCards] = lureCard;
               choosenCard.transform.position = handZones[indexOfLureCard].transform.position;
               lureCard.transform.position = zone.zones[indexOfChoosenCards].transform.position;
               choosenCard.invoked = false;           //Al volver la carta a la mano se permite volver a invocarla
               choosenCard.EffectActivated = false;   //y activar su efecto
           }
       }
       public void EffectLure(Silver choosenCard,Lure LureCard)//Efecto cartas senuelo
       {
                if(choosenCard != null)
                {
                   int indexOfLureCard = cardsInHand.IndexOf(LureCard);//ELimina la carta senuelo de la mano
                   cardsInHand[indexOfLureCard] = choosenCard;
                   FindSilverCard(meleesZones,choosenCard,LureCard,indexOfLureCard);
                   FindSilverCard(fromDistanceZones,choosenCard,LureCard,indexOfLureCard);
                   FindSilverCard(siegeZones,choosenCard,LureCard,indexOfLureCard);
                }
       }
       public void CountSilverCards(Zone zone,int amountofsilvercards)
       {
           for(int i =0;i<zone.cardsInZones.Count;i++)
           {
             if(zone.cardsInZones[i]!=null)
             {
                Silver SilverComponent = zone.cardsInZones[i].GetComponent<Silver>();
                if(SilverComponent != null) amountofsilvercards++; 
             }
           }
       }
       public int AmountOfSilverCards()//Para saber cuantas cartas plata tiene el jugador en el campo
       {
         int amountofsilvercards = 0;
         CountSilverCards(meleesZones,amountofsilvercards);
         CountSilverCards(fromDistanceZones,amountofsilvercards);
         CountSilverCards(siegeZones,amountofsilvercards);
         return amountofsilvercards;//Devuelve el total de cartas plata
       } 
       //Efectos Carta Unidad
       public void EliminateCardLessAtk()//Efecto que elimina la carta con menos atk del rival
       {
        Card CardToEliminate = null;//La carta que va a ser eliminada
        int lessAtk = int.MaxValue;//La variable que va a guardar el menor ataque de todas las cartas
        for(int i = 0;i<RivalPlayer.meleesZones.cardsInZones.Count;i++)
        {//Recorre la zona Cuerpo a cuerpo del rival
            if(RivalPlayer.meleesZones.cardsInZones[i] != null)
            {
                Card card = RivalPlayer.meleesZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();//Se verifica si la carta es de oro o plata
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
               {
                  if(GoldComponent.atk < lessAtk)
                  {
                       lessAtk = GoldComponent.atk;//Se actualiza el menor ataque y  
                       CardToEliminate = card;     //la carta que va a ser eliminada 
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
        }
        for(int i = 0;i<RivalPlayer.fromDistanceZones.cardsInZones.Count;i++)
        {//Se recorre la fila A Distancia del rival
            if(RivalPlayer.fromDistanceZones.cardsInZones[i] !=null)
            {
                Card card = RivalPlayer.fromDistanceZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();//Se verifica si la carta es de oro o plata
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
               {
                   if(GoldComponent.atk < lessAtk)
                   {
                     lessAtk = GoldComponent.atk;//Se actualiza el menor ataque y
                     CardToEliminate = card;     //la carta que va a ser eliminada
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
        }
        for(int i = 0 ;i<RivalPlayer.siegeZones.cardsInZones.Count;i++)
        {//Se recorre la fila Asedio del rival
            if(RivalPlayer.siegeZones.cardsInZones[i] !=null)
            {
                Card card = RivalPlayer.siegeZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();//Se verifica si la carta es de oro o plata
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
               {
                  if(GoldComponent.atk < lessAtk)
                   {
                    lessAtk = GoldComponent.atk;//Se actualiza el menor ataque y
                    CardToEliminate = card;     //la carta que va a ser eliminada
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
        }
        if(CardToEliminate != null)
        {
          CardToEliminate.transform.position = RivalPlayer.graveyard.transform.position;//Se envia la carta eliminada al cementerio
          RivalPlayer.graveyard.cardsInGraveyard.Add(CardToEliminate);
          if(RivalPlayer.meleesZones.cardsInZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.meleesZones.cardsInZones.IndexOf(CardToEliminate);
            RivalPlayer.meleesZones.cardsInZones[index] = null;
            emptyMeleeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
          else if(RivalPlayer.fromDistanceZones.cardsInZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.fromDistanceZones.cardsInZones.IndexOf(CardToEliminate);
            RivalPlayer.fromDistanceZones.cardsInZones[index] = null;
            emptyFromDistanceZones[index] = false;
            RivalPlayer.UpdateScore(); 
          }
          else if(RivalPlayer.siegeZones.cardsInZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.siegeZones.cardsInZones.IndexOf(CardToEliminate);
            RivalPlayer.siegeZones.cardsInZones[index] = null;
            emptySiegeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
        }
       }
       public void EliminateCardHigherAtk()//Efecto que elimina la carta con mas atk del rival
       {
         Card CardToEliminate = null;//La carta que va a ser eliminada
         int higheratk = int.MinValue;//La variable que va a guardar el menor ataque de todas las cartas
         for(int i = 0;i<RivalPlayer.meleesZones.cardsInZones.Count;i++)
        {//Recorre la zona Cuerpo a cuerpo del rival
          if(RivalPlayer.meleesZones.cardsInZones[i] != null)
          {
            Card card = RivalPlayer.meleesZones.cardsInZones[i];
            Gold GoldComponent = card.GetComponent<Gold>();//Se verifica si la carta es de oro o plata
            Silver SilverComponent = card.GetComponent<Silver>();
             if(GoldComponent != null)
            {
              if(GoldComponent.atk > higheratk)
              {
                higheratk = GoldComponent.atk;//Se actualiza el mayor ataque y
                CardToEliminate = card;       //la carta que va a ser eliminada
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
        }
        for(int i = 0;i<RivalPlayer.fromDistanceZones.cardsInZones.Count;i++)
        {//Recore la zona A Distancia del rival
            if(RivalPlayer.fromDistanceZones.cardsInZones[i] != null)
            {
                Card card = RivalPlayer.fromDistanceZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();//Se verfica si la carta es de oro o plata
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)
                {
                 if(GoldComponent.atk > higheratk)
                  {
                    higheratk = GoldComponent.atk;//Se acualiza el mayor ataque y
                    CardToEliminate = card;       //la carta que va a ser eliminada
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
        }
        for(int i = 0 ;i<RivalPlayer.siegeZones.cardsInZones.Count;i++)
        {//Recorre la zona Asedio del rival
            if(RivalPlayer.siegeZones.cardsInZones[i] != null)
            {
                 Card card = RivalPlayer.siegeZones.cardsInZones[i];
                 Gold GoldComponent = card.GetComponent<Gold>();//Se verifica si la carta es de oro  o plata 
                 Silver SilverComponent = card.GetComponent<Silver>();
                 if(GoldComponent != null)
               {
                   if(GoldComponent.atk > higheratk)
                   {
                    higheratk = GoldComponent.atk;//Se actualiza el mayor ataque y
                    CardToEliminate = card;       //la carta que va a ser eliminada
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
        }
        if(CardToEliminate != null)
        {
          CardToEliminate.transform.position = RivalPlayer.graveyard.transform.position;//Se envia la carta eliminada al cementerio
          RivalPlayer.graveyard.cardsInGraveyard.Add(CardToEliminate);
          if(RivalPlayer.meleesZones.cardsInZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.meleesZones.cardsInZones.IndexOf(CardToEliminate);
            RivalPlayer.meleesZones.cardsInZones[index] = null;
            emptyMeleeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
          else if(RivalPlayer.fromDistanceZones.cardsInZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.fromDistanceZones.cardsInZones.IndexOf(CardToEliminate);
            RivalPlayer.fromDistanceZones.cardsInZones[index] = null;
            emptyFromDistanceZones[index] = false;
            RivalPlayer.UpdateScore(); 
          }
          else if(RivalPlayer.siegeZones.cardsInZones.Contains(CardToEliminate))
          {
            int index = RivalPlayer.siegeZones.cardsInZones.IndexOf(CardToEliminate);
            RivalPlayer.siegeZones.cardsInZones[index] = null;
            emptySiegeZones[index] = false;
            RivalPlayer.UpdateScore();
          }
        }
       }
       public void InvokeWeatherCardEffect()//Permite invocar una carta clima que este en la mano
       {
        for(int i = 0;i<cardsInHand.Count;i++)
        {//Verifica si hay alguna carta clima en la mano
            if(cardsInHand[i] != null)
            {
             Card card = cardsInHand[i];
             Weather WeatherComponent = card.GetComponent<Weather>();
             if(WeatherComponent != null)
             {
                SummonWeatherCard(WeatherComponent);//Invoca la carta clima
                WeatherComponent.invoked = true;
                if(WeatherComponent.name == "Kirin")
               {
                 ShowMenuWeatherEffect(WeatherComponent,2);//Activa el efecto de la carta clima
               }
               else if(WeatherComponent.name == "Jutsu Bola de Fuego")
               {
                 ShowMenuWeatherEffect(WeatherComponent,1);
               }
               return;
             }
            else if(i == cardsInHand.Count - 1) Debug.Log("No hay cartas clima en la mano");
            }
        }
       }
       public void InvokeBoostCardEffect(string row)//Permite invocar una carta aumento de la mano
       {
          for(int i = 0;i<cardsInHand.Count;i++)
          {//Verifica si hay alguna carta aumento en la mano
            if(cardsInHand[i] != null)
            {
                Card card = cardsInHand[i];
                Boost BoostComponent = card.GetComponent<Boost>();
                if(BoostComponent != null)
                {
                    SummonBoostCard(BoostComponent,row,null);//Invoca la carta aumento
                    BoostComponent.invoked = true;
                    if(BoostComponent.name == "Pildoras Ninjas") 
                    {
                       EffectBoost(row,1); //Activa el efecto de la carta aumento
                    }
                    else if(BoostComponent.name == "Jutsu de la Alianza Shinobi")
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
          int totalcards = 0; //El numero de cartas de oro y plata totales en el campo
          int totalatk = 0; //El ataque total de todas las cartas oro y plata  en el campo
          for(int i = 0;i < meleesZones.cardsInZones.Count;i++)
          {
            if(meleesZones.cardsInZones[i] != null)
            {//Se recorre la fila Cuerpo a cuerpo
                Card card = meleesZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)//Se actualiza la cantidad de cartas y el ataque total 
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
          for(int i = 0;i<fromDistanceZones.cardsInZones.Count;i++)
          {//Se recorre la fila A Distancia
            if(fromDistanceZones.cardsInZones[i] != null)
            {
                Card card = fromDistanceZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)//Se actualiza la cantidad de cartas y el ataque total
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
          for(int i = 0;i<siegeZones.cardsInZones.Count;i++)
          {//Se recorre la fila Asedio
            if(siegeZones.cardsInZones[i] != null)
            {
                Card card = siegeZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)//Se actualiza la cantidad de cartas y el ataque total
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
          for(int i = 0;i<RivalPlayer.meleesZones.cardsInZones.Count;i++)
          {
            if(RivalPlayer.meleesZones.cardsInZones[i] != null)
            {//Recorre la fila Cuerpo a cuerpo del rival
                Card card = RivalPlayer.meleesZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)//Se actualiza la cantidad de cartas y el ataque total
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
          for(int i = 0; i<RivalPlayer.fromDistanceZones.cardsInZones.Count;i++)
          {//Recorre la fila A Distancia del rival
            if(RivalPlayer.fromDistanceZones.cardsInZones[i] != null)
            {
                Card card = RivalPlayer.fromDistanceZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)//Se actualiza la cantidad de cartas y el ataque total
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
          for(int i =0;i<RivalPlayer.siegeZones.cardsInZones.Count;i++)
          {//Recore la fila Asedio del rival
            if(RivalPlayer.siegeZones.cardsInZones[i] != null)
            {
                Card card = RivalPlayer.siegeZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null)//Se actualiza la cantidad de cartas y el ataque total
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
         int prom = CalculateProm();//El promedio de las cartas en el campo(Redondeado)
         for(int i = 0 ;i<meleesZones.cardsInZones.Count;i++)
         {//Recorre la fila Cuerpo a cuerpo
            if(meleesZones.cardsInZones[i] != null)
            {//Iguala el ataque de cada carta al promedio
                Card card = meleesZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null) GoldComponent.atk = prom;
                else if(SilverComponent != null) SilverComponent.atk = prom;
            }
         }
         for(int i =0;i<fromDistanceZones.cardsInZones.Count;i++)
         {//Recorre la fila A Distancia
            if(fromDistanceZones.cardsInZones[i] != null)
            {//Iguala el ataque de cada  carta al promedio
                Card card = fromDistanceZones.cardsInZones[i];
                Gold GoldComponent = card.GetComponent<Gold>();
                Silver SilverComponent = card.GetComponent<Silver>();
                if(GoldComponent != null) GoldComponent.atk = prom;
                else if(SilverComponent != null) SilverComponent.atk = prom;
            }
         }
         for(int i =0;i<siegeZones.cardsInZones.Count;i++)
         {//Recorre la fila Asedio
            if(siegeZones.cardsInZones[i] != null)
            {//Iguala el ataque de cada carta al promedio
                Card card = siegeZones.cardsInZones[i];
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
          Card prefabCard = deck.deck[randomIndex];
          deck.deck.RemoveAt(randomIndex);
           for(int i = 0;i<deck.emptyZones.Count;i++)
           {
              if(!deck.emptyZones[i])//Verifica si en esa zona puede robarse una carta
             {
               Card instantiatedCard = Instantiate(prefabCard,handZones[i].transform.position,Quaternion.identity);
               instantiatedCard.transform.SetParent(this.transform);//Establece a la mano como padre de la carta
                cardsInHand[i] = instantiatedCard;//Actualiza la lista para saber que cartas tenemos en la mano actualmente
               deck.emptyZones[i] = true;//ACtualiza la mano para saber que esa zona esta ocupada
               break;
             }
              else if(i==9)
             {
               Card instantiatedCard = Instantiate(prefabCard,graveyard.transform.position,Quaternion.identity);
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
       {//Se escoge la fila con menos cartas del rival
        int min = Mathf.Min(RivalPlayer.meleesZones.cardsInZones.Count,Mathf.Min(RivalPlayer.fromDistanceZones.cardsInZones.Count,RivalPlayer.siegeZones.cardsInZones.Count));
        if(min == RivalPlayer.meleesZones.cardsInZones.Count && min != 0)
        {//Verifica si la fila con menos cartas es la Cuerpo a cuerpo
            for(int i = 0;i<RivalPlayer.meleesZones.cardsInZones.Count;i++)
            {
                if(RivalPlayer.meleesZones.cardsInZones[i] != null)
                {//Envia las cartas de la fila al cementerio
                    Card card = RivalPlayer.meleesZones.cardsInZones[i];
                    card.transform.position = graveyard.transform.position;
                    graveyard.cardsInGraveyard.Add(card);
                    card = null;
                }
            }
            for(int i = 0 ;i<RivalPlayer.emptyMeleeZones.Length;i++)
            {//Deja todos los espacios en blanco
                RivalPlayer.emptyMeleeZones[i] = false;
            }
            if(RivalPlayer.meleesZones.cardInMeleeBoostZone != null)
            {//Envia al cementerio la carta Aumento
                RivalPlayer.meleesZones.cardInMeleeBoostZone.transform.position = graveyard.transform.position;
                RivalPlayer.meleesZones.cardInMeleeBoostZone = null;
                RivalPlayer.emptyMeleeBoost = false;
            }
        }
        else if(min == RivalPlayer.fromDistanceZones.cardsInZones.Count && min !=0)
        {//Verifica si la fila con menos cartas es la de A Distancia
            for(int i = 0;i<RivalPlayer.fromDistanceZones.cardsInZones.Count;i++)
            {
                if(RivalPlayer.fromDistanceZones.cardsInZones[i] != null)
                {//Envia las cartas de la fila al cementerio
                    Card card = RivalPlayer.fromDistanceZones.cardsInZones[i];
                    card.transform.position = graveyard.transform.position;
                    graveyard.cardsInGraveyard.Add(card);
                    card = null;
                }
            }
            for(int i = 0 ;i<RivalPlayer.emptyFromDistanceZones.Length;i++)
            {//Deja todos los espacios en blanco
                RivalPlayer.emptyFromDistanceZones[i] = false;
            }
            if(RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone != null)
            {//Envia al cementerio la  carta Aumento  
                RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone.transform.position = graveyard.transform.position;
                RivalPlayer.fromDistanceZones.cardInFromDistanceBoostZone = null;
                RivalPlayer.emptyFromDistanceBoost = false;
            }
        }
        else if(min == RivalPlayer.siegeZones.cardsInZones.Count && min != 0)
        {//Verifica si la fila con menos cartas es la de Asedio
            for(int i = 0;i<RivalPlayer.siegeZones.cardsInZones.Count;i++)
            {
                if(RivalPlayer.siegeZones.cardsInZones[i] != null)
                {//Envia todas las cartas de la fila al cementerio
                    Card card = RivalPlayer.siegeZones.cardsInZones[i];
                    card.transform.position = graveyard.transform.position;
                    graveyard.cardsInGraveyard.Add(card);
                    card = null;
                }
            }
            for(int i =0;i<RivalPlayer.emptySiegeZones.Length;i++)
            {//Deja todos los espacios en blanco
                RivalPlayer.emptySiegeZones[i] = false;
            }
            if(RivalPlayer.siegeZones.cardInSiegeBoostZone != null)
            {//Envia al cementerio la carta Aumnento
                RivalPlayer.siegeZones.cardInSiegeBoostZone.transform.position = graveyard.transform.position;
                RivalPlayer.siegeZones.cardInSiegeBoostZone = null;
                RivalPlayer.emptySiegeBoost = false;
            }
        }
        RivalPlayer.UpdateScore();
    } 
}