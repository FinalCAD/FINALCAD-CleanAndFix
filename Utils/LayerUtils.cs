using Autodesk.AutoCAD.DatabaseServices;

namespace CleanAndFix.Utils
{
    public static class LayerUtils
    {
        /// <summary>Create a new layer on a database</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="layerName">Layer name to use</param>
        /// <param name="isOff">Define if the layer is off</param>
        /// <param name="isPlottable">Define if the layer is plottable</param>
        /// <returns></returns>
        public static ObjectId CreateLayer(Database database, string layerName, bool isOff = false,
            bool isPlottable = true)
        {
            ObjectId layerId;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                layerId = CreateLayer(database, transaction, layerName, isOff, isPlottable);
                transaction.Commit();
            }
            return layerId;
        }

        /// <summary>Create a new layer on a database</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <param name="isOff">Define if the layer is off</param>
        /// <param name="isPlottable">Define if the layer is plottable</param>
        /// <returns></returns>
        public static ObjectId CreateLayer(Database database, Transaction transaction, string layerName,
            bool isOff = false, bool isPlottable = true)
        {
            LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
            if (layerTable != null)
            {
                CreateLayer(transaction, layerTable, layerName, isOff, isPlottable);
            }
            return ObjectId.Null;
        }

        /// <summary>Create a new layer on a database</summary>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerTable">Layer table of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <param name="isOff">Define if the layer is off</param>
        /// <param name="isPlottable">Define if the layer is plottable</param>
        /// <returns></returns>
        public static ObjectId CreateLayer(Transaction transaction, LayerTable layerTable, string layerName,
            bool isOff = false, bool isPlottable = true)
        {
            if (!layerTable.Has(layerName))
            {
                LayerTableRecord layer = new LayerTableRecord();
                layer.Name = layerName;
                layer.IsOff = isOff;
                layer.IsPlottable = isPlottable;

                layerTable.UpgradeOpen();
                layerTable.Add(layer);
                transaction.AddNewlyCreatedDBObject(layer, true);
                return layer.ObjectId;
            }
            else
            {
                return layerTable[layerName];
            }
        }


    }
}
