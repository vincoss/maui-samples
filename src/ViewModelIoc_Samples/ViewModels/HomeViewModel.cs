using MauiSharedLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ViewModelIocSample.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public override void Initialize()
        {
            Title = DateTime.Now.ToString();
        }
    }
}
