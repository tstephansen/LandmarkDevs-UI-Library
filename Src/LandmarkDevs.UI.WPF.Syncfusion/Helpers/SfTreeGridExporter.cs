#region
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Parsing;
using Syncfusion.UI.Xaml.TreeGrid;
using Syncfusion.UI.Xaml.TreeGrid.Converter;
using Syncfusion.Windows.PdfViewer;
using Syncfusion.XlsIO;
#endregion

namespace LandmarkDevs.UI.WPF.Syncfusion.Helpers
{
    /// <summary>
    ///     Class SfTreeGridExporter.
    /// </summary>
    public static class SfTreeGridExporter
    {
        /// <summary>
        ///     Gets or sets the name of the header font.
        /// </summary>
        /// <value>The name of the header font.</value>
        public static string HeaderFontName { get; set; } = "Calibri";

        /// <summary>
        ///     Gets or sets the name of the cell font.
        /// </summary>
        /// <value>The name of the cell font.</value>
        public static string CellFontName { get; set; } = "Calibri";

        /// <summary>
        ///     Gets or sets the size of the header font.
        /// </summary>
        /// <value>The size of the header font.</value>
        public static int HeaderFontSize { get; set; } = 11;

        /// <summary>
        ///     Gets or sets the size of the cell font.
        /// </summary>
        /// <value>The size of the cell font.</value>
        public static int CellFontSize { get; set; } = 11;

        /// <summary>
        ///     Exports the SfTreeGrid to Excel.
        /// </summary>
        /// <param name="treeGrid">The tree grid.</param>
        /// <param name="defaultFileName">Default name of the file.</param>
        /// <param name="allowIndentColumn">if set to <c>true</c> [allow indent column].</param>
        public static void ExportTreeGridToExcel(SfTreeGrid treeGrid, string defaultFileName, bool allowIndentColumn)
        {
            var options = new SfTreeGridExporterOptions
            {
                DefaultFileName = defaultFileName,
                AllowIndentColumn = allowIndentColumn
            };
            ExportTreeGridToExcel(treeGrid, options);
        }

        /// <summary>
        ///     Exports the SfTreeGrid to Excel.
        /// </summary>
        /// <param name="treeGrid">The tree grid.</param>
        /// <param name="exportOptions">The export options.</param>
        public static void ExportTreeGridToExcel(SfTreeGrid treeGrid, SfTreeGridExporterOptions exportOptions)
        {
            if (treeGrid == null)
                return;
            var origRootNodes = treeGrid.View.Nodes.RootNodes;
            HeaderFontName = exportOptions.HeaderFontName;
            HeaderFontSize = exportOptions.HeaderFontSize;
            CellFontName = exportOptions.CellFontName;
            CellFontSize = exportOptions.CellFontSize;
            if (exportOptions.OnlyCheckedNodes)
            {
                var rootNodesList = treeGrid.View.Nodes.RootNodes;
                var rootNodes = new TreeNodes();
                foreach (var o in rootNodesList)
                    if (o.IsChecked != null && o.IsChecked == true)
                        rootNodes.Add(o);
                treeGrid.View.Nodes.RootNodes = rootNodes;
            }
            try
            {
                SetExportOptions(treeGrid, exportOptions, origRootNodes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.Trim());
            }
        }

