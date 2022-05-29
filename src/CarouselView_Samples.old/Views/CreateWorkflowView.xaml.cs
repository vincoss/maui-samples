using Microsoft.Maui.Controls;

using System;
using System.Collections.ObjectModel;

namespace CarouselView_Samples.Views
{
    public partial class CreateWorkflowView : ContentPage
    {
        public CreateWorkflowView()
        {
            InitializeComponent();

            var model = new CreateWorkflowViewModel();
            BindingContext = model;
        }
    }

    public class CreateWorkflowViewModel
    {
        public CreateWorkflowViewModel()
        {
            ItemsSource = new ObservableCollection<CreateModelListItem>();

            ItemsSource.Add(new CreateModelListItem { TemplateName = "A" });
            ItemsSource.Add(new CreateModelListItem { TemplateName = "B" });
            ItemsSource.Add(new CreateModelListItem { TemplateName = "C" });
            ItemsSource.Add(new CreateModelListItem { TemplateName = "D" });
        }

        public ObservableCollection<CreateModelListItem> ItemsSource { get; private set; }
    }

    public class CreateWorkflowDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate A { get; set; }
        public DataTemplate B { get; set; }
        public DataTemplate C { get; set; }
        public DataTemplate D { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var model = item as CreateModelListItem;
            if (model == null)
            {
                return A;
            }
            switch (model.TemplateName)
            {
                case "A":
                    return A;
                case "B":
                    return B;
                case "C":
                    return C;
                case "D":
                    return D;
                default: return A;
            }
        }
    }

    public class CreateModelListItem
    {
        public string TemplateName { get; set; }
    }

}
