using System.Collections.Generic;

namespace DataModel.Common
{
    public class Page<T>
    {
        public IList<T> Data { get; set; }
        public int Total { get; set; }
    }
}