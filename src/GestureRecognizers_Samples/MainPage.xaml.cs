namespace GestureRecognizers_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void PointerGestureRecognizer_PointerEntered(object sender, PointerEventArgs e)
        {

        }

        private void PointerGestureRecognizer_PointerExited(object sender, PointerEventArgs e)
        { 
            // Position inside window
            Point? windowPosition = e.GetPosition(null);

            // Position relative to an Image
            Point? relativeToImagePosition = e.GetPosition(lblPointerGesture);

            // Position relative to the container view
            Point? relativeToContainerPosition = e.GetPosition((View)sender);

        }

        private void PointerGestureRecognizer_PointerMoved(object sender, PointerEventArgs e)
        {

        }
    }
}