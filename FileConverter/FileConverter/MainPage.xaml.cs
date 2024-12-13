using FileConverter.Controller;

namespace FileConverter
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
#if WINDOWS
            // Windows独自の処理
            WindowsController windowsCotroller = new WindowsController();
            windowsCotroller.ImportInputFile("");

#elif MACCATALYST
            // Mac独自の処理

#endif
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }

}
