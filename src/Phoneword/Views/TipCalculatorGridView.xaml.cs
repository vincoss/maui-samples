namespace Phoneword.Views;

public partial class TipCalculatorGridView : ContentPage
{
    private Color colorNavy = Colors.Navy;
    private Color colorSilver = Colors.Silver;

    public TipCalculatorGridView()
	{
		InitializeComponent();
    }

    void OnLight(object sender, EventArgs e)
    {
        Resources["fgColor"] = colorNavy;
        Resources["bgColor"] = colorSilver;
    }

    void OnDark(object sender, EventArgs e)
    {
        Resources["fgColor"] = colorSilver;
        Resources["bgColor"] = colorNavy;
    }

    private void OnNormalTip(object sender, EventArgs e)
    {

    }
    private void OnGenerousTip(object sender, EventArgs e)
    {

    }
}