#region
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
#endregion

namespace LandmarkDevs.UI.Material.Models
{
    /// <summary>
    /// Class IdentityCardModel.
    /// </summary>
    public class IdentityCardModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityCardModel"/> class.
        /// </summary>
        public IdentityCardModel()
        {
            Roles = new ObservableCollection<RoleModel>();
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the user domain.
        /// </summary>
        /// <value>The user domain.</value>
        public string UserDomain { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is administrator.
        /// </summary>
        /// <value><c>true</c> if this instance is administrator; otherwise, <c>false</c>.</value>
        public bool IsAdministrator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is manufacturing user.
        /// </summary>
        /// <value><c>true</c> if this instance is manufacturing user; otherwise, <c>false</c>.</value>
        public bool IsManufacturingUser { get; set; }

        /// <summary>
        /// Gets or sets the user image source.
        /// </summary>
        /// <value>The user image source.</value>
        public ImageSource UserImageSource { get; set; }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <returns>System.String.</returns>
        private string GetFullName()
        {
            if (FirstName == null)
                return LastName;
            var fullName = FirstName;
            if (LastName != null)
                fullName = $"{FirstName} {LastName}";
            return fullName;
        }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>The roles.</value>
        public ObservableCollection<RoleModel> Roles { get; set; }
    }
}