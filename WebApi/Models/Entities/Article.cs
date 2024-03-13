namespace WebApi.Models.Entities
{
    public class Article
    {
        public Article()
        {
            Images = new HashSet<ArticleImage>();
        }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? ParagraphOne { get; set; }
        public string? ParagraphTwo { get; set; }
        public string? Language { get; set; }
        public string? ImagePath { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public ICollection<ArticleImage> Images { get; set; }

    }


    public class ArticleImage
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article? Article { get; set; }
        public string? Path { get; set; }
        public string? Name { get; set; }
        public string? MimeType { get; set; }
    }
}
