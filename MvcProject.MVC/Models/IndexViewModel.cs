using PagedList;
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

        public ControllerParameters ControllerParameters { get; set; }

        public IEnumerable<TChildEntity> ChildEntityList { get; set; }
    }
}