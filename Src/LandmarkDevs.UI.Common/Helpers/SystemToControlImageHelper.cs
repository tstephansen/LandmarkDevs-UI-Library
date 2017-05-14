using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace LandmarkDevs.UI.Common.Helpers
{
    /// <summary>
    /// Class SystemToControlImageHelper.
    /// </summary>
    public static class SystemToControlImageHelper
    {
        /// <summary>
        /// Converts to controls image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>BitmapImage.</returns>
        public static BitmapImage ConvertToControlsImage(Image image)
        {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }
    }
}
