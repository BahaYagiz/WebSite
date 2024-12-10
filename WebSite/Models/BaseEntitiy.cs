namespace WebSite.Models
{
    public class BaseEntitiy
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }

        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
