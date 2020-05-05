using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace ReinforcedConcreteFactoryBusinessLogic.ViewModels
{
    [DataContract]
    public class WarehouseViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        [DisplayName("Название склада")]
        public string WarehouseName { get; set; }

        [DataMember]
        public Dictionary<int, (string, int)> WarehouseComponents { get; set; }
    }
}
