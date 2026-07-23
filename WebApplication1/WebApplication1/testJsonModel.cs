using System.ComponentModel.DataAnnotations;

namespace WebApplication1
{
    public record testJsonModel
    {
        [Required, MinLength(1)]
        public required string Name { get; set; }
    }
}
