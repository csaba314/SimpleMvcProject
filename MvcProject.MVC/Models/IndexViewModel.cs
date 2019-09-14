using PagedList;
using Project.Common.ParamContainers;
using System.Collections.Generic;

namespace Project.MVC.Models
{
    public class IndexViewModel<TEntity, TChildEntity>
    {
        public IPagedList<TEntity> EntityList { get; set; }
        public TEntity Entity { get; set; }

        public int Id { get; set; }
        public string Sorting { get; set; }
        public string CurrentFilter { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }

        public IEnumerable<TChildEntity> ChildEntityList { get; set; }
    }
}