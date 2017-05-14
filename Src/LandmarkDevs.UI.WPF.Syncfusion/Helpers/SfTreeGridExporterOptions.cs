#region
using System.Collections.Generic;
using Syncfusion.XlsIO;
#endregion

namespace LandmarkDevs.UI.WPF.Syncfusion.Helpers
{
    /// <summary>
    ///     Class SfTreeGridExporterOptions.
    /// </summary>
    public class SfTreeGridExporterOptions
    {
        /// <summary>
        ///     The default file name
        /// </summary>
        private string _defaultFileName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SfTreeGridExporterOptions" /> class.
        /// </summary>
        public SfTreeGridExporterOptions()
        {
            ExcludedColumns = new List<string>();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether [allow indent column].
        /// </summary>
        /// <value><c>true</c> if [allow indent column]; otherwise, <c>false</c>.</value>
        public bool AllowIndentColumn { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [only checked nodes].
        /// </summary>
        /// <value><c>true</c> if [only checked nodes]; otherwise, <c>false</c>.</value>
        public bool OnlyCheckedNodes { get; set; }

        /// <summary>
        ///     Gets or sets the default name of the file.
        /// </summary>
        /// <value>The default name of the file.</value>
        public string DefaultFileName
        {
            get { return _defaultFileName.Replace('/', '-'); }
            set { _defaultFileName = value; }
        }

        /// <summary>
        ///     Gets or sets the margins.
        /// </summary>
        /// <value>The margins.</value>
        public double[] Margins { get; set; } = { 0.25, 0.25, 0.25, 0.25 };

        /// <summary>
        ///     Gets the left margin.
        /// </summary>
        /// <value>The left margin.</value>
        public double LeftMargin => Margins[0];

        /// <summary>
        ///     Gets the top margin.
        /// </summary>
        /// <value>The top margin.</value>
        public double TopMargin => Margins[1];

        /// <summary>
        ///     Gets the right margin.
        /// </summary>
        /// <value>The right margin.</value>
        public double RightMargin => Margins[2];

        /// <summary>
        ///     Gets the bottom margin.
        /// </summary>
        /// <value>The bottom margin.</value>
        public double BottomMargin => Margins[3];

        /// <summary>
        ///     Gets or sets the orientation.
        /// </summary>
        /// <value>The orientation.</value>
        public ExcelPageOrientation Orientation { get; set; } = ExcelPageOrientation.Portrait;

        /// <summary>
        ///     Gets or sets the name of the header font.
        /// </summary>
        /// <value>The name of the header font.</value>
        public string HeaderFontName { get; set; } = "Calibri";

        /// <summary>
        ///     Gets or sets the name of the cell font.
        /// </summary>
        /// <value>The name of the cell font.</value>
        public string CellFontName { get; set; } = "Calibri";

        /// <summary>
        ///     Gets or sets the size of the header font.
        /// </summary>
        /// <value>The size of the header font.</value>
        public int HeaderFontSize { get; set; } = 11;

        /// <summary>
        ///     Gets or sets the size of the cell font.
        /// </summary>
        /// <value>The size of the cell font.</value>
        public int CellFontSize { get; set; } = 11;

        /// <summary>
        ///     Gets or sets a value indicating whether [automatic fit rows].
        /// </summary>
        /// <value><c>true</c> if [automatic fit rows]; otherwise, <c>false</c>.</value>
        public bool AutoFitRows { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [automatic fit columns].
        /// </summary>
        /// <value><c>true</c> if [automatic fit columns]; otherwise, <c>false</c>.</value>
        public bool AutoFitColumns { get; set; }

        /// <summary>
        ///     Gets or sets the default folder path.
        /// </summary>
        /// <value>The default folder path.</value>
        public string DefaultFolderPath { get; set; }

        /// <summary>
        ///     Gets or sets the excluded columns.
        /// </summary>
        /// <value>The excluded columns.</value>
        public List<string> ExcludedColumns { get; set; }
    }
}