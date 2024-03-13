using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApi.Models.Entities
{
    public class Service
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TitleKz { get; set; }
        public string? TitleRu { get; set; }
        public string? Description { get; set; }
        public string? DescriptionKz { get; set; }
        public string? DescriptionRu { get; set; }
        public string? ImagePath { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

}
