using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silver : MonoBehaviour
{//Cartas Plata
    public string name;
    public int atk;
    public string atkType;
    public Player player;
    public bool invoked;
    public void Start()
    {
        player = transform.parent.GetComponent<Player>();//Toma como referencia a la mano que sea su padre
    }
    public void OnMouseDown()
    {
      if((player.isMyTurn && player.playedCards == 0) || player.ICanStillSummoning)
        {
         if(!invoked)
         {
           player.SummonSilverCard(this);
           invoked = true;
           player.playedCards++;
         }
         else
         {
            Debug.Log("Ya esta carta esta invocada");
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