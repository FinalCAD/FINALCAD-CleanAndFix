// (C) Copyright 2015 by Knowledge Corp

using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Ribbon;
using Autodesk.AutoCAD.Runtime;
using Autodesk.Windows;
using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

[assembly: ExtensionApplication(typeof(CleanAndFix.CleanAndFixPlugin))]

namespace CleanAndFix
{
    public class CleanAndFixPlugin : IExtensionApplication
    {
        public static RibbonManager RibbonManager;

        void IExtensionApplication.Initialize()
        {
            RibbonManager = new RibbonManager();
            RibbonServices.RibbonPaletteSetCreated += RibbonServices_RibbonPaletteSetCreated;
        }

        void IExtensionApplication.Terminate()
        {
        }

        private void RibbonServices_RibbonPaletteSetCreated(object sender, EventArgs e)
        {
            RibbonServices.RibbonPaletteSet.Load += RibbonPaletteSet_Loaded;
        }

        private void RibbonPaletteSet_Loaded(object sender, EventArgs e)
        {
            RibbonManager.InitializeFinalRibbon();
        }
    }

    public class AdskCommandHandler : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            RibbonButton button = parameter as RibbonButton;
            if (doc != null && button != null)
            {
                doc.SendStringToExecute((char)27 + button.CommandParameter.ToString() + " ", true, false, false);
            }
        }
    }
}
