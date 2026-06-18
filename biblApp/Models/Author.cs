using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace biblApp.Models
{
    public class Author
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [NotMapped]
        public List<int> SelectedEditionIds { get; set; } = new();

        [Display(Name = "Биография")]
        public string Biography { get; set; }

        public List<Book> Books { get; set; } = new();

        public List<Edition> Editions { get; set; } = new();
    }
}