using System.Windows.Input;

namespace Jajo.Exporter.Commands;

/// <summary>
/// The base for all commands that are inherited from <see cref="ICommand"/>
/// </summary>
public abstract class CommandBase : ICommand
{
    public bool CanExecute(object parameter) => true;

    public abstract void Execute(object parameter);

    public event EventHandler CanExecuteChanged;
}