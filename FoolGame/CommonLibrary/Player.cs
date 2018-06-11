﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class Player
    {
        public int Id { get; private set; }
        public List<Card> Hand { get; set; }
        

        public Player(int id)
        {
            Id = id;
            Hand = new List<Card>();
        }

        public List<Card> SelectCards()
        {
            return new List<Card>();
        }

        public IPlayerAction SelectPlayerAction(bool isAttack = false)
        {
            return null;
        } 
    }
}
