namespace MauiAppWeb.WinUI
{
    public partial class App : MauiWinUIApplication
    {
        // Статическое поле класса
        public static readonly Entities.HrDbContext DataBase = new Entities.HrDbContext();

        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
