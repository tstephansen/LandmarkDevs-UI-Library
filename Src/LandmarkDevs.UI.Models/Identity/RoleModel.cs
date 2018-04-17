using System;

namespace LandmarkDevs.UI.Models.Identity
{
    /// <summary>
    ///     Class RoleModel.
    /// </summary>
    public class RoleModel
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value><see langword="true" /> if this instance is selected; otherwise, <see langword="false" />.</value>
        public bool IsSelected { get; set; }

        /// <summary>
        ///     Gets or sets the name of the role.
        /// </summary>
        /// <value>The name of the role.</value>
        public string RoleName { get; set; }
    }
}