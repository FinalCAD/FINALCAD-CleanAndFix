using System;
using Autodesk.AutoCAD.DatabaseServices;

namespace CleanAndFix.Utils
{
    static class EntityUtils
    {
        public static void ProcessingDwgs(Database database, Transaction transaction, Func<Entity, Entity> processFunc, bool toStatic = false)
        {
            BlockTable blockTb = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
            ObjectId msId = blockTb[BlockTableRecord.ModelSpace];

            LayerTable layerTb = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
            if (layerTb == null)
                return;

            ProcessingDwgsEntities(msId, transaction, processFunc, toStatic);
            BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
            if (blockTable != null)
            {
                foreach (ObjectId blockId in blockTable)
                {
                    try
                    {
                        ProcessingDwgsEntities(blockId, transaction, processFunc, toStatic);
                    }
                    catch {}
                }
            }
        }

        private static void ProcessingDwgsEntities(ObjectId btrId, Transaction transaction,
            Func<Entity, Entity> processFunc, bool toStatic)
        {
            if (btrId == ObjectId.Null)
                return;

            BlockTableRecord blockTableRecord = transaction.GetObject(btrId, OpenMode.ForRead) as BlockTableRecord;
            if (blockTableRecord == null)
                return;

            foreach (ObjectId entId in blockTableRecord)
            {
                try
                {
                    if (entId == ObjectId.Null)
                        return;
                    Entity entity = transaction.GetObject(entId, OpenMode.ForRead) as Entity;

                    if (entity == null)
                        continue;
                    BlockReference blockRef = entity as BlockReference;
                    if (blockRef != null)
                    {
                        if (toStatic)
                            blockRef.ConvertToStaticBlock();
                        ProcessingDwgsEntities(blockRef.BlockTableRecord, transaction, processFunc, toStatic);
                    }
                    else
                    {
                        entity.UpgradeOpen();
                        entity = processFunc(entity);
                        entity.DowngradeOpen();
                    }
                }
                catch {}
            }
        }
    }
}
