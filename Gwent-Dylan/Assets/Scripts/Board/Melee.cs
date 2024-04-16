using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public List<GameObject> meleesZones = new List<GameObject>();//Las zonas donde pueden ser invocadas las cartas
    public List<GameObject> cardsInMeleeZone = new List<GameObject>();//Las cartas que actualmente estan en la fila
}