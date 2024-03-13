using WebApi.ViewModels;

namespace WebApi.Common.Models
{
    public class DataPageModel<T>
    {
        public DataPageModel()
        {
            Data = new List<T>();
        }
        public int Count { get; set; }
        public double PageCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}
