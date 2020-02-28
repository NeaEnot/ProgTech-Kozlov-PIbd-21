﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinforcedConcreteFactoryBusinessLogic.BindingModels
{
    public class WarehouseComponentBindingModel
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int ComponentId { get; set; }
        public int Count { get; set; }
    }
}
