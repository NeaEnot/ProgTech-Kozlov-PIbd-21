using System.Runtime.Serialization;

namespace ReinforcedConcreteFactoryBusinessLogic.BindingModels
{
    [DataContract]
    public class WarehouseComponentBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int WarehouseId { get; set; }

        [DataMember]
        public int ComponentId { get; set; }
        
        [DataMember]
        public int Count { get; set; }
    }
}
