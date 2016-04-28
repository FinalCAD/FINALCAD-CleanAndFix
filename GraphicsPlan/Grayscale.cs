using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.GraphicsPlan;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(Grayscale))]

namespace CleanAndFix.GraphicsPlan
{
    class Grayscale
    {
        [CommandMethod("GRAPHICSPLAN", "FCGRAYSCALE", CommandFlags.Modal), UsedImplicitly]
        public void GrayscaleCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            if (!GrayscaleDwg(database))
                Application.ShowAlertDialog("Une erreur est survenue.");
        }

        private bool GrayscaleDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                ColorUtils.ProcessingDwgsColor(database, transaction, ColorToGrayscale);
                transaction.Commit();
            }
            return true;
        }

        private Color ColorToGrayscale(Color color)
        {
            double lightness = ColorUtils.RgbToHslConverter(color)[1];
            double[] rgb = ColorUtils.HslToRgbConverter(0.0F, 0.0F, (float)lightness);
            Color newColor = Color.FromRgb((byte)rgb[0], (byte)rgb[1], (byte)rgb[2]);
            return newColor;
        }
    }
}
