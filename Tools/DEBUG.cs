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

        private bool DebugDwg(Database database)
        {
            try
            {
                LayerUtils.CreateLayer(database, "Test1");
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LayerUtils.CreateLayer(database, transaction, "Test2");
                    transaction.Commit();
                }

                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    LayerUtils.CreateLayer(transaction, layerTable, "Test3");
                    transaction.Commit();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
