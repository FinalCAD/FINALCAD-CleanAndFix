using System.IO;
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
        [CommandMethod("Fix", "FCCLEAN", CommandFlags.Transparent), UsedImplicitly]
        public void CleanCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            if (!CleanDwg(database))
                Application.ShowAlertDialog("An error occurred!"); 
        }

        private bool CleanDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                CleanLayers(database, transaction);
                EraseProxies(database);
                //CleanEntities(database, transaction);
                transaction.Commit();
            }
            return true;
        }

        private void EraseProxies(Database db)
        {
            RXClass zombieEntity = RXObject.GetClass(typeof(ProxyEntity));
            RXClass zombieObject = RXObject.GetClass(typeof(ProxyObject));
            ObjectId id;
            for (long l = db.BlockTableId.Handle.Value; l < db.Handseed.Value; l++)
            {
                if (!db.TryGetObjectId(new Handle(l), out id))
                    continue;
                if (id.ObjectClass.IsDerivedFrom(zombieObject) && !id.IsErased)
                {
                    try
                    {
                        using (DBObject proxy = id.Open(OpenMode.ForWrite))
                        {
                            proxy.Erase();
                        }
                    }
                    catch
                    {
                        using (DBDictionary newDict = new DBDictionary())
                        using (DBObject proxy = id.Open(OpenMode.ForWrite))
                        {
                            try
                            {
                                proxy.HandOverTo(newDict, true, true);
                            }
                            catch { }
                        }
                    }
                }
                else if (id.ObjectClass.IsDerivedFrom(zombieEntity) && !id.IsErased)
                {
                    try
                    {
                        using (DBObject proxy = id.Open(OpenMode.ForWrite))
                        {
                            proxy.Erase();
                        }
                    }
                    catch { }
                }
            }
        }

        private void CleanEntities(Database database, Transaction transaction)
        {
            EntityUtils.ProcessingDwgs(database, transaction, CleanEntity);
        }

        Entity CleanEntity(Entity entity)
        {
            MText mText = entity as MText;
            if (mText != null)
            {
                mText.Contents = mText.Text;
            }
            return entity;
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
    }
}
