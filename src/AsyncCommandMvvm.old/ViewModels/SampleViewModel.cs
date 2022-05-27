using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Microsoft.Maui.Controls;

namespace AsyncCommandMvvm.ViewModels
{
    public class SampleViewModel : BaseViewModel
    {
        private readonly WeakEventManager<string> _errorOccurredEventManager = new WeakEventManager<string>();

        public SampleViewModel()
        {
            SaveCommand = new AsyncCommand(OnSaveCommand);
            OtherCommand = new Command(OnOtherCommand);
        }

        public override Task InitializeAsync()
        {
            this.Title = nameof(InitializeAsync);
            return Task.CompletedTask;
        }

        private async Task OnSaveCommand()
        {
            this.Title = nameof(InitializeAsync);

            var t = Task.Run(() =>
            {
                Title = "From Task";
            });

            await t;
        }

        private void OnOtherCommand()
        {
            var a = async () =>
            {
                var t = Task.Run(() =>
                {
                    throw new Exception("test");
                });

                await t;
            };

            a().SafeFireAndForget(ex => OnErrorOccurred(ex.ToString()));
        }

        public ICommand SaveCommand { get; set; }
        public ICommand OtherCommand { get; set; }

        public event EventHandler<string> ErrorOccurred
        {
            add => _errorOccurredEventManager.AddEventHandler(value);
            remove => _errorOccurredEventManager.RemoveEventHandler(value);
        }
        void OnErrorOccurred(string message) => _errorOccurredEventManager.RaiseEvent(this, message, nameof(ErrorOccurred));
    }
}
