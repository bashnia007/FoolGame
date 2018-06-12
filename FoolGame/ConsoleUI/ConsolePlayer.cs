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
                if (action == null) return SelectPlayerAction(false);
                return action;
            }
            else
            {
                var action = ReadAttackAction();
                if (action == null) return SelectPlayerAction(true);
                return action;
            }
        }
        
        public DefendAction Defend()
        {
            var defendAction = new DefendAction(this);
            Console.WriteLine("Select cards to defend. Press 0 to select another action");
            foreach (var notCoveredCard in Table.NotCoveredCards)
            {
                Console.WriteLine(notCoveredCard);
                var input = Console.ReadLine();
                int res;
                while (!int.TryParse(input, out res))
                {
                    Console.WriteLine("Incorrect input, please reneter");
                    input = Console.ReadLine();
                }
                if (res <= 0) return null;
                
                if (!defendAction.AddPair(new CardsPair
                {
                    DefendCard = Hand[res - 1],
                    AttackCard = notCoveredCard
                }))
                {
                    Console.WriteLine("You can't select this card. Please try again");
                    return Defend();
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
            Console.WriteLine("Select cards to attack. Press 0 to select another action");
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
                if (int.TryParse(s, out res))
                {
                    if (res <= 0 && res <= Hand.Count) return null;
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

        public TransferAction Transfer()
        {
            var transferAction = new TransferAction(this);
            Console.WriteLine("Select card for transfer. Press 0 to select another action");
            var input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return transferAction;
            }

            int selected;
            while (!int.TryParse(input, out selected))
            {
                Console.WriteLine("Incorrect input, please reneter");
            }
            if (selected <= 0) return null;


            if (!transferAction.SelectTransferCard(Hand[selected - 1]))
            {
                Console.WriteLine("You can select this card. Please try again");
                return Transfer();
            }
            return transferAction;
        }
        
        private IPlayerAction ReadAction()
        {
            Console.WriteLine("Выберите действие");
            Console.WriteLine("1:Defend");
            if(Table.TransferPossible) Console.WriteLine("2:Transfer");
            Console.WriteLine("3:Pass");
            var result = Console.ReadLine();
            switch (result)
            {
                case "1":
                    return Defend();
                case "2":
                    return Table.TransferPossible ? Transfer() : ReadAction();
                case "3":
                    return new PassAction(this);
            }
            return ReadAction();
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


        public void WinAction()
        {
            Console.WriteLine($"Player {Id}, you are WINNER!");
        }

        public void LoseAction()
        {
            Console.WriteLine($"Player {Id}, you are LOSER!");
        }
    }
}
