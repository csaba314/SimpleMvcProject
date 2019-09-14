namespace Project.Common.ParamContainers
{
    public class SortingFactory : ISortingFactory
    {
        private ISortingParams _sortingParams;

        public SortingFactory(ISortingParams sortingParams)
        {
            _sortingParams = sortingParams;
        }
        public ISortingParams Build(string sorting)
        {
            _sortingParams.Sorting = sorting;
            return _sortingParams;
        }
    }
}
