using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Autodesk.Windows;

namespace CleanAndFix
{
    public class RibbonManager
    {
        public void InitializeFinalRibbon()
        {
            RibbonButton cleanButton = new RibbonButton();
            cleanButton.IsToolTipEnabled = true;
            cleanButton.ToolTip = "Clean the drawing and unlock all layer";
            cleanButton.Orientation = Orientation.Vertical;
            //finalBrowserButton.LargeImage = ToBitmapImage(Resources.browser);
            cleanButton.Size = RibbonItemSize.Large;
            cleanButton.Text = "Clean";
            cleanButton.CommandParameter = "FCCLEAN";
            cleanButton.ShowText = true;
            cleanButton.CommandHandler = new AdskCommandHandler();
            cleanButton.MinWidth = 50;

            RibbonButton dyn2StaticButton = new RibbonButton();
            dyn2StaticButton.IsToolTipEnabled = true;
            dyn2StaticButton.ToolTip = "Remove all dynamic data in the drawing";
            dyn2StaticButton.Orientation = Orientation.Vertical;
            //dyn2staticButton.LargeImage = ToBitmapImage(Resources.commands);
            dyn2StaticButton.Size = RibbonItemSize.Large;
            dyn2StaticButton.Text = "Dyn2Static";
            dyn2StaticButton.CommandParameter = "FCDYNTOSTATIC";
            dyn2StaticButton.ShowText = true;
            dyn2StaticButton.CommandHandler = new AdskCommandHandler();
            dyn2StaticButton.MinWidth = 50;

            RibbonButton blackButton = new RibbonButton();
            blackButton.IsToolTipEnabled = true;
            blackButton.ToolTip = "Change all drawing to black";
            blackButton.Orientation = Orientation.Vertical;
            //blackButton.LargeImage = ToBitmapImage(Resources.commands);
            blackButton.Size = RibbonItemSize.Large;
            blackButton.Text = "Black";
            blackButton.CommandParameter = "FCBLACK";
            blackButton.ShowText = true;
            blackButton.CommandHandler = new AdskCommandHandler();
            blackButton.MinWidth = 50;

            RibbonButton grayscaleButton = new RibbonButton();
            grayscaleButton.IsToolTipEnabled = true;
            grayscaleButton.ToolTip = "Change all drawing to grayscale";
            grayscaleButton.Orientation = Orientation.Vertical;
            //grayscale.LargeImage = ToBitmapImage(Resources.commands);
            grayscaleButton.Size = RibbonItemSize.Large;
            grayscaleButton.Text = "Grayscale";
            grayscaleButton.CommandParameter = "FCGRAYSCALE";
            grayscaleButton.ShowText = true;
            grayscaleButton.CommandHandler = new AdskCommandHandler();
            grayscaleButton.MinWidth = 50;

            RibbonButton darkerButton = new RibbonButton();
            darkerButton.IsToolTipEnabled = true;
            darkerButton.ToolTip = "Change all drawing to darker";
            darkerButton.Orientation = Orientation.Vertical;
            //darker.LargeImage = ToBitmapImage(Resources.commands);
            darkerButton.Size = RibbonItemSize.Large;
            darkerButton.Text = "darker";
            darkerButton.CommandParameter = "FCDARKER";
            darkerButton.ShowText = true;
            darkerButton.CommandHandler = new AdskCommandHandler();
            darkerButton.MinWidth = 50;

            RibbonButton zeroopacityButton = new RibbonButton();
            zeroopacityButton.IsToolTipEnabled = true;
            zeroopacityButton.ToolTip = "Change all drawing to zeroopacity";
            zeroopacityButton.Orientation = Orientation.Vertical;
            //zeroopacity.LargeImage = ToBitmapImage(Resources.commands);
            zeroopacityButton.Size = RibbonItemSize.Large;
            zeroopacityButton.Text = "zeroopacity";
            zeroopacityButton.CommandParameter = "FCZEROOPACITY";
            zeroopacityButton.ShowText = true;
            zeroopacityButton.CommandHandler = new AdskCommandHandler();
            zeroopacityButton.MinWidth = 50;

            RibbonButton layerFilterButton = new RibbonButton();
            layerFilterButton.IsToolTipEnabled = true;
            layerFilterButton.ToolTip = "";//TODO
            layerFilterButton.Orientation = Orientation.Vertical;
            //layerFilter .LargeImage = ToBitmapImage(Resources.commands);
            layerFilterButton.Size = RibbonItemSize.Large;
            layerFilterButton.Text = "layerFilter";
            layerFilterButton.CommandParameter = "FCLAYERFILTER";
            layerFilterButton.ShowText = true;
            layerFilterButton.CommandHandler = new AdskCommandHandler();
            layerFilterButton.MinWidth = 50;

            RibbonButton reverseLayerFilterButton = new RibbonButton();
            reverseLayerFilterButton.IsToolTipEnabled = true;
            reverseLayerFilterButton.ToolTip = ""; //TODO
            reverseLayerFilterButton.Orientation = Orientation.Vertical;
            //reverseLayerFilter .LargeImage = ToBitmapImage(Resources.commands);
            reverseLayerFilterButton.Size = RibbonItemSize.Large;
            reverseLayerFilterButton.Text = "reverseLayerFilter";
            reverseLayerFilterButton.CommandParameter = "FCREVERSELAYERFILTER";
            reverseLayerFilterButton.ShowText = true;
            reverseLayerFilterButton.CommandHandler = new AdskCommandHandler();
            reverseLayerFilterButton.MinWidth = 50;

            RibbonButton mergeTextButton = new RibbonButton();
            mergeTextButton.IsToolTipEnabled = true;
            mergeTextButton.ToolTip = "";//TODO
            mergeTextButton.Orientation = Orientation.Vertical;
            //mergeText.LargeImage = ToBitmapImage(Resources.commands);
            mergeTextButton.Size = RibbonItemSize.Large;
            mergeTextButton.Text = "mergeText";
            mergeTextButton.CommandParameter = "FCMERGETEXT";
            mergeTextButton.ShowText = true;
            mergeTextButton.CommandHandler = new AdskCommandHandler();
            mergeTextButton.MinWidth = 50;

            RibbonControl ribbonControl = ComponentManager.Ribbon;
            RibbonTab tab = GetFinalcadRibbon(ribbonControl);

            RibbonPanelSource panelSource = new RibbonPanelSource();
            RibbonPanel panel = new RibbonPanel();
            panelSource.Title = "Clean";
            panelSource.Items.Add(cleanButton);
            panelSource.Items.Add(dyn2StaticButton);
            panel.Source = panelSource;
            tab.Panels.Add(panel);

            panelSource = new RibbonPanelSource();
            panel = new RibbonPanel();
            panelSource.Title = "Colors";
            panelSource.Items.Add(blackButton);
            panelSource.Items.Add(darkerButton);
            panelSource.Items.Add(grayscaleButton);
            panelSource.Items.Add(zeroopacityButton);
            panel.Source = panelSource;
            tab.Panels.Add(panel);

            panelSource = new RibbonPanelSource();
            panel = new RibbonPanel();
            panelSource.Title = "Tools";
            panelSource.Items.Add(layerFilterButton);
            panelSource.Items.Add(reverseLayerFilterButton);
            panelSource.Items.Add(mergeTextButton);
            panel.Source = panelSource;
            tab.Panels.Add(panel);
        }

        private RibbonTab GetFinalcadRibbon(RibbonControl ribbonControl)
        {
            foreach (RibbonTab tab in ribbonControl.Tabs)
            {
                if (tab.Id == "FINALCAD_Id")
                    return tab;
            }
            RibbonTab newTab = new RibbonTab();
            newTab.Title = "FINALCAD";
            newTab.Id = "FINALCAD_Id";
            ribbonControl.Tabs.Add(newTab);
            return newTab;
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}