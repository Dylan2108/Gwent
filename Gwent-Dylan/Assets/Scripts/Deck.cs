using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    void Awake()
    {
        deck.Add(new Leader("Naruto Uzumaki"));
        deck.Add(new SilverCard("Hinata Hyuga",3,"Asedio"));
        deck.Add(new SilverCard("Hinata Hyuga",3,"Asedio"));
        deck.Add(new SilverCard("Sakura Haruno",3,"Cuerpo a cuerpo"));
        deck.Add(new SilverCard("Sakura Haruno",3,"Cuerpo a cuerpo"));
        deck.Add(new SilverCard("Rock Lee",4,"Cuerpo a Cuerpo"));
        deck.Add(new SilverCard("Rock Lee",4,"Cuerpo a Cuerpo"));
        deck.Add(new SilverCard("Gaara",4,"Asedio"));
        deck.Add(new SilverCard("Gaara",4,"Asedio"));
        deck.Add(new GoldCard("Madara Uchiha",10,"Asedio"));
        deck.Add(new GoldCard("Sasuke Uchicha",9,"Cuerpo a cuerpo"));
        deck.Add(new GoldCard("Itachi Uchiha",8,"Cuerpo a cuerpo"));
        deck.Add(new GoldCard("Minato Namikaze",8,"Cuerpo a cuerpo"));
        deck.Add(new GoldCard("Kakashi Hatake",7,"Asedio"));
        deck.Add(new GoldCard("Might Guy",7,"Cuerpo a cuerpo"));
        deck.Add(new GoldCard("Jiraiya",6,"Asedio"));
        deck.Add(new GoldCard("Orochimaru",6,"Asedio"));
        deck.Add(new GoldCard("Tsunade",5,"Cuerpo a cuerpo"));
        deck.Add(new Lure("Jutsu de sustitucion"));
        deck.Add(new Weather("Jutsu Bola de Fuego"));
        deck.Add(new Weather("Kirin"));
        deck.Add(new Clear("Bijudama"));
        deck.Add(new Clear("Flecha de Indra"));
        deck.Add(new Boost("Jutsu de la Alianza Shinobi"));
        deck.Add(new Boost("Pildoras Ninjas"));
    }
}