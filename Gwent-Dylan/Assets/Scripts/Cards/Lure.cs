using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{
    public string Name;
    public Player player;
    public bool invoked;
    public bool destroyed;
    public void Start()
    {
       player = transform.parent.GetComponent<Player>();
    }
    public void OnMouseDown()
    {
        int amountofsilvercards = player.AmountOfSilverCards();
        if(destroyed)Debug.Log("Ya esta carta fue destruida");
        else if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
          if(amountofsilvercards == 0) Debug.Log("No hay ninguna carta plata para seleccionar");
         else if(!invoked)
         {
           //Aqui quiero que se active el efecto de la carta senuelo
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