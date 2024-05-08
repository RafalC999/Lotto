using LottoScaper.DAL.Services.Interfaces;

namespace LottoScaper.DAL.Services.Implementation
{
    public class ConsoleMenu : IConsoleMenu
    {
        private readonly IUserActionsService _userActionsService;
        private readonly IStatisticsService _statisticsService;
        public ConsoleMenu(IUserActionsService userActionsService,
                IStatisticsService statisticsService)
        {
            _userActionsService = userActionsService;
            _statisticsService = statisticsService;
        }

        public void ShowMenu()
        {
            Console.Clear();
            Menu();
        }

        public void WelcomeScreen()
        {
            var welcomeText = "Witaj w aplikacji lotto";
            Console.SetCursorPosition((Console.WindowWidth - welcomeText.Length) / 2, Console.CursorTop);
            Console.WriteLine(welcomeText);
        }

        public void Menu()
        {
            WelcomeScreen();
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Wyświetl ostatnie losowanie");
            Console.WriteLine("2. Sprawdź losowanie");
            Console.WriteLine("3. Wyświetl najczęstsze numery");
            //Console.WriteLine("4. Statystyki");

            Console.WriteLine("5. Zamknij");
            Choose();
        }

        public void Choose()
        {
            var keyInfo = Console.ReadKey(intercept: true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    ShowMenu();
                    break;

                case ConsoleKey.D1:
                    _userActionsService.LastDraw();
                    Back();
                    break;

                case ConsoleKey.D2:
                    _userActionsService.CheckDraw();
                    Back();
                    break;

                case ConsoleKey.D3:
                    _statisticsService.PrintMostCommonNumbers();
                    Back();
                    break;

                case ConsoleKey.D4:
                    UpdateDrawsDB();
                    Back();
                    break;

                case ConsoleKey.D5:
                    Environment.Exit(0);
                    break;
            }
        }

        public void MostCommonNumbers()
        {
            Console.Clear();
            Console.WriteLine("Najczęstsze");
        }

        public void UpdateDrawsDB()
        {
            Console.Clear();
            Console.WriteLine("Aktualizuj");
        }

        public void Back()
        {
            Console.WriteLine("\n\n\n Wciśnij ESC, aby powrócić do menu głównego...");
            Choose();
        }
    }
}
