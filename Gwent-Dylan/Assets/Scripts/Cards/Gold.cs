using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{//Carta de Oro
    public string name;
    public int originalAtk;
    public int atk;
    public string atkType;
    public Player player;
    public bool invoked;
    public bool EffectActivated;
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia a la mano que sea su padre
        originalAtk = atk;
    }
    public void OnMouseDown()
    {
        if((player.isMyTurn && player.playedCards == 0) || player.ICanStillSummoning)
        {
         if(!invoked)
         {
           player.SummonGoldCard(this);
           invoked = true;
           player.playedCards++;
         }
         else if(invoked && !EffectActivated)
         {
            if(this.name == "Itachi Uchiha")
            {
                player.InvokeBoostCardEffect("Melee");
                EffectActivated = true;
            }
            else if(this.name == "Jiraiya")
            {
                player.EliminateCardLessAtk();
                EffectActivated = true;
            }
            else if(this.name == "Might Guy")
            {
                player.InvokeWeatherCardEffect();
                EffectActivated = true;
            }
            else if(this.name == "Orochimaru")
            {
                player.InvokeBoostCardEffect("Siege");
                EffectActivated = true;
            }
            else if(this.name == "Madara Uchiha")
            {
                player.EffectProm();
                EffectActivated = true;
            }
            else if(this.name == "Kakashi Hatake" || this.name == "Tsunade")
            {
                player.EffectDrawCard();
                EffectActivated =true;
            }
            else if(this.name == "Minato Namikaze")
            {
                player.EliminateCardHigherAtk();
                EffectActivated = true;
            }
            else if(this.name == "Sasuke Uchiha")
            {
                player.EliminateRow();
                EffectActivated = true;
            }
         }
         else
         {
            Debug.Log("Ya esta carta esta convocada");
         }
        }
        else if(!player.isMyTurn)
        {
            Debug.Log("No es tu turno");
        }
        else if(player.playedCards!=0 && !player.ICanStillSummoning)
        {
            Debug.Log("Ya jugaste una carta este turno");
        }
    }
}