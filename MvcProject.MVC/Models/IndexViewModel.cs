using PagedList;
using Project.Common.ParamContainers;
using System.Collections.Generic;

namespace Project.MVC.Models
{
    public class IndexViewModel<TEntity, TChildEntity>
    {
        public IPagedList<TEntity> EntityList { get; set; }
        public TEntity Entity { get; set; }

        public ISortingParams SortingParams { get; set; }
        public IFilteringParams FilteringParams { get; set; }
        public IPagingParams PagingParams { get; set; }
        public IOptions Options { get; set; }

        public IEnumerable<TChildEntity> ChildEntityList { get; set; }
    }
}