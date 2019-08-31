using PagedList;
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


        public string Sorting { get; set; }
        public string CurrentFilter { get; set; }
        public SelectList PageSizeDropdown { get; set; }

        public IEnumerable<TChildEntity> ChildEntityList { get; set; }
    }
}