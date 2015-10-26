using System.IO;
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

            _layerName = EditorUtils.GetTextFromEditor(doc.Editor, "Enter keyword to turn on layer with it");
            _display = true;
            FilterDwg(database);
        }

        [CommandMethod("Fix", "FCFILTERAll", CommandFlags.Transparent), UsedImplicitly]
        public void FilterAllCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            _layerName = EditorUtils.GetTextFromEditor(doc.Editor, "Enter keyword to turn on layer with it");
            _display = true;
            DwgUtils.ExecuteForeach(FilterDwg, DwgUtils.GetFolderDwgs(database, SearchOption.AllDirectories));
        }

        [CommandMethod("Fix", "FCRFILTER", CommandFlags.Transparent), UsedImplicitly]
        public void ReverseFilterCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            _layerName = EditorUtils.GetTextFromEditor(doc.Editor, "Enter keyword to turn off layer with it");
            _display = false;
            FilterDwg(database);
        }

        [CommandMethod("Fix", "FCRFILTERAll", CommandFlags.Transparent), UsedImplicitly]
        public void ReverseFilterAllCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            _layerName = EditorUtils.GetTextFromEditor(doc.Editor, "Enter keyword to turn off layer with it");
            _display = false;
            DwgUtils.ExecuteForeach(FilterDwg, DwgUtils.GetFolderDwgs(database, SearchOption.AllDirectories));
        }

        private bool FilterDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                LayerTable layerTb = (LayerTable)transaction.GetObject(database.LayerTableId, OpenMode.ForRead);

                foreach (ObjectId id in layerTb)
                {
                    LayerTableRecord layer = transaction.GetObject(id, OpenMode.ForWrite) as LayerTableRecord;
                    if (layer != null && layer.Name.ToLower().Contains(_layerName.ToLower()))
                    {
                        if (layer.IsPlottable != _display)
                            layer.IsPlottable = _display;
                        if (layer.IsOff != !_display)
                            layer.IsOff = !_display;
                    }
                    else if (layer != null)
                    {
                        if (layer.IsPlottable != !_display)
                            layer.IsPlottable = !_display;
                        if (layer.IsOff != _display)
                            layer.IsOff = _display;
                    }
                }
                transaction.Commit();
            }
            return true;
        }
    }
}