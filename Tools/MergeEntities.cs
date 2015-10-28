using System.Collections.Generic;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using CleanAndFix.Annotations;
using CleanAndFix.Tools;
using CleanAndFix.Utils;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: CommandClass(typeof(MergeEntities))]

namespace CleanAndFix.Tools
{
    public class MergeEntities
    {
        [CommandMethod("Tools", "FCMERGETEXT", CommandFlags.UsePickSet), UsedImplicitly]
        public void MergeTextCommand()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database database = doc.Database;

            List<ObjectId> textsId = EditorUtils.GetMultipleElementId(doc, "Select text to merge\n", typeof(MText), typeof(DBText));
            if (textsId != null)
                MergeTextDwg(database, textsId);
        }

        private void MergeTextDwg(Database database, List<ObjectId> textsId)
        {
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                BlockTable blockTable = transaction.GetObject(database.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord blockTableRecord = transaction.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Point3d pos;
                string message = GetPosAndMessageFromTexts(textsId, transaction, out pos);
                MText mtext = transaction.GetObject(textsId[0], OpenMode.ForWrite) as MText;
                if (mtext != null)
                {
                    MText newMText = new MText();
                    newMText.CopyFrom(mtext);
                    newMText.Contents = message;
                    newMText.Location = pos;

                    if (blockTableRecord != null)
                        blockTableRecord.AppendEntity(newMText);
                    transaction.AddNewlyCreatedDBObject(newMText, true);
                }
                transaction.Commit();
            }
        }

        private static string GetPosAndMessageFromTexts(List<ObjectId> textsId, Transaction transaction, out Point3d pos)
        {
            string message = "";
            double x = 0;
            double y = 0;
            double z = 0;
            foreach (ObjectId id in textsId)
            {
                MText mtext = transaction.GetObject(id, OpenMode.ForRead) as MText;
                if (mtext != null)
                {
                    x += mtext.Location.X;
                    y += mtext.Location.Y;
                    z += mtext.Location.Z;
                    message += mtext.Text + "\r\n";
                }
                else
                {
                    DBText dbText = transaction.GetObject(id, OpenMode.ForRead) as DBText;
                    if (dbText != null)
                    {
                        x += dbText.Position.X;
                        y += dbText.Position.Y;
                        z += dbText.Position.Z;
                        message += dbText.TextString + "\r\n";
                    }
                }
            }
            x = x / textsId.Count;
            y = y / textsId.Count;
            z = z / textsId.Count;
            pos = new Point3d(x, y, z);
            return message.Remove(message.Length - 1);
        }
    }
}