using System;
using System.Collections.Generic;

namespace ReinforcedConcreteFactoryBusinessLogic.ViewModels
{
    public class ReportProductComponentViewModel
    {
        public string ComponentName { get; set; }

        public int TotalCount { get; set; }

        public List<Tuple<string, int>> Products { get; set; }
}
}
