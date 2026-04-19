namespace BlazorProject.API.DTO
{
    public class UpdatePostDTO
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
    }
}
