using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryApi.Models;

namespace LibraryApi.ViewModel
{
    public class BookViewModel : Audit
    {
    public int BookId { get; set; }
    
    [Required(ErrorMessage = "Please insert the  title")]
    [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
    [Display(Name = "Title")]
    public string? Title { get; set; }

    [Required(ErrorMessage = "Please insert the  ISBN")]
    [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
    [Display(Name = "ISBN")]
    public string? ISBN { get; set; }
    
    [Required(ErrorMessage = "Please insert the Publication date")]
    [Display(Name = "Publication")]
    [DataType(DataType.Date)]
    public DateTime PublicationDate { get; set; }
    public string? Genre { get; set; }

    [ForeignKey("AuthorId")]
    [Display(Name = "Author")]
    public int AuthorId { get; set; }

    [ForeignKey("LibraryBranchId")]
    [Display(Name = "Branch")]
    public int LibraryBranchId { get; set; }
    
   }
}