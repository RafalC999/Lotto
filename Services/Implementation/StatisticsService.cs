using LottoScaper.DAL.Entities;
using LottoScaper.DAL.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace LottoScaper.DAL.Services.Implementation
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IWebScrapingService _webScrapingService;
        private readonly List<LottoDraw> drawList = new List<LottoDraw>();
        private readonly List<Frequency> frequencyList = new List<Frequency>();

        public StatisticsService(IWebScrapingService webScrapingService)
        {
            _webScrapingService = webScrapingService;
            var document = _webScrapingService.GetDocument("https://megalotto.pl/wyniki/lotto/30-ostatnich-losowan");
            drawList = _webScrapingService.ScrapLottoData(document);
        }

        public void PrintMostCommonNumbers()
        {
            var range = EnterNumberOfDraws();

            var list = MostCommonNumbers(range);

            PrintStatistics(list, range);
        }


        public int EnterNumberOfDraws()
        {
            Console.Clear();
            Console.WriteLine("Wprowadź liczbę losowań:");
            int numberOfDraws;
        Enter:
            bool success = int.TryParse(Console.ReadLine(), out numberOfDraws);

            if (numberOfDraws != null)
            {
                if (numberOfDraws > 0 && numberOfDraws < 31)
                {
                    return numberOfDraws;
                }
                else
                {
                    Console.WriteLine("Wprowadź liczbę od 1 do 30:");
                    goto Enter;
                }
            }
            else
            {
                Console.WriteLine("Wprowadź liczbę!");
                goto Enter;
            }
        }

        public void PrintStatistics(IOrderedEnumerable<Frequency> list, int range)
        {
            Console.Clear();
            Console.WriteLine($"Najczęściej występujące cyfry z {range} losowań: \n");
            Console.WriteLine("Liczba | Ilość wystąpień");

            foreach (var freq in list.Take(10))
            {
                Console.WriteLine($"{freq.Number} \t {freq.Quantity}");
            }
        }

        public IOrderedEnumerable<Frequency> MostCommonNumbers(int range)
        {
            var numbers = Enumerable.Range(1, 49);
            frequencyList.Clear();

            if (frequencyList.IsNullOrEmpty())
            {
                foreach (int number in numbers)
                {
                    Frequency frequency = new Frequency();
                    int reps = 0;
                    foreach (var draw in drawList.Take(range))
                    {
                        if (draw.Numbers.Contains(number))
                        {
                            reps++;
                        }
                    }
                    frequency.Number = number;
                    frequency.Quantity = reps;
                    frequencyList.Add(frequency);
                }
            }
            var sortedlist = frequencyList.OrderByDescending(o => o.Quantity).ThenBy(o => o.Number);

            return sortedlist;
        }
    }
}
