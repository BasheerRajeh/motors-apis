using WebApi.Services.Common;

namespace WebApi.ViewModels
{
    public class ArticleVms
    {
    }
    public class ArticleVm
    {
        public ArticleVm()
        {
            Images = new List<FileToken>();
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
        public ICollection<FileToken> Images { get; set; }
    }
}