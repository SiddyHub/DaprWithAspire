using System;
using System.ComponentModel.DataAnnotations;

namespace GloboTicket.Web.Models.View
{
    public class BasketCheckoutViewModel
    {
        public Guid BasketId { get; set; }
        public Guid UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Zip code")]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(100)]
        [Display(Name = "First name")]
        public string City { get; set; }
        [Required]
        [StringLength(100)]
        public string Country { get; set; }


        //payment information
        [Required]
        [StringLength(16)]
        [DataType(DataType.CreditCard)]
        [Display(Name = "Credit card number")]
        public string CardNumber { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Credit card name")]
        public string CardName { get; set; }
        [Required]
        [StringLength(4)]
        [Display(Name = "Expiration date")]
        public string CardExpiration { get; set; }
        [Required]
        [StringLength(3)]
        [Display(Name = "CVV code")]
        public string CvvCode { get; set; }
    }
}
