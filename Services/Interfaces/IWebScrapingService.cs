using HtmlAgilityPack;
using LottoScaper.DAL.Entities;

namespace LottoScaper.DAL.Services.Interfaces
{
    public interface IWebScrapingService
    {
        HtmlDocument GetDocument(string url);
        List<LottoDraw> ScrapLottoData(HtmlDocument document);
        //void LastDraw();
    }
}
