using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace CleanAndFix.Utils
{
    public static class EditorUtils
    {
        public static string GetTextFromEditor(Editor editor, string message)
        {
            PromptStringOptions promptSo = new PromptStringOptions(message);
            PromptResult promptResult = editor.GetString(promptSo);

            if (promptResult.Status == PromptStatus.OK)
            {
                return promptResult.StringResult;
            }
            return null;
        }

        public static List<ObjectId> GetMultipleElementId(Document document, string message,
            params Type[] elementTypes)
        {
            var objectIds = new List<ObjectId>();

            Editor editor = document.Editor;
            Database database = document.Database;

            var peo = new PromptEntityOptions(message);
            peo.AllowNone = true;
            using (Transaction transaction = database.TransactionManager.StartTransaction())
            {
                while (true)
                {
                    PromptEntityResult psr = editor.GetEntity(peo);
                    if (psr == null || objectIds.Contains(psr.ObjectId))
                        continue;
                    if (psr.Status == PromptStatus.Cancel)
                        return null;
                    if (psr.Status == PromptStatus.OK)
                    {
                        DBObject obj = transaction.GetObject(psr.ObjectId, OpenMode.ForRead);

                        if (elementTypes == null || elementTypes.Any(elementType => obj.GetType() == elementType))
                        {
                            objectIds.Add(psr.ObjectId);
                            continue;
                        }
                    }
                    if (psr.Status == PromptStatus.None)
                        break;
                }
            }
            return objectIds;
        }
    }
}