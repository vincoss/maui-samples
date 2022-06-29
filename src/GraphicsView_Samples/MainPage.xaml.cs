using GraphicsView_Samples.Pages;
using System.Timers;
using SystemTimer = System.Timers;

namespace GraphicsView_Samples
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            var pages = new List<PageInfo>();
            pages.Add(new PageInfo { Type = typeof(DrawGraphicalObjectsPage) });
            pages.Add(new PageInfo { Type = typeof(TimerDrawLinesPage) });
            pages.Add(new PageInfo { Type = typeof(TimerDrawBallsPage) });

            ListOfPages.ItemsSource = pages;
        }

        private async void ListOfPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                var info = (PageInfo)e.CurrentSelection[0];
                var page = (Page)Activator.CreateInstance(info.Type);

                await this.Navigation.PushAsync(page);
            }
            ListOfPages.SelectedItem = null;
        }

        public class PageInfo
        {
            public Type Type { get; set; }
            public string Name
            {

                get
                {
                    if (Type != null)
                    {
                        return Type.Name;
                    }
                    return base.GetType().ToString();
                }
            }

            public override string ToString()
            {
                if (string.IsNullOrWhiteSpace(Name))
                {
                    return base.ToString();
                }
                return Name;
            }
        }

    }
}