        private static void SetExportOptions(SfTreeGrid treeGrid, SfTreeGridExporterOptions exportOptions, TreeNodes origRootNodes)
        {
            var options = new TreeGridExcelExportingOptions
            {
                AllowIndentColumn = exportOptions.AllowIndentColumn,
                CanExportHyperLink = true,
                AllowOutliningGroups = true,
                IsGridLinesVisible = true,
                ExcelVersion = ExcelVersion.Excel2013,
                ExportingEventHandler = ExportingHandler,
                CellsExportingEventHandler = CellExportingHandler
            };
            using (var excelEngine = new ExcelEngine())
            {
                var workBook = excelEngine.Excel.Workbooks.Create();
                workBook.Worksheets[0].PageSetup.LeftMargin = exportOptions.LeftMargin;
                workBook.Worksheets[0].PageSetup.TopMargin = exportOptions.TopMargin;
                workBook.Worksheets[0].PageSetup.BottomMargin = exportOptions.BottomMargin;
                workBook.Worksheets[0].PageSetup.RightMargin = exportOptions.RightMargin;
                workBook.Worksheets[0].PageSetup.Orientation = exportOptions.Orientation;
                treeGrid.ExportToExcel(workBook, options);
                if (exportOptions.AutoFitColumns)
                    foreach (var column in workBook.Worksheets[0].Columns)
                        column.AutofitColumns();
                if (exportOptions.AutoFitRows)
                    foreach (var row in workBook.Worksheets[0].Rows)
                        row.AutofitRows();
                var dialog = new SaveFileDialog
                {
                    FilterIndex = 2,
                    Filter = "Excel 97 to 2003 Files(*.xls)|*.xls|Excel 2007 to 2016 Files(*.xlsx)|*.xlsx",
                    FileName = exportOptions.DefaultFileName
                };
                if (dialog.ShowDialog() == true)
                {
                    using (var stream = dialog.OpenFile())
                    {
                        workBook.Version = dialog.FilterIndex == 1 ? ExcelVersion.Excel97to2003 : ExcelVersion.Excel2010;
                        workBook.SaveAs(stream);
                    }
                    Process.Start(dialog.FileName);
                }
                if (exportOptions.OnlyCheckedNodes)
                    treeGrid.View.Nodes.RootNodes = origRootNodes;
            }
        }

