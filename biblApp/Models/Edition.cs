using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblApp.Models
{
    public class Edition
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [NotMapped]
        public List<int> SelectedAuthorIds { get; set; } = new();

        [Display(Name = "Описание")]
        public string Description { get; set; }
         
        public List<Author> Authors { get; set; } = new();
    }
}