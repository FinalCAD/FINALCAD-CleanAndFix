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
    }
}