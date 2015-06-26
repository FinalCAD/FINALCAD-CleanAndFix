using System;
using System.IO;
using System.Collections.Generic;
using Autodesk.AutoCAD.DatabaseServices;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace CleanAndFix.Utils
{
    public static class DwgUtils
    {
        /// <summary> Get files of the current directory </summary>
        /// <param name="database">Database of dwg</param>
        /// <param name="searchOptions">All directory or current directory</param>
        /// <returns>Dwgs of current directory</returns>
        public static string[] GetFolderDwgs(Database database, SearchOption searchOptions)
        {
            return GetFolderDwgs(GetRealPath(database), searchOptions);
        }
        /// <summary> Get files of the current directory </summary>
        /// <param name="dwgPath">Path of dwg</param>
        /// <param name="searchOptions">All directory or current directory</param>
        /// <returns>Dwgs of current directory</returns>
        public static string[] GetFolderDwgs(string dwgPath, SearchOption searchOptions)
        {
            return Directory.GetFiles(Path.GetDirectoryName(dwgPath), "*.dwg", searchOptions);
        }

        /// <summary> Get files of all dwgs related to current dwg </summary>
        /// <param name="database">Database of dwg</param>
        /// <returns>dwgs related to current dwg</returns>
        public static string[] GetRelatedDwgs(Database database)
        {
            XrefGraph xrefs = database.GetHostDwgXrefGraph(true);
            GraphNode root = xrefs.RootNode;
            List<string> files = new List<string>();
            files.Add(GetRealPath(database));
            return GetXrefsPath(root, files).ToArray();
        }

        public static List<string> GetXrefsPath(GraphNode node, List<string> files)
        {
            for (int i = 0; i < node.NumOut; i++ )
            {
                XrefGraphNode child = node.Out(i) as XrefGraphNode;
                if (child.XrefStatus == XrefStatus.Resolved)
                {
                    files.Add(GetRealPath(child.Database));
                    GetXrefsPath(child, files);
                }
            }
            return files;
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
                    log += Path.GetFileName(file) + ": " + e.Message + "\n";
                }
            }
            if (!string.IsNullOrEmpty(log))
            {
                Application.ShowAlertDialog(log);
            }
        }

        /// <summary> Get real path of a dwg.</summary>
        /// <param name="database">Database of dwg</param>
        /// <returns>Path of dwg</returns>
        public static string GetRealPath(Database database)
        {
            if (!database.Filename.Contains("appdata"))
                return database.Filename;
            return database.OriginalFileName;
        }
    }
}
