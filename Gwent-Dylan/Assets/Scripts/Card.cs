using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
  public string Name {get; set;}
  public Card(string name)
  {
    Name = name;
  }  
}
public class Leader : Card //Cartas
{
    public Leader(string name) : base(name){}
}
public class Weather : Card //Cartas clima
{
    public Weather(string name) : base(name){}
}
public class Lure : Card //Cartas sennuelo
{
    public Lure(string name) : base(name){}
}
public class Clear : Card //Cartas despeje
{
    public Clear(string name) : base(name){}
}
public class Boost : Card //Cartas aumento
{
    public Boost(string name) : base(name){}
}
public class UnitCard : Card //Cartas Unidad
{
    public int Atk {get; set;}
    public string AtkType {get; set;}
    public UnitCard(string name ,int atk , string atktype) : base(name)
    {
        Atk = atk;
        AtkType = atktype;
    }
}
public class SilverCard : UnitCard //Cartas Plata
{
    public SilverCard (string name,int atk, string atktype) : base(name,atk,atktype){}
}
public class GoldCard : UnitCard // Cartas Oro
{
    public GoldCard (string name, int atk, string atktype) : base(name,atk,atktype){}
}