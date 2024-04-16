using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromDistance : MonoBehaviour
{
   public List<GameObject> fromDistanceZones = new List<GameObject>();//Las zonas donde van a ser invocadas las cartas
   public List<GameObject> cardsInFromDistanceZones = new List<GameObject>();//Las cartas que estan actualmente en la fila
}