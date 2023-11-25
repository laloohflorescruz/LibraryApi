using System.ComponentModel.DataAnnotations;
using LibraryApi.Models;

namespace LibraryApi.ViewModel
{
    public class CustomerViewModel : Audit
    {
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Please inset a valid last name")]
        [Display(Name = "Last Name")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 2)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please inset a valid name")]
        [Display(Name = "First Name")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please inset a valid birthday")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Display(Name = "Is a student?")]
        public bool Student { get; set; }

        [Required(ErrorMessage = "Please inset an email")]
        [Display(Name = "eMail")]
        [DataType(DataType.EmailAddress)]
        [StringLength(35, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please inset phone")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(15, ErrorMessage = "Name length can't be more than 15.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please inset an address")]
        [Display(Name = "Address")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string? Address { get; set; }

        [Display(Name = "Membership Since")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]

        [Required(ErrorMessage = "Please inset a valid date")]
        [DataType(DataType.Date)]
        public DateTime MembershipSince { get; set; }
        
        public string? Genre { get; set; }
    }
}