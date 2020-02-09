using System;
using System.Collections.Generic;
using System.Text;

namespace ReinforcedConcreteFactoryBusinessLogic.BindingModels
{
    public class OrderBindingModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}
