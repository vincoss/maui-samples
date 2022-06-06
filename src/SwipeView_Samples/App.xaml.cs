using SwipeView_Samples.Views;

namespace SwipeView_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(new UI_SwipeViewMenuView())) { Title = "SwipeView_Samples" };
        }
    }
}