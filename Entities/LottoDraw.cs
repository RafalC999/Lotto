namespace LottoScaper.DAL.Entities
{
    public class LottoDraw
    {
        public DateOnly DrawDate { get; set; }
        public List<int> Numbers { get; set; }
    }
}
