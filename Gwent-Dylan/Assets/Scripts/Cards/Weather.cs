using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
   public string Name;//Nombre
   public Player player;//El jugador que posee la carta
   public bool invoked;//Para saber si la carta fue invocada
   public bool destroyed;//Para saber si la carta fue destruida
   public List<Silver> affectedCards = new List<Silver>();//Lista que contiene las cartas a las cuales les fue modificado su ataque
   public void Start()
   {
      player = transform.parent.GetComponent<Player>();//Toma como referencia al padre de la carta
   }
   public void OnMouseDown()
   {
      if(destroyed) Debug.Log("Esta carta ya fue destruida");
     else if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
         if(player.EffectLureIsActive) Debug.Log("Debe seleccionar una carta plata en el campo");
         else if(!invoked)
         {//Invoca la carta y activa el efecto
           player.SummonWeatherCard(this);
           invoked = true;
           player.playedCards++;
           player.ChangedCards = true;
            if(this.Name == "Kirin")player.ShowMenuWeatherEffect(this,2);
            else if(this.Name == "Jutsu Bola de Fuego")player.ShowMenuWeatherEffect(this,1);
         }
         else
         {
            Debug.Log("Ya el efecto de esta carta fue activado");
         }
        }
        else if(!player.isMyTurn)Debug.Log("No es tu turno");
        else if(player.playedCards!=0 && !player.ICanStillSummoning)Debug.Log("Ya invocaste una carta este turno");
   }
}