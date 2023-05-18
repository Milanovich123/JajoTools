using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Jajo.Utils.Core;

namespace Jajo.Tools.Commands.Handlers;

public sealed class TestEventHandler : BaseEventHandler
{
    private Action<string> _showMessage;
    private string _someText;

    public override void Execute(UIApplication app)
    {
        using var t = new Transaction(RevitApi.Document, "ProjectName_DocumentChanged");
        try
        {
            t.Start();
            _showMessage.Invoke(_someText);
        }
        catch (Exception)
        {
            _showMessage.Invoke("Описание ошибки");
            t.RollBack();
        }
        finally
        {
            if (!t.HasEnded()) t.Commit();
        }
    }

    public void Raise(Action<string> showMessage, string someText)
    {
        _showMessage = showMessage;
        _someText = someText;
        Raise();
    }
}