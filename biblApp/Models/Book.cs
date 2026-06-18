using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }

        [Display(Name = "Количество страниц")]
        public int Pages { get; set; }

        [Display(Name = "Автор")]
        public int AuthorId { get; set; }

        [Display(Name = "Автор")]
        public Author? Author { get; set; }
    }
}