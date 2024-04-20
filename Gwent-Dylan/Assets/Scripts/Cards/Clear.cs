using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clear : MonoBehaviour
{
    public string Name;
    public Hand hand;
    public void Start()
    {
        hand = transform.parent.GetComponent<Hand>();
    }
    public void OnMouseDown()
    {
        hand.ShowMenuSummonClear(this.gameObject);
    }
}