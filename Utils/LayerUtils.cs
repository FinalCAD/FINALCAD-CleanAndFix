using System;
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
        /// <returns>LayerTableRecord of the new layer</returns>
        public static ObjectId CreateLayer(Database database, string layerName, bool isOff = false,
            bool isPlottable = true)
        {
            LayerTableRecord layer;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                layer = CreateLayer(database, transaction, layerName, isOff, isPlottable);
                transaction.Commit();
            }
            return layer.ObjectId;
        }

        /// <summary>Create a new layer on a database</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <param name="isOff">Define if the layer is off</param>
        /// <param name="isPlottable">Define if the layer is plottable</param>
        /// <returns>LayerTableRecord of the new layer</returns>
        public static LayerTableRecord CreateLayer(Database database, Transaction transaction, string layerName,
            bool isOff = false, bool isPlottable = true)
        {
            LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
            if (layerTable != null)
            {
                return CreateLayer(transaction, layerTable, layerName, isOff, isPlottable);
            }
            return null;
        }

        /// <summary>Create a new layer on a database</summary>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerTable">Layer table of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <param name="isOff">Define if the layer is off</param>
        /// <param name="isPlottable">Define if the layer is plottable</param>
        /// <returns>LayerTableRecord of the new layer</returns>
        public static LayerTableRecord CreateLayer(Transaction transaction, LayerTable layerTable, string layerName,
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
                return layer;
            }
            else
            {
                return transaction.GetObject(layerTable[layerName], OpenMode.ForWrite) as LayerTableRecord;
            }
        }

        /// <summary>Get layer</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="layerName">Layer name to use</param>
        /// <returns>LayerTableRecord of the layer</returns>
        public static LayerTableRecord GetLayer(Database database, string layerName)
        {
            LayerTableRecord layer;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                layer = GetLayer(database, transaction, layerName);
                transaction.Abort();
            }
            return layer;
        }

        /// <summary>Get layer</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <returns>LayerTableRecord of the layer</returns>
        public static LayerTableRecord GetLayer(Database database, Transaction transaction, string layerName)
        {
            LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForRead) as LayerTable;
            if (layerTable != null)
            {
                return GetLayer(transaction, layerTable, layerName);
            }
            return null;
        }

        /// <summary>Get layer</summary>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerTable">Layer table of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <returns>LayerTableRecord of the layer</returns>
        public static LayerTableRecord GetLayer(Transaction transaction, LayerTable layerTable, string layerName)
        {
            if (layerTable.Has(layerName))
                return transaction.GetObject(layerTable[layerName], OpenMode.ForWrite) as LayerTableRecord;
            else
                return null;
        }




        /// <summary>Remove layer</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="layerName">Layer name to use</param>
        /// <returns>success</returns>
        public static bool RemoveLayer(Database database, string layerName)
        {
            bool success;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                success = RemoveLayer(database, transaction, layerName);
                transaction.Commit();
            }
            return success;
        }

        /// <summary>Remove layer</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <returns>success</returns>
        public static bool RemoveLayer(Database database, Transaction transaction, string layerName)
        {
            LayerTable layerTable = transaction.GetObject(database.LayerTableId, OpenMode.ForWrite) as LayerTable;
            if (layerTable != null)
            {
                return RemoveLayer(transaction, layerTable, layerName);
            }
            return false;
        }

        /// <summary>Remove layer</summary>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layerTable">Layer table of the database</param>
        /// <param name="layerName">Layer name to use</param>
        /// <returns>success</returns>
        public static bool RemoveLayer(Transaction transaction, LayerTable layerTable, string layerName)
        {
            if (layerTable.Has(layerName))
            {
                LayerTableRecord layer =
                    transaction.GetObject(layerTable[layerName], OpenMode.ForWrite) as LayerTableRecord;
                if (layer != null)
                    layerTable.Erase();
                return true;
            }
            else
                return false;
        }
    }
}
