using Autodesk.Revit.DB;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jajo.Tools.Commands;
using Jajo.Tools.Core;
using Jajo.Tools.ViewModels.Utils;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Jajo.Tools.ViewModels
{
    public sealed class ToolsViewModel : ObservableValidator, IViewModel
    {
        private ICommand _revitApiContextActionCommand;
        private ICommand _simpleRelayCommand;
        private RelayCommand<bool?> _relayWithConditionCommand;
        private ICommand _onWindowLoadedCommand;
        private ICommand _simpleRevitApiCommand;
        private bool _isChecked;
        private string _text;

        public Action<string> ShowMessage { get; set; }

        public ICommand RevitApiContextActionCommand => _revitApiContextActionCommand ??= new RelayCommand(() =>
        {
            //различные варианты вызова ExternalCommand можно посмотреть тут:
            //https://github.com/Nice3point/RevitTemplates/wiki/Modeless-window
            ToolsCommand.ExternalEvent.Raise(ShowMessage, "Revit Api Context action");
        });

        public ICommand SimpleRelayCommand => _simpleRelayCommand ??= new RelayCommand(() =>
        {
            ShowMessage?.Invoke("Simple Command");
        });

        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                if (value == _isChecked) return;
                _isChecked = value;
                OnPropertyChanged();
                //сообщаем команде, что изменилось условие Predicate
                RelayWithConditionCommand.NotifyCanExecuteChanged();
            }
        }

        [Required(ErrorMessage = "This parameter must not be empty")]
        public string Text
        {
            get => _text;
            set
            {
                if (value == _text) return;
                _text = value;
                OnPropertyChanged();
                ValidateProperty(_text);
            }
        }

        public ICommand OnWindowLoadedCommand => _onWindowLoadedCommand ??= new RelayCommand(() =>
        {
            ShowMessage?.Invoke("Window opened");
            ValidateAllProperties();
        });


        public ICommand SimpleRevitApiContextActionCommand => _simpleRevitApiCommand ??= new RelayCommand(() =>
        {
            ToolsCommand.DelegateEvent.Raise(() =>
            {
                using var t = new Transaction(RevitApi.Document, "Simple Command");
                t.Start();

                ShowMessage?.Invoke("Simple Revit Api Context action");

                t.Commit();
            });
        });

        //команда с параметром типа bool
        public RelayCommand<bool?> RelayWithConditionCommand => _relayWithConditionCommand ??= new RelayCommand<bool?>(p =>
            {
                //мы можем использовать значение параметра в команде
                ShowMessage?.Invoke("Command with condition");
            },
            p =>
            {
                //и использовать значение параметра для активации/деактивации контрола.
                return p == true;
            });

        public void OnApplicationClosing()
        {
        }

    }
}