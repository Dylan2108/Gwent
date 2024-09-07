using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCard : Card
{
   public int atk;
   public string atkType;
   public Player player;//El jugador que posee la carta 
   public bool invoked;//Para saber si la carta fue convocada
   public bool destroyed;//Para saber si la carta fue destruida
   public bool EffectActivated;//Para saber si el efecto de la carta fue activado    
}