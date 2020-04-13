using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using System.Collections.Generic;

namespace ReinforcedConcreteFactoryBusinessLogic.Interfaces
{
    public interface IMessageInfoLogic
    {
        List<MessageInfoViewModel> Read(MessageInfoBindingModel model);

        void Create(MessageInfoBindingModel model);
    }
}
