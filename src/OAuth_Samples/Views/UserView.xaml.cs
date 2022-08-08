using OAuth_Samples.ViewModels;

namespace OAuth_Samples.Views;

public partial class UserView
{
	private readonly UserViewModel _model = new UserViewModel();

    public UserView()
	{
		InitializeComponent();

		BindingContext = _model;

		_model.Initialize();
    }

}