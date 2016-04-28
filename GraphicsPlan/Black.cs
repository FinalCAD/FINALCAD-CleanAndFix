using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.GraphicsPlan;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(Black))]

namespace CleanAndFix.GraphicsPlan
{
    class Black
    {
        [CommandMethod("GRAPHICSPLAN", "FCBLACK", CommandFlags.Transparent), UsedImplicitly]
        public void BlackCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            if (!BlackDwg(database))
                Application.ShowAlertDialog("An error occurred!"); 
        }

        private bool BlackDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                ColorUtils.ProcessingDwgsColor(database, transaction, ColorToBlack);
                transaction.Commit();
            }
            return true;
        }

        private Color ColorToBlack(Color color)
        {
            if (color == null) throw new ArgumentNullException("color");
            color = Color.FromRgb(0, 0, 0);
            return color;
        }
    }
}
