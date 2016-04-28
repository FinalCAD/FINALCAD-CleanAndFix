using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.Tools;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(LayerFilter))]

namespace CleanAndFix.Tools
{
    public class LayerFilter
    {
        private string _layerName = "";
        private bool _display;

        [CommandMethod("Fix", "FCFILTER", CommandFlags.Transparent), UsedImplicitly]
        public void FilterCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            _layerName = EditorUtils.GetTextFromEditor(doc.Editor, "Enter keyword to turn off layer with it");
            if (_layerName != null)
            {
                _display = false;
                FilterDwg(database);
            }
        }

        [CommandMethod("Fix", "FCRFILTER", CommandFlags.Transparent), UsedImplicitly]
        public void ReverseFilterCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            _layerName = EditorUtils.GetTextFromEditor(doc.Editor, "Enter keyword to turn on layer with it");
            if (_layerName != null)
            {
                _display = true;
                FilterDwg(database);
            }
        }

        private void FilterDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                LayerTable layerTb = (LayerTable)transaction.GetObject(database.LayerTableId, OpenMode.ForRead);

                foreach (ObjectId id in layerTb)
                {
                    LayerTableRecord layer = transaction.GetObject(id, OpenMode.ForWrite) as LayerTableRecord;
                    if (layer != null && layer.Name.ToLower().Contains(_layerName.ToLower()) && layer.Name.ToLower() != "defpoints")
                    {
                        if (layer.IsPlottable != _display)
                            layer.IsPlottable = _display;
                        if (layer.IsOff != !_display)
                            layer.IsOff = !_display;
                    }
                    else if (layer != null && layer.Name.ToLower() != "defpoints")
                    {
                        if (layer.IsPlottable != !_display)
                            layer.IsPlottable = !_display;
                        if (layer.IsOff != _display)
                            layer.IsOff = _display;
                    }
                }
                transaction.Commit();
            }
        }
    }
}