using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ReinforcedConcreteFactoryBusinessLogic.HelperModels
{
    class PdfInfo
    {
        public string FileName { get; set; }

        public string Title { get; set; }

        public List<ReportProductComponentViewModel> ProductComponents { get; set; }

        public List<ReportWarehouseComponentViewModel> WarehouseComponents { get; set; }
    }
}
