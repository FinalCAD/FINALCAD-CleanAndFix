using System;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;

namespace CleanAndFix.Utils
{
    public static class ColorUtils
    {
        public static void ProcessingDwgsColor(Database database, Transaction transaction, Func<Color, Color> colorFunc)
        {
            Transparency trans = new Transparency(255);

            BlockTable blockTb = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
            ObjectId msId = blockTb[BlockTableRecord.ModelSpace];

            LayerTable layerTb = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
            if (layerTb == null)
                return;

            ProcessingDwgsColorEntities(msId, transaction, colorFunc);
            foreach (ObjectId layerId in layerTb)
            {
                LayerTableRecord layer = transaction.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                if (layer != null)
                {
                    layer.Color = colorFunc(layer.Color);
                    if (layer.Transparency != trans)
                        layer.Transparency = trans;
                }
            }
            BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
            if (blockTable != null)
            {
                foreach (ObjectId blockId in blockTable)
                {
                    ProcessingDwgsColorEntities(blockId, transaction, colorFunc);
                }
            }
        }

        private static void ProcessingDwgsColorEntities(ObjectId btrId, Transaction transaction,
            Func<Color, Color> colorFunc)
        {
            if (btrId == ObjectId.Null)
                return;

            Transparency trans = new Transparency(255);
            BlockTableRecord blockTableRecord = transaction.GetObject(btrId, OpenMode.ForRead) as BlockTableRecord;
            if (blockTableRecord == null)
                return;

            foreach (ObjectId entId in blockTableRecord)
            {
                if (entId == ObjectId.Null)
                    return;
                Entity entity = transaction.GetObject(entId, OpenMode.ForRead) as Entity;
                RotatedDimension dim = entity as RotatedDimension;

                if (entity == null)
                    continue;
                BlockReference blockRef = entity as BlockReference;
                if (blockRef != null)
                    ProcessingDwgsColorEntities(blockRef.BlockTableRecord, transaction, colorFunc);
                else
                {
                    ProcessingEntityColor(colorFunc, entity, trans, dim);
                }
            }
        }

        private static void ProcessingEntityColor(Func<Color, Color> colorFunc, Entity entity, Transparency trans, RotatedDimension dim)
        {
            if (!entity.Color.IsByLayer)
            {
                entity.UpgradeOpen();
                entity.Color = colorFunc(entity.Color);
                entity.DowngradeOpen();
            }
            if (entity.Transparency != trans)
            {
                entity.UpgradeOpen();
                entity.Transparency = trans;
                entity.DowngradeOpen();
            }
            if (dim != null)
            {
                dim.UpgradeOpen();
                dim.Dimclrd = colorFunc(dim.Dimclrd);
                dim.Dimclre = colorFunc(dim.Dimclre);
                dim.Dimclrt = colorFunc(dim.Dimclrt);
                dim.DowngradeOpen();
            }
        }

        // Convert RGB to HSL format
        public static double[] RgbToHslConverter(int red, int green, int blue)
        {
            Color inputColor = Color.FromRgb((byte)red, (byte)green, (byte)blue);
            double[] hls = new double[3];

            // Format HSL
            double cMax = Math.Max(Math.Max(red, green), blue);
            double cMin = Math.Min(Math.Min(red, green), blue);
            double lightness = (cMax + cMin) / 510.0d;

            lightness = Math.Round(100 * lightness) / 100.0;
            hls[0] = inputColor.ColorValue.GetHue();
            hls[1] = lightness;
            hls[2] = inputColor.ColorValue.GetSaturation();
            return hls;
        }

        // Convert from HSL to RGB format
        public static double[] HslToRgbConverter(float hue, float saturation, float lightness)
        {
            float red = 0;
            float green = 0;
            float blue = 0;

            float c = (1 - Math.Abs(2 * lightness - 1)) * saturation;
            float x = c * (1 - Math.Abs((hue / 60) % 2 - 1));
            float m = (lightness - c / 2) * 255.0F;
            if (hue >= 0 && hue <= 60)
            {
                red = 255 * c;
                green = 255 * x;
                blue = 0;
            }

            else if (hue >= 60 && hue < 120)
            {
                red = 255 * x;
                green = 255 * c;
                blue = 0;
            }

            else if (hue >= 120 && hue < 180)
            {
                red = 0;
                green = 255 * c;
                blue = 255 * x;
            }

            else if (hue >= 180 && hue < 240)
            {
                red = 0;
                green = 255 * x;
                blue = 255 * c;
            }

            else if (hue >= 240 && hue < 300)
            {
                red = 255 * x;
                green = 0;
                blue = 255 * c;
            }
            else if (hue >= 300 && hue < 360)
            {
                red = 255 * c;
                green = 0;
                blue = 255 * x;
            }
            red += m;
            green += m;
            blue += m;

            double[] rgb = new double[3];
            rgb[0] = Math.Round(red);
            rgb[1] = Math.Round(green);
            rgb[2] = Math.Round(blue);
            return rgb;
        }

        public static double[] RgbToHslConverter(Color inputColor)
        {
            return RgbToHslConverter(inputColor.ColorValue.R, inputColor.ColorValue.G, inputColor.ColorValue.B);
        }
    }
}
