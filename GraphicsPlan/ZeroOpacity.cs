using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.GraphicsPlan;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(ZeroOpacity))]

namespace CleanAndFix.GraphicsPlan
{
    class ZeroOpacity
    {
        [CommandMethod("GRAPHICSPLAN", "FCZEROOPACITY", CommandFlags.Modal), UsedImplicitly]
        public void ZeroOpacityCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            if (!ZeroOpacityDwg(database))
                Application.ShowAlertDialog("Une erreur est survenue.");
        }

        [CommandMethod("GRAPHICSPLAN", "FCZEROOPACITYALL", CommandFlags.Modal), UsedImplicitly]
        public void ZeroOpacityAllCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;

            DwgUtils.ExecuteForeach(ZeroOpacityDwg, DwgUtils.GetRelatedDwgs(doc.Database));
        }

        private bool ZeroOpacityDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                ColorUtils.ProcessingDwgsColor(database, transaction, ColorToZeroOpacity);
                transaction.Commit();
            }
            return true;
        }

        private Color ColorToZeroOpacity(Color color)
        {
            return color;
        }
    }
}