        /// <summary>
        ///     Exports the SfTreeGrid to Excel.
        /// </summary>
        /// <param name="treeGrid">The SfTreeGrid.</param>
        /// <param name="msCode">The MS Code.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "SG0001:Potential command injection with Process.Start")]
        public static void ExportTreeGridToExcel(SfTreeGrid treeGrid, string msCode)
        {
            if (treeGrid == null)
                return;
            try
            {
                var options = new TreeGridExcelExportingOptions
                {
                    AllowIndentColumn = false,
                    CanExportHyperLink = true,
                    AllowOutliningGroups = true,
                    IsGridLinesVisible = true,
                    ExcelVersion = ExcelVersion.Excel2013,
                    ExportingEventHandler = ExportingHandler,
                    CellsExportingEventHandler = CellExportingHandler
                };
                using (var excelEngine = new ExcelEngine())
                {
                    var workBook = excelEngine.Excel.Workbooks.Create();
                    treeGrid.ExportToExcel(workBook, options);
                    var dialog = new SaveFileDialog
                    {
                        FilterIndex = 2,
                        Filter = "Excel 97 to 2003 Files(*.xls)|*.xls|Excel 2007 to 2016 Files(*.xlsx)|*.xlsx",
                        FileName = $"{msCode} BOM"
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        using (var stream = dialog.OpenFile())
                        {
                            workBook.Version = dialog.FilterIndex == 1 ? ExcelVersion.Excel97to2003 : ExcelVersion.Excel2010;
                            workBook.SaveAs(stream);
                        }
                        Process.Start(dialog.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.Trim());
            }
        }

        /// <summary>
        ///     Prints the tree grid.
        /// </summary>
        /// <param name="treeGrid">The tree grid.</param>
        /// <param name="exportOptions">The export options.</param>
        public static void PrintTreeGrid(SfTreeGrid treeGrid, SfTreeGridExporterOptions exportOptions)
        {
            if (treeGrid == null)
                return;
            HeaderFontName = exportOptions.HeaderFontName;
            HeaderFontSize = exportOptions.HeaderFontSize;
            CellFontName = exportOptions.CellFontName;
            CellFontSize = exportOptions.CellFontSize;
            try
            {
                var options = new TreeGridExcelExportingOptions
                {
                    AllowIndentColumn = exportOptions.AllowIndentColumn,
                    CanExportHyperLink = true,
                    AllowOutliningGroups = true,
                    IsGridLinesVisible = true,
                    ExcelVersion = ExcelVersion.Excel2013,
                    ExportingEventHandler = ExportingHandler,
                    CellsExportingEventHandler = CellExportingHandler
                };
                using (var excelEngine = new ExcelEngine())
                {
                    var workBook = excelEngine.Excel.Workbooks.Create();
                    workBook.Worksheets[0].PageSetup.LeftMargin = exportOptions.LeftMargin;
                    workBook.Worksheets[0].PageSetup.TopMargin = exportOptions.TopMargin;
                    workBook.Worksheets[0].PageSetup.BottomMargin = exportOptions.BottomMargin;
                    workBook.Worksheets[0].PageSetup.RightMargin = exportOptions.RightMargin;
                    workBook.Worksheets[0].PageSetup.Orientation = exportOptions.Orientation;
                    treeGrid.ExportToExcel(workBook, options);
                    if (exportOptions.AutoFitColumns)
                        foreach (var column in workBook.Worksheets[0].Columns)
                            column.AutofitColumns();
                    if (exportOptions.AutoFitRows)
                        foreach (var row in workBook.Worksheets[0].Rows)
                            row.AutofitRows();
                    workBook.Version = ExcelVersion.Excel2010;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.Trim());
            }
        }

        /// <summary>
        ///     Prints the tree grid.
        /// </summary>
        /// <param name="treeGrid">The tree grid.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S125:Sections of code should not be \"commented out\"", Justification = "This was left here on purpose as an example of an alternate way to execute the user's request.")]
        public static void PrintTreeGrid(SfTreeGrid treeGrid)
        {
            try
            {
                var options = new TreeGridPdfExportingOptions
                {
                    AllowIndentColumn = true,
                    FitAllColumnsInOnePage = true,
                    AutoColumnWidth = true
                };
                var document = treeGrid.ExportToPdf(options);
                document = ChangeOrientation(document, true);
                var pdfViewer = new PdfViewerControl();
                var stream = new MemoryStream();
                document.Save(stream);
                var ldoc = new PdfLoadedDocument(stream);
                pdfViewer.Load(ldoc);
                // if you want to  show the pdf viewer window. Please enable the below line.
                //var pdfPage = new Window();
                //pdfPage.Content = pdfViewer;
                //pdfPage.Show();
                pdfViewer.Print(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message.Trim());
            }
        }

        /// <summary>
        ///     Changes the orientation.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="isOrientationChanged">if set to <see langword="true" /> [is orientation changed].</param>
        /// <returns>PdfDocument.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "SG0001:Potential command injection with Process.Start")]
        private static PdfDocument ChangeOrientation(PdfDocument document, bool isOrientationChanged)
        {
            var pageCount = document.Pages.Count;
            document.PageSettings.Margins.All = 0;
            for (var i = 0; i < pageCount; i++)
            {
                var loadedPage = document.Pages[i];
                var width = loadedPage.Size.Width;
                var height = loadedPage.Size.Height;
                if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle90 ||
                    loadedPage.Rotation == PdfPageRotateAngle.RotateAngle270)
                {
                    width = loadedPage.Size.Height;
                    height = loadedPage.Size.Width;
                }

                //Change the orientation
                if (width > height)
                    document.PageSettings.Orientation = PdfPageOrientation.Portrait;
                else
                    document.PageSettings.Orientation = PdfPageOrientation.Landscape;
                document.PageSettings.Size = new SizeF(width, height);

                //Convert landscape to portrait
                if (document.PageSettings.Orientation == PdfPageOrientation.Landscape)
                {
                    ConvertLandscapeToPortrait(document, loadedPage, isOrientationChanged, height, width);
                }
                //Convert portrait to landscape
                else
                {
                    ConvertPortraitToLandscape(document, loadedPage, isOrientationChanged, height, width);
                }
            }
            return document;
        }

        private static void ConvertLandscapeToPortrait(PdfDocument document, PdfPage loadedPage, bool isOrientationChanged, float height, float width)
        {
            var page = document.Pages.Add();
            var graphics = page.Graphics;
            var template = loadedPage.CreateTemplate();
            var state = graphics.Save();
            graphics.TranslateTransform(height / 2, width / 2);
            if (!isOrientationChanged)
                graphics.ScaleTransform(width / height, width / height);
            else
                graphics.ScaleTransform(height / width, height / width);
            if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle90)
            {
                graphics.RotateTransform(90);
                graphics.TranslateTransform(-height / 2, -width / 2);
            }
            else if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle270)
            {
                graphics.RotateTransform(270);
                graphics.TranslateTransform(-height / 2, -width / 2);
            }
            else if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle180)
            {
                graphics.RotateTransform(180);
                graphics.TranslateTransform(-width / 2, -height / 2);
            }
            else
            {
                graphics.TranslateTransform(-width / 2, -height / 2);
            }
            graphics.DrawPdfTemplate(template, new PointF(0, 0));
            graphics.Restore(state);
        }

        private static void ConvertPortraitToLandscape(PdfDocument document, PdfPage loadedPage, bool isOrientationChanged, float height, float width)
        {
            var page = document.Pages.Add();
            var graphics = page.Graphics;
            var template = loadedPage.CreateTemplate();
            var state = graphics.Save();
            graphics.TranslateTransform(height / 2, width / 2);
            if (!isOrientationChanged)
                graphics.ScaleTransform(height / width, height / width);
            else
                graphics.ScaleTransform(width / height, width / height);
            if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle90)
            {
                graphics.RotateTransform(90);
                graphics.TranslateTransform(-height / 2, -width / 2);
            }
            else if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle270)
            {
                graphics.RotateTransform(270);
                graphics.TranslateTransform(-height / 2, -width / 2);
            }
            else if (loadedPage.Rotation == PdfPageRotateAngle.RotateAngle180)
            {
                graphics.RotateTransform(180);
                graphics.TranslateTransform(-width / 2, -height / 2);
            }
            else
            {
                graphics.TranslateTransform(-width / 2, -height / 2);
            }
            graphics.DrawPdfTemplate(template, new PointF(0, 0));
            graphics.Restore(state);
        }

        #region ExportingExcelEventHandler
        /// <summary>
        ///     Handles the TreeGridCellExporting Event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TreeGridCellExcelExportingEventArgs" /> instance containing the event data.</param>
        private static void CellExportingHandler(object sender, TreeGridCellExcelExportingEventArgs e)
        {
            if (e.CellType == TreeGridCellType.RecordCell && e.ColumnName == "Title")
                e.Range.CellStyle.Font.FontName = HeaderFontName;
        }

        /// <summary>
        ///     Handles the TreeGridExcelExporting Event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TreeGridExcelExportingEventArgs" /> instance containing the event data.</param>
        private static void ExportingHandler(object sender, TreeGridExcelExportingEventArgs e)
        {
            if (e.CellType == TreeGridCellType.HeaderCell)
            {
                e.Style.ColorIndex = ExcelKnownColors.Light_yellow;
                e.Style.Font.Color = ExcelKnownColors.Dark_red;
                e.Style.Font.Bold = false;
                e.Style.Font.Size = HeaderFontSize;
                e.Style.Font.FontName = HeaderFontName;
                e.Handled = true;
            }
            else if (e.CellType == TreeGridCellType.RecordCell)
            {
                e.Style.Font.Size = CellFontSize;
                e.Style.Font.FontName = CellFontName;
                e.Handled = true;
            }
        }

        #endregion
    }
}