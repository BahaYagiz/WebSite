namespace WebSite.Models
{
        public class User :BaseEntitiy
        {
            public string FullName { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string RoleName { get; set; }
            public string PhotoUrl { get; set; }

            public Role Role { get; set; } 

        }
}

