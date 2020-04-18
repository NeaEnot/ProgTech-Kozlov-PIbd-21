using System.Collections.Generic;

namespace ReinforcedConcreteFactoryBusinessLogic.BindingModels
{
    public class ProductBindingModel
    {
        public int? Id { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public Dictionary<int, (string, int)> ProductComponents { get; set; }
    }
}
