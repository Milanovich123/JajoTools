using Jajo.Tools.ViewModels.Utils;
using System.Windows;

namespace Jajo.Tools.Views
{
    public partial class ToolsView
    {
        readonly IViewModel _viewModel;
        public ToolsView(IViewModel viewModel) : this()
        {
            _viewModel = viewModel;
            viewModel.ShowMessage += ShowMessage;
            Closing += Window_Closing;
            DataContext = viewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _viewModel.OnApplicationClosing();
        }

        private void ShowMessage(string text)
        {
            MessageBox.Show(this, text, "Внимание");
        }

        public ToolsView()
        {
            InitializeComponent();
        }
    }
}