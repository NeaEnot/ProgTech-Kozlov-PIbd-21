using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinforcedConcreteFactoryBusinessLogic.BindingModels
{
    public class WarehouseBindingModel
    {
        public int Id { get; set; }
        public string WarehouseName { get; set; }
        public List<WarehouseComponentBindingModel> WarehouseComponent { get; set; }
    }
}
