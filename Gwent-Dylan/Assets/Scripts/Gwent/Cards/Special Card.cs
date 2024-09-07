using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCard : Card
{
   public Player player;//El jugador que posee la carta
   public bool invoked;//Para saber si la carta fue convocada
   public bool destroyed;//Para saber si la carta fue destruida  
}