using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.Fix;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(Clean))]

namespace CleanAndFix.Fix
{
    class Clean
    {
        [CommandMethod("Fix", "CLEAN", CommandFlags.Transparent), UsedImplicitly]
        public void CleanCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            if (!CleanDwg(database))
                Application.ShowAlertDialog("An error occurred!"); 
        }

        [CommandMethod("Fix", "CLEANALL", CommandFlags.Transparent), UsedImplicitly]
        public void CleanAllCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            DwgUtils.ExecuteForeach(CleanDwg, DwgUtils.GetFolderDwgs(database, SearchOption.AllDirectories));
        }

        private bool CleanDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                CleanXrefDwgs(database, transaction);
                CleanXrefImages(database, transaction);
                CleanXrefPdfs(database, transaction);
                CleanLayers(database, transaction);
                CleanElements(database, transaction);

                transaction.Commit();
            }
            return true;
        }

        private void CleanXrefDwgs(Database database, Transaction transaction)
        {
            ObjectIdCollection idsToReload = new ObjectIdCollection();
            string rootPath = DwgUtils.GetRealPath(database);

            XrefGraph xrefGraph = database.GetHostDwgXrefGraph(true);
            if (xrefGraph.NumNodes - 1 > 0)
            {
                IEnumerable<string> dwgsList = DwgUtils.GetFolderDwgs(database, SearchOption.AllDirectories);
                IEnumerable<Tuple<string, string>> dwgTuples =
                    dwgsList.Select(x => new Tuple<string, string>(Path.GetFileName(x), x)).ToList();
                for (int i = 1; i < xrefGraph.NumNodes; i++)
                {
                    XrefGraphNode child = xrefGraph.GetXrefNode(i);
                    if (child.XrefStatus != XrefStatus.Resolved)
                    {
                        if (child.XrefStatus == XrefStatus.FileNotFound || child.XrefStatus == XrefStatus.Unresolved)
                        {
                            BlockTableRecord xrefRecord =
                                transaction.GetObject(child.BlockTableRecordId, OpenMode.ForWrite) as BlockTableRecord;
                        }
                        else if (child.XrefStatus == XrefStatus.Unloaded)
                        {
                            idsToReload.Add(child.BlockTableRecordId);
                        }
                        else if (child.XrefStatus == XrefStatus.Unreferenced)
                        {
                            database.DetachXref(child.BlockTableRecordId);
                        }
                    }
                }
            }
        }

        private bool FindDwgPath(IEnumerable<Tuple<string, string>> dwgTuples, string rootPath, string xrefPath)
        {
            throw new NotImplementedException();
        }

        private void CleanXrefImages(Database database, Transaction transaction)
        {
            //TODO throw new NotImplementedException();
        }

        private void CleanXrefPdfs(Database database, Transaction transaction)
        {
            //TODO throw new NotImplementedException();
        }

        private void CleanLayers(Database database, Transaction transaction)
        {
            LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
            if (layerTable == null)
                return;
            foreach (ObjectId layerId in layerTable)
            {
                LayerTableRecord layer = transaction.GetObject(layerId, OpenMode.ForWrite) as LayerTableRecord;
                if (layer != null)
                {
                    if (layer.IsLocked)
                        layer.IsLocked = false;
                    if (layer.IsReconciled == false)
                        layer.IsReconciled = true;
                }
            }
        }

        private void CleanElements(Database database, Transaction transaction)
        {
            //TODO throw new NotImplementedException();
        }
    }
}
