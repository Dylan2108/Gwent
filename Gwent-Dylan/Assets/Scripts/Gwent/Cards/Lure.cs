using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{
    public string Name;//Nombre
    public Player player;//El jugador que posee la carta
    public bool invoked;//Para saber si la carta fue invocada
    public bool destroyed;//Para saber si la carta fue destruida
    public void Start()
    {
       player = transform.parent.GetComponent<Player>();//Toma como referencia al jugador que la posee
    }
    public void OnMouseDown()
    {
        int amountofsilvercards = player.AmountOfSilverCards();
        if(destroyed)Debug.Log("Ya esta carta fue destruida");
        else if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
          if(amountofsilvercards == 0) Debug.Log("No hay ninguna carta plata para seleccionar");
         else if(!invoked)
         {//Para comenzar a intentar activar el efecto de la carta
           Debug.Log("Seleccione una carta plata en el campo");
           player.EffectLureIsActive = true;
           invoked = true;
           player.ChangedCards = true;
         }
         else
         {
            Debug.Log("Ya esta carta esta invocada");
         }
        }
        else if(!player.isMyTurn)Debug.Log("No es tu turno");
        else if(player.playedCards!=0 && !player.ICanStillSummoning)Debug.Log("Ya jugaste una carta");
    }
}