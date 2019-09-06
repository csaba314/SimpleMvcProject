using PagedList;
using Project.Service.ParamContainers;
using Project.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcProject.MVC.Models
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