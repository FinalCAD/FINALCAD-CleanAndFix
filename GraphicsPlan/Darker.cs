using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.GraphicsPlan;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(Darker))]

namespace CleanAndFix.GraphicsPlan
{
    class Darker
    {
        private const float MaxLightness = 0.30F;

        [CommandMethod("GRAPHICSPLAN", "FCDARKER", CommandFlags.Modal), UsedImplicitly]
        public void DarkerCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            if (!DarkerDwg(database))
                Application.ShowAlertDialog("Une erreur est survenue.");
        }

        private bool DarkerDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                ColorUtils.ProcessingDwgsColor(database, transaction, ColorToDarker);
                transaction.Commit();
            }
            return true;
        }

        private Color ColorToDarker(Color color)
        {
            double[] hls = ColorUtils.RgbToHslConverter(color);
            double hue = hls[0];
            double lightness = hls[1];
            double saturation = hls[2];
            double[] rgb;
            if (lightness > MaxLightness)
                rgb = ColorUtils.HslToRgbConverter((float)hue, (float)saturation, MaxLightness);
            else
                rgb = ColorUtils.HslToRgbConverter((float)hue, (float)saturation, (float)lightness);
            Color newColor = Color.FromRgb((byte)rgb[0], (byte)rgb[1], (byte)rgb[2]);
            return newColor;
        }
    }
}
