using System.Collections.Generic;
using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;

namespace ReinforcedConcreteFactoryBusinessLogic.Interfaces
{
    public interface IProductLogic
    {
        List<ProductViewModel> Read(ProductBindingModel model);

        void CreateOrUpdate(ProductBindingModel model);

        void Delete(ProductBindingModel model);
    }
}
