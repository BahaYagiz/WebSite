using System.ComponentModel;

namespace WebSite.Models
{
    public class Report:BaseEntitiy
    {

        public string Title { get; set; }
        
        public string Content { get; set; }

        public string AuthorName { get; set; }


        public string PhotoUrl { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }

    }
}
