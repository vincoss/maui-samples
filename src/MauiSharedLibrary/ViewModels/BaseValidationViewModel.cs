using MauiSharedLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace MauiSharedLibrary.ViewModels
{
    public abstract class BaseValidationViewModel : BaseViewModel
    {
        private readonly ModelStateDictionary _modelState = new ModelStateDictionary();

        public ModelStateDictionary ModelState
        {
            get { return _modelState; }
        }

        [IndexerName("Item")]
        public string this[string propertyName]
        {
            get
            {
                return _modelState.GetValue(propertyName);
            }
        }

        public bool IsValid
        {
            get { return _modelState.IsValid == false; }
        }
    }
}