using System.Collections.Generic;
using ServiceLocator;

namespace DynamicLoader
{
    public class DynamicLoaderUnloader : IService
    {
        public List<LoadedLevelElement> elements;
    }
}