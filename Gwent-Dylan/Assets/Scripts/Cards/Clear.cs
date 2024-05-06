using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    public string Name;
    public Player player;
    public bool invoked;
    public bool EffectActivated;
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();
    }
    public void OnMouseDown()
    {
        if((player.isMyTurn && player.playedCards==0) || player.ICanStillSummoning)
        {
         if(!invoked)
         {
           player.ShowMenuSummonClear(this.gameObject);
           invoked = true;
           player.playedCards++;
         }
         else if(invoked && ! EffectActivated)
         {
            player.EffectClear(3);
            EffectActivated = true;
         }
         else
         {
            Debug.Log("Ya el efecto de esta carta fue activado");
         }
        }
        else if(!player.isMyTurn)
        {
            Debug.Log("No es tu turno");
        }
        else if(player.playedCards!=0 && !player.ICanStillSummoning)
        {
            Debug.Log("No puedes jugar mas de una carta");
        }
    }
}