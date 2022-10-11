using System.ComponentModel.DataAnnotations;


namespace Payment.API.Models
{
    public class CardPaymentDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string CardName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string Expiration { get; set; }
        [Required]
        public string CVV { get; set; }
        [Required]
        public bool Default { get; set; }
    }
}
