using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace ReinforcedConcreteFactoryBusinessLogic.ViewModels
{
    public class WarehouseViewModel
    {
        public int Id { get; set; }

        [DisplayName("Название склада")]
        public string WarehouseName { get; set; }

        public Dictionary<int, (string, int)> WarehouseComponents { get; set; }
    }
}
