using static System.Net.Mime.MediaTypeNames;
using WebApi.Services.Common;

namespace WebApi.ViewModels
{
    public class ServiceVms
    {
    }

    public class ServiceVm
    {
        public ServiceVm()
        {
            Images = new HashSet<FileToken>();
        }
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? TitleKz { get; set; }
        public string? TitleRu { get; set; }
        public string? Description { get; set; }
        public string? DescriptionKz { get; set; }
        public string? DescriptionRu { get; set; }
        public string? ImagePath { get; set; }
        public bool Active { get; set; }
        public ICollection<FileToken> Images { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
