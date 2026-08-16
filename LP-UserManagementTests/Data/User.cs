
namespace LP_UserManagementTests.Data
{
    /// <summary>
    /// Represent a user
    /// </summary>
    public class User(string name, string email, int age)
    {
        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; } = name;
        /// <summary>
        /// User email address
        /// </summary>
        public string Email { get; set; } = email;
        /// <summary>
        /// User Age
        /// </summary>
        public int Age { get; set; } = age;
    }
}
