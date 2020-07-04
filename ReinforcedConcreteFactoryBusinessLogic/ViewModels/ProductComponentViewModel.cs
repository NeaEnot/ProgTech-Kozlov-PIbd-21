using ReinforcedConcreteFactoryBusinessLogic.Attributes;
using ReinforcedConcreteFactoryBusinessLogic.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReinforcedConcreteFactoryBusinessLogic.ViewModels
{
    [DataContract]
    public class ProductComponentViewModel : BaseViewModel
    {
        [DataMember]
        public int ProductId { get; set; }

        [DataMember]
        public int ComponentId { get; set; }

        [Column(title: "Компонент", gridViewAutoSize: GridViewAutoSize.Fill)]
        [DataMember]
        public string ComponentName { get; set; }

        [Column(title: "Количество", width: 100)]
        [DataMember]
        public int Count { get; set; }

        public override List<string> Properties() => new List<string>
        {
            "Id",
            "ComponentName",
            "Count",
            "ImplementerFIO",
            "Count",
            "Sum",
            "Status",
            "DateCreate",
            "DateImplement"
        };
    }
}
