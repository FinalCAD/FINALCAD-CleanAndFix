// (C) Copyright 2015 by  Knowledge Corp
//

using Autodesk.AutoCAD.Runtime;

[assembly: ExtensionApplication(typeof(CleanAndFix.CleanAndFixPlugin))]

namespace CleanAndFix
{
    public class CleanAndFixPlugin : IExtensionApplication
    {
        void IExtensionApplication.Initialize()
        {
        }

        void IExtensionApplication.Terminate()
        {
        }
    }
}
