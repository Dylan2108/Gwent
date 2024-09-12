using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public List<GameObject> zones = new List<GameObject>();//Las zonas donde van a ser invocadas las cartas
    public List<GameObject> cardsInZones = new List<GameObject>();//Las cartas que estan actualmente en la fila
}