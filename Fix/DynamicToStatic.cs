using System.IO;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.Fix;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(DynamicToStatic))]

namespace CleanAndFix.Fix
{
    class DynamicToStatic
    {
        [CommandMethod("Fix", "FCDYNTOSTATIC", CommandFlags.Transparent), UsedImplicitly]
        public void DynToStaticCommmand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            if (!DynToStaticDwg(database))
                Application.ShowAlertDialog("An error occurred!");
        }

        [CommandMethod("Fix", "FCDYNTOSTATICALL", CommandFlags.Transparent), UsedImplicitly]
        public void DynToStaticAllCommmand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            DwgUtils.ExecuteForeach(DynToStaticDwg, DwgUtils.GetFolderDwgs(database, SearchOption.AllDirectories));
        }

        private bool DynToStaticDwg(Database database)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                EntityUtils.ProcessingDwgs(database, transaction, DynToStaticEntity, true);
                transaction.Commit();
            }
            return true;
        }

        public Entity DynToStaticEntity(Entity entity)
        {
            MText text = entity as MText;
            if (text != null)
            {
                if (text.HasFields)
                    text.ConvertFieldToText(); 
                text.Contents = text.Text;
            }
            else
            {
                DBText dbText = entity as DBText;
                if (dbText != null)
                {
                    if (dbText.HasFields)
                        dbText.ConvertFieldToText();
                }
            }
            return entity;
        }
    }
}
