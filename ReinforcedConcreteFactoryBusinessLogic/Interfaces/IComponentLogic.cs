using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;

namespace ReinforcedConcreteFactoryBusinessLogic.Interfaces
{
    public interface IComponentLogic
    {
        List<ComponentViewModel> GetList();

        ComponentViewModel GetElement(int id);

        void AddElement(ComponentBindingModel model);

        void UpdElement(ComponentBindingModel model);

        void DelElement(int id);
    }
}
