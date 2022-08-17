using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Api.Models.Book
{
    public class BookCreateDto
    {
        [Required]
        [StringLength(50)]
        public string? Title { get; set; }
        public int? Year { get; set; }
        [StringLength(50)]
        public string Isbn { get; set; } = null!;
        [StringLength(250)]
        public string? Summary { get; set; }
        [StringLength(50)]
        public string? Image { get; set; }
        public decimal? Price { get; set; }
        public int? AuthorId { get; set; }
    }
}
