using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using LottoScaper.DAL.Entities;
using LottoScaper.DAL.Services.Interfaces;

namespace LottoScaper.DAL.Services.Implementation
{
    public class WebScrapingService : IWebScrapingService
    {
        public HtmlDocument GetDocument(string url)
        {
            var web = new HtmlWeb();
            var document = web.Load(url);

            return document;
        }

        public List<LottoDraw> ScrapLottoData(HtmlDocument document)
        {
            List<LottoDraw> drawList = new List<LottoDraw>();
            var scrapedData = document.QuerySelectorAll("#list_of_last_drawings_wyniki_lotto > div ul");

            foreach (var draw in scrapedData)
            {
                var node = draw.QuerySelectorAll("li");
                var lotto = new LottoDraw();
                List<int> numbers = new List<int>();

                lotto.DrawDate = DateOnly.Parse(node[1].InnerText);

                for (int i = 2; i <= 7; i++)
                {
                    numbers.Add(int.Parse(node[i].InnerText));
                }
                lotto.Numbers = numbers;
                drawList.Add(lotto);
            }
            return drawList;
        }

    }
}
