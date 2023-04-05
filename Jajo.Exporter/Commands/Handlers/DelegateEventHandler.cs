using Autodesk.Revit.UI;

namespace Jajo.Exporter.Commands.Handlers;

public class DelegateEventHandler : BaseEventHandler
{
    private Action _action;

    public override void Execute(UIApplication app)
    {
        _action?.Invoke();
    }

    public void Raise(Action action)
    {
        _action = action;
        Raise();
    }
}