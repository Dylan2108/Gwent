using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{
    public string Name;
    public Hand hand;
    public bool invoked;
    public void Start()
    {
       hand = transform.parent.GetComponent<Hand>();
    }
    public void OnMouseDown()
    {
        if(hand.isMyTurn && hand.playedCards==0)
        {
         if(!invoked)
         {
           hand.ShowMenuSummonLure(this.gameObject);
           invoked = true;
           hand.playedCards++;
         }
         else
         {
            Debug.Log("Ya esta carta esta invocada");
         }
        }
        else if(!hand.isMyTurn)
        {
            Debug.Log("No es tu turno");
        }
        else if(hand.playedCards!=0)
        {
            Debug.Log("Ya jugaste una carta");
        }
    }
}