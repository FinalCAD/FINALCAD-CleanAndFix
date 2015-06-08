//TODO DELETE THIS FILE

using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.Tools;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(Debug))]

namespace CleanAndFix.Tools
{
    class Debug
    {
        [CommandMethod("DEBUG", "DEBUG", CommandFlags.Transparent), UsedImplicitly]
        public void DebugCommand()
        {
            Application.ShowAlertDialog("DEBUG");
        }
    }
}
