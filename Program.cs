using System;
using System.Collections.Generic;
using System.Threading;

namespace OOP_10_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Aquarium aquarium = new Aquarium();
            Menu menu = new Menu();
            bool continueCicle = true;

            while (continueCicle)
            {
                menu.ShowQuantityFish(aquarium.GetQuantutyFish());
                aquarium.ShowAquarium();
                aquarium.CalculateLifeCycleAquarium();

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    switch (key.Key)
                    {
                        case ConsoleKey.Enter:

                            aquarium.AddFish();

                            break;
                        case ConsoleKey.Backspace:

                            aquarium.DeleteFish();

                            break;
                        case ConsoleKey.Escape:

                            continueCicle = false;

                            break;
                    }
                }
                Thread.Sleep(200);
            }
        }
    }

    class Aquarium
    {
        private Menu _menu = new Menu();

        private List<Fish> _fishs = new List<Fish>();
        private int _maximumFish = 10;
        private int _minimumFish = 5;


        internal Aquarium()
        {
            GenerateAquarium();
        }

        internal void AddFish()
        {
            Fish fish = new Fish(GetId());
            _fishs.Add(fish);
        }

        internal void DeleteFish()
        {
            int id = _menu.EnterId();
            List<int> tempId = GetListId();

            if (tempId.Exists(idFromList => idFromList == id))
            {
                RemoveFish(id);
            }
            else
            {
                _menu.ShowEnterErrorId();
            }
        }

        internal void ShowAquarium()
        {
            int index = 0;

            foreach (Fish fish in _fishs)
            {
                _menu.ShowDisplayFishStats(fish, index);
                index++;
            }
        }

        internal int GetQuantutyFish()
        {
            return _fishs.Count;
        }

        internal void CalculateLifeCycleAquarium()
        {
            bool thereAreLive = true;

            foreach (Fish fish in _fishs)
            {
                fish.RemoveOneLife();
            }

            while (thereAreLive)//\\\\
            {
                FindFishToRemove();
                thereAreLive = ContinueCicle();
                if (ContinueCicle())
                {
                thereAreLive = ThereFishInAquarium();
                }
            }
        }

        private bool ThereFishInAquarium()
        {
            bool continueTemp = true;

            if (_fishs.Count == 0)
            {
                continueTemp = false;
            }
            return continueTemp;
        }

        private bool ContinueCicle()
        {
            bool continueTemp = true;

            for (int i = 0; i < _fishs.Count; i++)
            {
                if (_fishs[i].IsDead == false | i == _fishs.Count - 1 | _fishs.Count == 0)
                {
                    continueTemp = false;
                }
            }
            return continueTemp;
        }

        private void FindFishToRemove()
        {
            foreach (Fish fish in _fishs)
            {
                if (fish.IsDead == true)
                {
                    RemoveFish(fish);
                    break;
                }
            }
        }

        private void RemoveFish(Fish fish)
        {
            _fishs.Remove(fish);
            _menu.Clear();
            _menu.ShowQuantityFish(_fishs.Count);
        }

        private void RemoveFish(int id)
        {
            foreach (Fish fish in _fishs)
            {
                if (fish.Id == id)
                {
                    _fishs.Remove(fish);
                    break;
                }
            }
            _menu.Clear();
            _menu.ShowQuantityFish(_fishs.Count);
        }

        private void GenerateAquarium()
        {
            Random random = new Random();

            for (int i = 0; i < random.Next(_minimumFish, _maximumFish); i++)
            {
                Fish fish = new(i);
                AddFishToAquarium(fish);
            }
        }

        private void AddFishToAquarium(Fish fish)
        {
            _fishs.Add(fish);
        }

        private int GetId()
        {
            int id = 0;

            foreach (Fish fish in _fishs)
            {
                if (fish.Id >= id)
                {
                    id = fish.Id + 1;
                }
            }
            return id;
        }

        private List<int> GetListId()
        {
            List<int> tempId = new List<int>();

            foreach (Fish fish in _fishs)
            {
                tempId.Add(fish.Id);
            }
            return tempId;
        }
    }

    class Menu
    {
        private int[] _messageCoordinateLeftTop = { 1, 1 };

        internal int EnterId()
        {
            int id = 0;
            SetCursorPositionMessag();
            Console.Write("Введите ID рыбы для удаления - ");
            string idString = Console.ReadLine();

            if (int.TryParse(idString, out id) == false)
            {
                Console.Write("Вы ввели не число попробуйте еще раз.");
            }
            return id;
        }

        internal void ShowDisplayFishStats(Fish fish, int index)
        {
            string status;
            int indent = index + 5;

            if (fish.IsDead == true)
                status = "Мертвая";
            else
                status = "Живая";

            Console.SetCursorPosition(_messageCoordinateLeftTop[0], _messageCoordinateLeftTop[1] + indent);
            Console.Write($"Id - {fish.Id} Жизнь - {fish.Life}");
            Console.Write(" Скин - ");
            Console.ForegroundColor = fish.ColorFish;
            Console.Write(fish.Skin);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" Статус - {status}     ");
            Console.WriteLine();
        }

        internal void Clear()
        {
            Console.Clear();
        }

        internal void ShowEnterErrorId()
        {
            SetCursorPositionMessag();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Введенный ID отсутствует.");
            Console.ForegroundColor = ConsoleColor.White;
        }

        internal void ShowQuantityFish(int allFish)
        {
            SetCursorPositionMessag();
            Console.WriteLine();
            Console.Write($"Всего рыб в аквариуме - {allFish}");
            Console.WriteLine();
            Console.WriteLine("Для выхода нажмите esc.");
            Console.WriteLine("Чтобы добавить рыбу, нажмите Enter.");
            Console.WriteLine("Чтобы удалить рыбу, нажмите Bacspace");
        }

        private void SetCursorPositionMessag()
        {
            Console.SetCursorPosition(_messageCoordinateLeftTop[0], _messageCoordinateLeftTop[1]);
        }
    }


    class Fish
    {
        private readonly ConsoleColor[] _colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        private char[] _skin = { '#', '@', '%', '$', '&', '№' };
        private int _minimumLifeTime = 40;
        private int _maximumLifeTime = 100;
        private Random _random = new();

        internal int Id { get; private set; }

        internal int Life { get; private set; }

        internal char Skin { get; private set; }

        internal bool IsDead { get; private set; }

        internal ConsoleColor ColorFish { get; private set; }

        internal Fish(int id)
        {
            Id = id;
            Life = GenerateLife();
            Skin = GenerateSkin();
            ColorFish = GenerateColor();
            IsDead = false;
        }

        internal void RemoveOneLife()
        {
            Life--;

            if (Life <= 0)
            {
                Life = 0;
                IsDead = true;
            }
        }

        private int GenerateLife()
        {
            int healt = _random.Next(_minimumLifeTime, _maximumLifeTime);
            return healt;
        }

        private ConsoleColor GenerateColor()
        {
            ConsoleColor color;

            color = _colors[(_random.Next(0, _colors.Length))];
            if (color == ConsoleColor.Black)
            {
                color = ConsoleColor.DarkBlue;
            }
            return color;
        }

        private char GenerateSkin()
        {
            char skin = _skin[_random.Next(0, _skin.Length)];
            return skin;
        }
    }
}
