using System.ComponentModel.DataAnnotations;

namespace ODataBookStore.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }

    }
    public enum Role
    {
        Admin,
        Customer
    }
}
