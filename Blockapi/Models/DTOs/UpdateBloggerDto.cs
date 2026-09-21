namespace Blockapi.Models.DTOs
{
    public class UpdateBloggerDto
    {
        public string Name { get; set; }
        public string? Email { get; set; }

        public int Age { get; set; }

        public string? password { get; set; }
    }
}
