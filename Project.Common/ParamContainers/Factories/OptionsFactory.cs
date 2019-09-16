using System;

namespace Project.Common.ParamContainers
{
    public class OptionsFactory : IOptionsFactory
    {
        private IOptions _options;

        public OptionsFactory(IOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(IOptions));
        }

        public IOptions Build(string include)
        {
            _options.Include = include;
            return _options;
        }
    }
}
