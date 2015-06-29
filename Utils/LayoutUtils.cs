using System;
using Autodesk.AutoCAD.DatabaseServices;

namespace CleanAndFix.Utils
{
    public static class LayoutUtils
    {
        /// <summary>Create a new layout on a database</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <param name="isOff">Define if the layout is off</param>
        /// <param name="isPlottable">Define if the layout is plottable</param>
        /// <returns>DBDictionaryEntry of the new layout</returns>
        public static DBDictionaryEntry CreateLayout(Database database, string layoutName, bool isOff = false,
            bool isPlottable = true)
        {
            DBDictionaryEntry layout;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                layout = CreateLayout(database, transaction, layoutName);
                transaction.Commit();
            }
            return layout;
        }

        /// <summary>Create a new layout on a database</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>DBDictionaryEntry of the new layout</returns>
        public static DBDictionaryEntry CreateLayout(Database database, Transaction transaction, string layoutName)
        {
            DBDictionary layouts = transaction.GetObject(database.LayoutDictionaryId, OpenMode.ForRead) as DBDictionary;
            if (layouts != null)
            {
                return CreateLayout(transaction, layouts, layoutName);
            }
            return null;
        }

        /// <summary>Create a new layout on a database</summary>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layoutTable">Layout table of the database</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>DBDictionaryEntry of the new layout</returns>
        public static DBDictionaryEntry CreateLayout(Transaction transaction, DBDictionary layouts, string layoutName)
        {
            if (!layoutTable.Has(layoutName))
            {
                DBDictionaryEntry layout = new DBDictionaryEntry();
                layout.Name = layoutName;
                layout.IsOff = isOff;
                layout.IsPlottable = isPlottable;

                layoutTable.UpgradeOpen();
                layoutTable.Add(layout);
                transaction.AddNewlyCreatedDBObject(layout, true);
                return layout;
            }
            else
            {
                return transaction.GetObject(layoutTable[layoutName], OpenMode.ForWrite) as DBDictionaryEntry;
            }
        }

        /// <summary>Get layout</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>DBDictionaryEntry of the layout</returns>
        public static DBDictionaryEntry GetLayout(Database database, string layoutName)
        {
            DBDictionaryEntry layout;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                layout = GetLayout(database, transaction, layoutName);
                transaction.Abort();
            }
            return layout;
        }

        /// <summary>Get layout</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>DBDictionaryEntry of the layout</returns>
        public static DBDictionaryEntry GetLayout(Database database, Transaction transaction, string layoutName)
        {
            LayoutTable layoutTable = transaction.GetObject(database.LayoutTableId, OpenMode.ForRead) as LayoutTable;
            if (layoutTable != null)
            {
                return GetLayout(transaction, layoutTable, layoutName);
            }
            return null;
        }

        /// <summary>Get layout</summary>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layoutTable">Layout table of the database</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>DBDictionaryEntry of the layout</returns>
        public static DBDictionaryEntry GetLayout(Transaction transaction, LayoutTable layoutTable, string layoutName)
        {
            if (layoutTable.Has(layoutName))
                return transaction.GetObject(layoutTable[layoutName], OpenMode.ForWrite) as DBDictionaryEntry;
            else
                return null;
        }




        /// <summary>Remove layout</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>success</returns>
        public static bool RemoveLayout(Database database, string layoutName)
        {
            bool success;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                success = RemoveLayout(database, transaction, layoutName);
                transaction.Commit();
            }
            return success;
        }

        /// <summary>Remove layout</summary>
        /// <param name="database">Database of the dwg</param>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>success</returns>
        public static bool RemoveLayout(Database database, Transaction transaction, string layoutName)
        {
            LayoutTable layoutTable = transaction.GetObject(database.LayoutTableId, OpenMode.ForWrite) as LayoutTable;
            if (layoutTable != null)
            {
                return RemoveLayout(transaction, layoutTable, layoutName);
            }
            return false;
        }

        /// <summary>Remove layout</summary>
        /// <param name="transaction">Transaction of the database</param>
        /// <param name="layoutTable">Layout table of the database</param>
        /// <param name="layoutName">Layout name to use</param>
        /// <returns>success</returns>
        public static bool RemoveLayout(Transaction transaction, LayoutTable layoutTable, string layoutName)
        {
            if (layoutTable.Has(layoutName))
            {
                DBDictionaryEntry layout =
                    transaction.GetObject(layoutTable[layoutName], OpenMode.ForWrite) as DBDictionaryEntry;
                if (layout != null)
                    layoutTable.Erase();
                return true;
            }
            else
                return false;
        }
    }
}
