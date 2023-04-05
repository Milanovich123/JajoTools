using Autodesk.Revit.Attributes;
using Jajo.Tools.Commands.Handlers;
using Jajo.Tools.Core;
using Jajo.Tools.Utils;
using Jajo.Tools.ViewModels;
using Jajo.Tools.Views;
using Nice3point.Revit.Toolkit.External;

namespace Jajo.Tools.Commands
{
    [UsedImplicitly]
    [Transaction(TransactionMode.Manual)]
    public class ToolsCommand : ExternalCommand
    {
        private static ToolsView _view;
        public static readonly TestEventHandler ExternalEvent = new();
        public static readonly DelegateEventHandler DelegateEvent = new();
        public override void Execute()
        {
            RevitApi.UiApplication ??= ExternalCommandData.Application;

            if (_view is not null && _view.IsLoaded)
            {
                _view.Focus();
            }

            var viewModel = new ToolsViewModel();
            _view = new ToolsView(viewModel);
            _view.Show(ExternalCommandData.Application);
        }
    }
}