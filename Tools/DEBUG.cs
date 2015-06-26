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

        [CommandMethod("DEBUG", "GUBED", CommandFlags.Modal), UsedImplicitly]
        public void GubedCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            GubedDwg(doc.Database);
        }

        [CommandMethod("DEBUG", "DEBUGALL", CommandFlags.Modal), UsedImplicitly]
        public void DebugAllCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            DwgUtils.ExecuteForeach(DebugDwg, DwgUtils.GetRelatedDwgs(doc.Database), false);
        }

        private bool GubedDwg(Database database)
        {
            try
            {
                LayerUtils.RemoveLayer(database, "Test1");
            }
            catch (Exception e)
            {
                Application.ShowAlertDialog(e.Message);
                return false;
            }
            try
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LayerUtils.RemoveLayer(database, transaction, "Test2");
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Application.ShowAlertDialog(e.Message);
            }
            try
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    LayerUtils.RemoveLayer(transaction, layerTable, "Test3");
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Application.ShowAlertDialog(e.Message);
            }
            return true;
        }


        private bool DebugDwg(Database database)
        {
            try
            {
                LayerUtils.CreateLayer(database, "Test1");
            }
            catch (Exception e)
            {
                Application.ShowAlertDialog(e.Message);
                return false;
            }
            try
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LayerUtils.CreateLayer(database, transaction, "Test2");
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Application.ShowAlertDialog(e.Message);
            }
            try
            {
                using (Transaction transaction = database.TransactionManager.StartTransaction())
                {
                    LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
                    LayerUtils.CreateLayer(transaction, layerTable, "Test3");
                    transaction.Commit();
                }
            }
            catch (Exception e)
            {
                Application.ShowAlertDialog(e.Message);
            }
            return true;
        }
    }
}
