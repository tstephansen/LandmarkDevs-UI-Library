using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace LandmarkDevs.UI.Material.Helpers
{
    /// <summary>
    ///     Class DirectoryHelper.
    /// </summary>
    public static class DirectoryHelper
    {
        /// <summary>
        ///     Searches the active directory for key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static object SearchDirectoryForKey(string key)
        {
            var username = UserPrincipal.Current.Name;
            using (var ds = new DirectorySearcher())
            {
                ds.Filter = "(&(objectClass=user) (cn=" + username + "))";
                var usr = ds.FindOne();
                if (usr == null)
                    return null;
                using (var de = new DirectoryEntry(usr.Path))
                {
                    var returnedValue = de.Properties[key].Value;
                    return returnedValue;
                }
            }
        }

        /// <summary>
        /// Searches the directory for the key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="username">The username.</param>
        /// <returns>System.Object.</returns>
        public static object SearchDirectoryForKey(string key, string username)
        {
            using (var ds = new DirectorySearcher())
            {
                string usrPath;
                ds.Filter = "(&" +
                            "(objectClass=user)" +
                            "(CN=" + username + "))";
                var usr = ds.FindOne();
                if (usr == null)
                {
                    ds.Filter = "(&" +
                                "(objectClass=user)" +
                                "(samaccountname=" + username + "))";
                    usr = ds.FindOne();
                }
                if (usr == null)
                {
                    usrPath = FindUserInActiveDirectory(username);
                    if (string.IsNullOrWhiteSpace(usrPath))
                        return null;
                }
                else
                {
                    usrPath = usr.Path;
                }
                using (var de = new DirectoryEntry(usrPath))
                {
                    var returnedValue = de.Properties[key].Value;
                    return returnedValue;
                }
            }
        }

        /// <summary>
        ///     Gets the user thumbnail photo from active directory as an image.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>System.Object.</returns>
        public static BitmapImage GetUserThumbnailPhotoAsImage(string username)
        {
            var photo = SearchDirectoryForKey("thumbnailphoto", username) as byte[];
            if (photo == null)
            {
                var thumbPhoto = SearchDirectoryForKey("thumbnailphoto", username) as byte[];
                if (thumbPhoto == null)
                    return new BitmapImage(new Uri("pack://application:,,,/LandmarkDevs.UI.Material;component/Images/Icons/PersonPlaceholder.png"));
                using (var stream = new MemoryStream(thumbPhoto))
                {
                    var userImage = Image.FromStream(stream);
                    return GetImageSource(userImage);
                }
            }
            using (var memoryStream = new MemoryStream(photo))
            {
                var userImage = Image.FromStream(memoryStream);
                return GetImageSource(userImage);
            }
        }

        /// <summary>
        /// Gets the image source.
        /// </summary>
        /// <param name="userImage">The user image.</param>
        /// <returns>BitmapImage.</returns>
        public static BitmapImage GetImageSource(Image userImage)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            var ms = new MemoryStream();
            userImage.Save(ms, ImageFormat.Bmp);
            ms.Seek(0, SeekOrigin.Begin);
            bi.StreamSource = ms;
            bi.EndInit();
            return bi;
        }

        /// <summary>
        /// Finds the user in active directory.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>System.String.</returns>
        public static string FindUserInActiveDirectory(string username)
        {
            using (var ds = new DirectorySearcher())
            {
                ds.Filter = "(&(objectClass=user))";
                var users = ds.FindAll();
                if (users.Count == 0)
                    return null;
                foreach (SearchResult searchResult in users)
                    if (searchResult.Properties["CN"][0].ToString() == username || searchResult.Properties["samaccountname"][0].ToString() == username)
                        return searchResult.Path;
            }
            return null;
        }
    }
}