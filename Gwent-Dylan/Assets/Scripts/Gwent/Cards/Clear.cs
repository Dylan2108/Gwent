using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : SpecialCard
{
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia al jugador que sea su padre
    }
    public void OnMouseDown()
    {
        if(destroyed)Debug.Log("Ya esta carta fue destruida");
        else if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
         if(player.EffectLureIsActive) Debug.Log("Debe seleccionar una carta plata en el campo");
         else if(!invoked)
         {//Invoca la carta y activa su efecto
           player.SummonClearCard(this);
           invoked = true;
           player.playedCards++;
           player.ChangedCards = true;
           if(this.name == "Bijudama")player.EffectClear(1);
           else if(this.name == "Flecha de Indra")player.EffectClear(3);
         }
        }
        else if(!player.isMyTurn)Debug.Log("No es tu turno");
        else if(player.playedCards!=0 && !player.ICanStillSummoning)Debug.Log("No puedes jugar mas de una carta");
    }
}