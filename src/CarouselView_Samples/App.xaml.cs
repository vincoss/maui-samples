using CarouselView_Samples.Views;

namespace CarouselView_Samples
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new NavigationPage(new PageWithCarouselView()));
        }
    }
}
