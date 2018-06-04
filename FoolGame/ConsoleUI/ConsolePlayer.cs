using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;

namespace ConsoleUI
{
    public class ConsolePlayer : IPlayer
    {
        public int Id { get; set; }
        public List<Card> Hand { get; set; }

        public ConsolePlayer(int id)
        {
            Id = id;
            Hand = new List<Card>();
        }

        public List<Card> SelectCards()
        {
            PrintHand();
            return ReadSelection();
        }

        public PlayerAction SelectPlayerAction(bool isAttack = false)
        {
            PrintHand();
            if (!isAttack)
            {
                var action = ReadAction();
                return action;
            }
            throw new NotImplementedException();
        }

        private PlayerAction ReadAction()
        {
            Console.WriteLine("Выберите действие");
            Console.WriteLine("1:Defend");
            Console.WriteLine("2:Transfer");
            Console.WriteLine("3:Pass");
            var result = Console.ReadLine();
            switch (result)
            {
                case "1":
                    return new DefendAction();
                case "2":
                    return new TransferAction();
                case "3":
                    return new PassAction();
            }
            throw new NotImplementedException();
        }

        private void PrintHand()
        {
            Console.WriteLine($"Player {Id}");
            foreach (var card in Hand)
            {
                Console.Write($"{card.Suit.Print()}{card.Nominal} ");
            }
            Console.WriteLine();
        }

        private List<Card> ReadSelection()
        {
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Incorrect input");
                return ReadSelection();
            }
            var selected = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Card>();
            foreach (var s in selected)
            {
                int res;
                if (int.TryParse(s, out res) && res > 0 && res <= Hand.Count)
                {
                    result.Add(Hand[res-1]);
                }
            }
            return result;
        }
    }
}
