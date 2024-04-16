using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siege : MonoBehaviour
{
    public List<GameObject> siegeZones = new List<GameObject>();//Las zonas donde van a ser invocadas las cartas
    public List<GameObject> cardsInSiegeZones = new List<GameObject>();//Las cartas que se encuentran actualmente en la fila
}