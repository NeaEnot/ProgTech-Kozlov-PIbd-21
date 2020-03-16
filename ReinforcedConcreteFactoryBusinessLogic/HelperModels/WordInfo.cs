using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ReinforcedConcreteFactoryBusinessLogic.HelperModels
{
    class WordInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ComponentViewModel> Components { get; set; }
    }
}
