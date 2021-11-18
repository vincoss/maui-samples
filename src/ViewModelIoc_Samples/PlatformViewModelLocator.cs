using MauiSharedLibrary.ViewModels;
using Microsoft.Maui.Controls;
using System;
using System.Globalization;
using System.Reflection;
using ViewModelIoc_Samples;
using ViewModelIocSample.ViewModels;


namespace ViewModelIocSample
{
    public static class PlatformViewModelLocator
    {
        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(PlatformViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(PlatformViewModelLocator.AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(PlatformViewModelLocator.AutoWireViewModelProperty, value);
        }

        public static T Resolve<T>() where T : class
        {
            return (T)App.ServiceProvider.GetService(typeof(T));
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (DesignMode.IsDesignModeEnabled)
            {
                return;
            }

            var view = bindable as Element;
            if (view == null)
            {
                return;
            }

            if (view.BindingContext == null)
            {
                var viewType = view.GetType();
                var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

                var viewModelType = Type.GetType(viewModelName);
                if (viewModelType == null)
                {
                    throw new InvalidOperationException($"Not found: {viewModelName}");
                }
                var viewModel = App.ServiceProvider.GetService(viewModelType);

                if (viewModel == null)
                {
                    throw new InvalidOperationException($"Could not resolve view model for type: {viewModelType}");
                }

                view.BindingContext = viewModel;
            }

            var page = view as Page;
            if (page != null)
            {
                page.Appearing += (s, e) =>
                {
                    var model = view.BindingContext as BaseViewModel;
                    if (model != null)
                    {
                        model.Initialize();
                    }
                };

                page.Disappearing += (s, e) =>
                {
                    var model = view.BindingContext as BaseViewModel;
                    if (model != null)
                    {
                        // NOTE: Dispose call is not good in here, since page might be show and hide multiple times. Use navigator to dispose view model.
                    }
                };
            }
        }
    }
}
