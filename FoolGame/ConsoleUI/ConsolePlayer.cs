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
        
        public IPlayerAction SelectPlayerAction(bool isAttack = false)
        {
            PrintTable();
            PrintHand();
            if (!isAttack)
            {
                var action = ReadAction();
                return action;
            }
            else
            {
                var action = ReadAttackAction();
                return action;
            }
        }
        
        public DefendAction Defend()
        {
            var defendAction = new DefendAction(this);
            Console.WriteLine("Select cards to defend");
            foreach (var notCoveredCard in Table.NotCoveredCards)
            {
                Console.WriteLine(notCoveredCard);
                var input = Console.ReadLine();
                int res;
                if (int.TryParse(input, out res) && res > 0)
                {
                    defendAction.AddPair(new CardsPair
                    {
                        DefendCard = Hand[res - 1],
                        AttackCard = notCoveredCard
                    });
                }
            }
            return defendAction;
        }

        public AttackAction Attack()
        {
            var attackAction = new AttackAction(this);
            PrintHand();
            Console.WriteLine("Select cards to attack");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Incorrect input");
                return Attack();
            }
            var selected = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Card>();
            foreach (var s in selected)
            {
                int res;
                if (int.TryParse(s, out res) && res > 0 && res <= Hand.Count)
                {
                    result.Add(Hand[res - 1]);
                }
            }
            if (!attackAction.AddCards(result))
            {
                Console.WriteLine("You can't select these cards");
                return Attack();
            }
            return attackAction;
        }

        public AddAction Add()
        {
            var addAction = new AddAction(this);
            PrintHand();
            Console.WriteLine("Select cards to attack");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return addAction;
            }
            var selected = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Card>();
            foreach (var s in selected)
            {
                int res;
                if (int.TryParse(s, out res) && res > 0 && res <= Hand.Count)
                {
                    result.Add(Hand[res - 1]);
                }
            }
            if (!addAction.AddCards(result))
            {
                Console.WriteLine("You can't select these cards");
                return Add();
            }
            return addAction;
        }
        
        private IPlayerAction ReadAction()
        {
            Console.WriteLine("Выберите действие");
            Console.WriteLine("1:Defend");
            Console.WriteLine("2:Transfer");
            Console.WriteLine("3:Pass");
            var result = Console.ReadLine();
            switch (result)
            {
                case "1":
                    return Defend();
                case "2":
                    return new TransferAction();
                case "3":
                    return new PassAction(this);
            }
            throw new NotImplementedException();
        }

        private IPlayerAction ReadAttackAction()
        {
            Console.WriteLine("Select action:");
            Console.WriteLine("1:Add");
            Console.WriteLine("2:None");
            var result = Console.ReadLine();
            switch (result)
            {
                case "1":
                    return Add();
                case "2":
                    return new NoneAction();
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

        private void PrintTable()
        {
            Console.Clear();
            Console.WriteLine("Trump is " + Table.Trump);
            Console.WriteLine("Opened cards:");
            foreach (var openedCard in Table.OpenedCards)
            {
                Console.WriteLine(openedCard);
            }
            Console.WriteLine();
            Console.WriteLine("Not covered cards:");
            foreach (var notCoveredCard in Table.NotCoveredCards)
            {
                Console.WriteLine(notCoveredCard);
            }
            Console.WriteLine();
        }
        
    }
}
