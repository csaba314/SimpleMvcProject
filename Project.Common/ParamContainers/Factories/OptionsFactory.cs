namespace Project.Common.ParamContainers
{
    public class OptionsFactory : IOptionsFactory
    {
        private IOptions _options;

        public OptionsFactory(IOptions options)
        {
            _options = options;
        }

        public IOptions Build(string include)
        {
            _options.Include = include;
            return _options;
        }
    }
}
