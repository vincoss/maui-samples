using Label_Samples.ViewModels;
using System.Diagnostics;

namespace Label_Samples
{
    public partial class StackLayoutContainerView : ContentPage
    {
        public StackLayoutContainerView()
        {
            InitializeComponent();

            this.BindingContext = new LabelSampleViewModel();

            // Text decorations
            var underlineLabel = new Label { Text = "This is underlined text.", TextDecorations = TextDecorations.Underline };
            var strikethroughLabel = new Label { Text = "This is text with strikethrough.", TextDecorations = TextDecorations.Strikethrough };
            var bothLabel = new Label { Text = "This is underlined text with strikethrough.", TextDecorations = TextDecorations.Underline | TextDecorations.Strikethrough };

            // Colours
            var label = new Label { Text = "This is a green label.", TextColor = Color.FromHex("#77d065"), FontSize = 20 };

            // Displaying a specific number of lines
            var label2 = new Label
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In facilisis nulla eu felis fringilla vulputate. Nullam porta eleifend lacinia. Donec at iaculis tellus.",
                LineBreakMode = LineBreakMode.WordWrap,
                MaxLines = 2
            };

        }

        public void FormatedTextSample()
        {
            var formattedString = new FormattedString();
            formattedString.Spans.Add(new Span { Text = "Red bold, ", TextColor = Colors.Red, FontAttributes = FontAttributes.Bold });

            var span = new Span { Text = "default, " };
            span.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(async () => await DisplayAlert("Tapped", "This is a tapped Span.", "OK")) });
            formattedString.Spans.Add(span);
            formattedString.Spans.Add(new Span { Text = "italic small.", FontAttributes = FontAttributes.Italic, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) });

        }

        public void LineHeightSample()
        {
            // Line heigh
            var label3 = new Label
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In facilisis nulla eu felis fringilla vulputate. Nullam porta eleifend lacinia. Donec at iaculis tellus.",
                LineBreakMode = LineBreakMode.WordWrap,
                LineHeight = 1.8
            };

            var formattedString = new FormattedString();
            formattedString.Spans.Add(new Span
            {
                Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In a tincidunt sem. Phasellus mollis sit amet turpis in rutrum. Sed aliquam ac urna id scelerisque. ",
                LineHeight = 1.8
            });
            formattedString.Spans.Add(new Span
            {
                Text = "Nullam feugiat sodales elit, et maximus nibh vulputate id.",
                LineHeight = 1.8
            });
            var label = new Label
            {
                FormattedText = formattedString,
                LineBreakMode = LineBreakMode.WordWrap
            };
        }

        void OnLineHeightChanged(object sender, TextChangedEventArgs args)
        {
            var lineHeight = ((Entry)sender).Text;
            try
            {
                _lineHeightLabel.LineHeight = double.Parse(lineHeight);
            }
            catch (FormatException ex)
            {
               Debug.WriteLine($"Can't parse {lineHeight}. {ex.Message}");
            }
        }

    }
}