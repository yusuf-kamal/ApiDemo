using Core.Entities;

namespace ApiDemo.Helper
{
    public class Pagination<T> where T : class
    {
        public Pagination(int pageIndex, int pageCount, int totalCount, IReadOnlyList<T> date)
        {
            PageIndex = pageIndex;
            PageCount = pageCount;
            TotalCount = totalCount;
            Date = date;
        }

        public int PageIndex { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public IReadOnlyList<T> Date { get; set; }


    }
}
