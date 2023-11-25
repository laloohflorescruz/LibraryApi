using System.ComponentModel.DataAnnotations;
using LibraryApi.Models;

namespace LibraryApi.ViewModel
{
    public class LibraryBranchViewModel : Audit
    {
        public int LibraryBranchId { get; set; }

        [Required(ErrorMessage = "Please inset a valid name")]
        [Display(Name = "Branch Name")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string? BranchName { get; set; }

        [Required(ErrorMessage = "Please inset a valid zipcode")]
        [Display(Name = "ZipCode")]
        [StringLength(6, ErrorMessage = "Zipcode length can't be more than 6.")]

        public string? ZipCode { get; set; }

        [Required(ErrorMessage = "Please inset an address")]
        [Display(Name = "Address")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Please inset a phone number")]
        [Display(Name = "Phone")]
        [StringLength(15, ErrorMessage = "Phone length can't be more than 15.")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Please inset a City")]
        [Display(Name = "City")]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]

        public string? City { get; set; }

        [Required(ErrorMessage = "Please inset an email")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [StringLength(75, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please inset the hours")]
        [Display(Name = "Opening Hours")]
        public string? OpeningHours { get; set; }
    }

}