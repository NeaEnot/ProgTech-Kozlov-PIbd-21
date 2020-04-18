using Microsoft.AspNetCore.Mvc;
using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.Interfaces;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using ReinforcedConcreteFactoryRestApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace ReinforcedConcreteFactoryRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseLogic _warehouse;
        private readonly IComponentLogic _component;

        public WarehouseController(IWarehouseLogic warehouse, IComponentLogic component)
        {
            _warehouse = warehouse;
            _component = component;
        }

        [HttpGet]
        public List<WarehouseModel> GetWarehousesList() => _warehouse.Read(null)?.Select(rec => Convert(rec)).ToList();

        [HttpGet]
        public List<ComponentViewModel> GetComponentsList() => _component.Read(null)?.ToList();

        [HttpGet]
        public WarehouseModel GetWarehouse(int warehouseId) => Convert(_warehouse.Read(new WarehouseBindingModel { Id = warehouseId })?[0]);

        [HttpPost]
        public void CreateOrUpdateWarehouse(WarehouseBindingModel model) => _warehouse.CreateOrUpdate(model);

        [HttpPost]
        public void DeleteWarehouse(WarehouseBindingModel model) => _warehouse.Delete(model);

        [HttpPost]
        public void ReplanishWarehouse(WarehouseComponentBindingModel model) => _warehouse.AddComponent(model);

        private WarehouseModel Convert(WarehouseViewModel model)
        {
            if (model == null) return null;

            return new WarehouseModel
            {
                Id = model.Id,
                WarehouseName = model.WarehouseName
            };
        }
    }
}
