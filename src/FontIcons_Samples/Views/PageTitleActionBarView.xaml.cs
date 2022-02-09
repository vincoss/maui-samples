

namespace FontIcons_Samples.Views
{
    public partial class PageTitleActionBarView : ContentPage
    {

        public PageTitleActionBarView()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var source = lblDone.BackgroundColor;
            lblDone.BackgroundColor = Colors.Pink;
       //     lblDone.BackgroundColor = source;
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if(e.StatusType == GestureStatus.Started || e.StatusType == GestureStatus.Running)
            {
                lblDone.BackgroundColor = Colors.Pink;
            }
            else
            {
                lblDone.BackgroundColor = Colors.Transparent;
            }
        }
    }
}