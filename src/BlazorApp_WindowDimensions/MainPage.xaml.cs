namespace BlazorApp_WindowDimensions
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            this.Appearing += MainPage_Appearing;
        }

        private void MainPage_Appearing(object sender, EventArgs e)
        {
        }

        public void SetInfo(string info)
        {
            lblDisplayInfo.Text = info; 
        }
    }
}