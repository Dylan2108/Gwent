using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
  public string name {get;}   
}
public class UnitCard : Card
{
    public int Atk {get; set;}
    public int OriginalAtk {get;}
    public string AtkType {get;}
}
public class SilverCard : UnitCard
{
    public SilverCard (string name,int OriginalAtk, string AtkType)
    {
        this.name = name;
        this.OriginalAtk = OriginalAtk;
        this.Atk = OriginalAtk;
        this.AtkType = AtkType;
    }
}
public class GoldCard : UnitCard
{
    public GoldCard (string name, int OriginalAtk, string AtkType)
    {
        this.name = name;
        this.OriginalAtk = OriginalAtk;
        this.Atk = OriginalAtk;
        this.AtkType = AtkType;
    }
}