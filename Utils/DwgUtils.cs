using System;
using System.IO;
using Autodesk.AutoCAD.DatabaseServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CleanAndFix.Utils
{
    public static class DwgUtils
    {
        /// <summary> Get files of the current directory </summary>
        /// <param name="dwgPath">Path of dwg</param>
        /// <returns>Dwgs of current directory</returns>
        public static string[] GetFolderDwgs(string dwgPath)
        {
            throw new NotImplementedException();
        }

        /// <summary> Get files of the current directory & sub directories </summary>
        /// <param name="dwgPath">Path of dwg</param>
        /// <returns>Dwgs of current directory & sub directories</returns>
        public static string[] GetSubFoldersDwgs(string dwgPath)
        {
            throw new NotImplementedException();
        }

        /// <summary> Get files of all dwgs related to current dwg </summary>
        /// <param name="dwgPath">Path of dwg</param>
        /// <returns>dwgs related to current dwg</returns>
        public static string[] GetRelatedDwgs(string dwgPath)
        {
            return new[] {dwgPath};
            throw new NotImplementedException();
        }

        /// <summary> Execute a method for each given file </summary>
        /// <param name="method">Method to execute</param>
        /// <param name="files">List of file</param>
        /// <param name="save">Allow to save the database</param>
        public static void ExecuteForeach(Func<Database, bool> method, string[] files, bool save=true)
        {
            string currentDocName = Application.DocumentManager.MdiActiveDocument.Name;
            Database currentDatabase = Application.DocumentManager.MdiActiveDocument.Database;
            string log = "";

            foreach (var file in files)
            {
                try
                {
                    string fileName = Path.GetFileName(file);
                    if (fileName == null || fileName[0] == '.')
                        continue;

                    Database database;
                    if (file == currentDocName)
                    {
                        database = currentDatabase;
                    }
                    else
                    {
                        database = new Database(false, true);
                        database.ReadDwgFile(file, FileShare.ReadWrite, false, "");
                    }
                    method(database);
                    if (save && GetRealPath(database) != Application.DocumentManager.MdiActiveDocument.Name)
                        database.SaveAs(GetRealPath(database), DwgVersion.Current);
                }
                catch (Exception e)
                {
                    log += e.Message + "\n";
                }
            }
            if (!string.IsNullOrEmpty(log))
            {
                Application.ShowAlertDialog(log);
            }
        }

        public static string GetRealPath(Database database)
        {
            if (!database.Filename.Contains("appdata"))
                return database.Filename;
            return database.OriginalFileName;
        }
    }
}
