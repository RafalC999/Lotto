using LottoScaper.DAL.Entities;
using LottoScaper.DAL.Services.Interfaces;

namespace LottoScaper.DAL.Services.Implementation
{
    public class UserActionsService : IUserActionsService
    {
        private readonly IWebScrapingService _webScrapingService;
        private readonly List<LottoDraw> drawList = new List<LottoDraw>();

        public UserActionsService(IWebScrapingService webScrapingService)
        {
            _webScrapingService = webScrapingService;
            var document = _webScrapingService.GetDocument("https://megalotto.pl/wyniki/lotto/30-ostatnich-losowan");
            drawList = _webScrapingService.ScrapLottoData(document);
        }

        public void LastDraw()
        {
            Console.Clear();
            var lastDraw = drawList.FirstOrDefault();
            Console.WriteLine("Ostatnie losowanie: \n");
            Console.WriteLine($"Data: {lastDraw.DrawDate}");
            Console.Write("Liczy: ");
            foreach (var numer in lastDraw.Numbers)
            {
                Console.Write($"{numer}, ");
            }
        }

        public void CheckDraw()
        {
            Console.Clear();
            var date = EnterDate();
            var numbers = EnterNumbers();
            YoursNumbers(numbers);

            var pickedDraw = drawList.FirstOrDefault(d => d.DrawDate == date);

            Console.Clear();
            YoursNumbers(numbers);
            Console.Write("\nLiczby z losowania: ");
            foreach (var number in pickedDraw.Numbers)
            {
                if (numbers.Contains(number))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{number}");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($", ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{number}, ");
                }
            }
        }

        public List<int> EnterNumbers()
        {
            var numbers = new List<int>();
            for (int i = 1; i < 7; i++)
            {
                try
                {
                    YoursNumbers(numbers);
                    Console.WriteLine($"\nWprowadź liczbę z kuponu ({i}/6): ");

                    var number = int.Parse(Console.ReadLine());

                    if (ValidateNumbers(number))
                    {
                        if (!numbers.Contains(number))
                        {
                            numbers.Add(number);
                        }
                        else
                        {
                            i--;
                            Console.WriteLine($"Liczba {number} została już wprowadzona!");
                        }
                    }
                    else
                    {
                        i--;
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Wprowadź liczbę!");
                    i--;
                }
            }
            return numbers;
        }
        public DateOnly? EnterDate()
        {
            try
            {
                DateOnly date;
                do
                {
                    Console.WriteLine("Wpisz datę w formacie DD-MM-YYYY:");
                    date = DateOnly.Parse(Console.ReadLine());
                }
                while (!ValidateDate(date));

                return date;
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("UWAGA: Wprowadź datę w poprawnym formacie (DD-MM-YYYY)!");
                EnterDate();
                return null;
            }
        }

        public bool ValidateDate(DateOnly inputDate)
        {
            Console.Clear();
            if (inputDate > DateOnly.FromDateTime(DateTime.Now))
            {
                Console.WriteLine($"Wprowadź przeszłą datę:");
                return false;
            }
            else if (drawList.FirstOrDefault(d => d.DrawDate == inputDate) == null)
            {
                Console.WriteLine($"UWAGA: Dnia {inputDate} nie było losowania!");
                return false;
            }
            return true;
        }

        public bool ValidateNumbers(int number)
        {
            if (number >= 1 && number <= 49)
            {
                Console.Clear();
                return true;
            }
            else
            {
                Console.Clear();
                Console.WriteLine("UWAGA: Wprowadź liczbę z zakresu od 1 do 49!");
                return false;
            }
        }

        public void YoursNumbers(List<int> numbers)
        {
            Console.Write("Twoje liczby: ");
            if (numbers != null)
            {
                foreach (var n in numbers)
                {
                    Console.Write($"{n}, ");
                }
            }
        }
    }
}

