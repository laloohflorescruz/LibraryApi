using System.ComponentModel.DataAnnotations;
using LibraryApi.Models;

namespace LibraryApi.ViewModel
{
    public class AuthorViewModel : Audit
    {
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Please insert the First Name")]
        [Display(Name = "Last name")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please insert the Last Name")]
        [Display(Name = "First name")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string? FirstName { get; set; }

        [Display(Name = "Birth Place")]
        public string? BirthPlace { get; set; }

        [Display(Name = "Has this author won any Nobel Prize")]
        public bool NobelPrize { get; set; }

        public List<Author>? AuthorList { get; set; }
    }
}