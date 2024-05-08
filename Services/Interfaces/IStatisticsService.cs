using LottoScaper.DAL.Entities;

namespace LottoScaper.DAL.Services.Interfaces
{
    public interface IStatisticsService
    {

        IOrderedEnumerable<Frequency> MostCommonNumbers(int range);
        void PrintMostCommonNumbers();

    }
}
