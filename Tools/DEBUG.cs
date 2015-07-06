//TODO DELETE THIS FILE

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.Tools;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(Debug))]

namespace CleanAndFix.Tools
{
    class Debug
    {
        [CommandMethod("DEBUG", "DEBUG", CommandFlags.Modal), UsedImplicitly]
        public void DebugCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            DebugDwg(doc.Database);
        }

        [CommandMethod("DEBUG", "DEBUGALL", CommandFlags.Modal), UsedImplicitly]
        public void DebugAllCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            DwgUtils.ExecuteForeach(DebugDwg, DwgUtils.GetRelatedDwgs(doc.Database), false);
        }

        [CommandMethod("DEBUG", "GUBED", CommandFlags.Modal), UsedImplicitly]
        public void GubedCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            GubedDwg(doc.Database);
        }

        [CommandMethod("DEBUG", "GUBEDALL", CommandFlags.Modal), UsedImplicitly]
        public void GubedAllCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            DwgUtils.ExecuteForeach(GubedDwg, DwgUtils.GetRelatedDwgs(doc.Database), false);
        }

        private bool GubedDwg(Database database)
        {
            return true;
        }


        private bool DebugDwg(Database database)
        {
            return true;
        }
    }
}
