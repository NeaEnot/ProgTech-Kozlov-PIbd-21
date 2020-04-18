using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReinforcedConcreteFactoryBusinessLogic.BindingModels
{
    [DataContract]
    public class WarehouseBindingModel
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        public string WarehouseName { get; set; }

        [DataMember]
        public List<WarehouseComponentBindingModel> WarehouseComponent { get; set; }
    }
}
