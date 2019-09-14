namespace Project.Common.ParamContainers
{
    public class PagingFactory : IPagingFactory
    { 
        private IPagingParams _pagingParams;

        public PagingFactory(IPagingParams pagingParams)
        {
            _pagingParams = pagingParams;
        }

        public IPagingParams Build(int pageNumber, int pageSize)
        {
            _pagingParams.PageNumber = pageNumber;
            _pagingParams.PageSize = pageSize;
            return _pagingParams;
        }
    }
}
