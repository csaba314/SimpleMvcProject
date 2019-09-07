using MvcProject.MVC.Models;
using Project.Service.ParamContainers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.MVC.PresentationService
{
    public static class ViewModelManager
    {
        public static void SetParams<TEntity, TChildEntity>(ref IndexViewModel<TEntity, TChildEntity> viewModel, 
                                                            IParamsFactory paramsFactory,
                                                            string searchString, 
                                                            string currentFilter,                
                                                            string sorting, 
                                                            int pageSize, 
                                                            int pageNumber,
                                                            bool loadMakesWithModel = false)
        {
            viewModel.FilteringParams = paramsFactory.FilteringParamsInstance();
            viewModel.SortingParams = paramsFactory.SortingParamsInstance();
            viewModel.PagingParams = paramsFactory.PagingParamsInstance();
            viewModel.Options = paramsFactory.IOptionsInstance();

            viewModel.FilteringParams.SearchString = searchString;
            viewModel.FilteringParams.CurrentFilter = currentFilter;
            viewModel.PagingParams.PageSize = pageSize;
            viewModel.PagingParams.PageNumber = pageNumber;
            viewModel.SortingParams.Sorting = sorting;
            viewModel.Options.LoadMakesWithModel = loadMakesWithModel;
        }
    }
}