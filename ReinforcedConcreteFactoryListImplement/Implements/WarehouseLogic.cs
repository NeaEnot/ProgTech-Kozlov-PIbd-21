using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.Interfaces;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using ReinforcedConcreteFactoryListImplement.Models;
using System;
using System.Collections.Generic;

namespace ReinforcedConcreteFactoryListImplement.Implements
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly DataListSingleton source;

        public WarehouseLogic()
        {
            source = DataListSingleton.GetInstance();
        }

        public void CreateOrUpdate(WarehouseBindingModel model)
        {
            Warehouse tempWarehouse = model.Id.HasValue ? null : new Warehouse { Id = 1, WarehouseName = model.WarehouseName };

            foreach (var warehouse in source.Warehouses)
            {
                if (warehouse.WarehouseName == model.WarehouseName && warehouse.Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }

                if (!model.Id.HasValue && warehouse.Id >= tempWarehouse.Id)
                {
                    tempWarehouse.Id = warehouse.Id + 1;
                }
                else if (model.Id.HasValue && warehouse.Id == model.Id)
                {
                    tempWarehouse = warehouse;
                }
            }

            if (model.Id.HasValue)
            {
                if (tempWarehouse == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempWarehouse.WarehouseName = model.WarehouseName;
            }
            else
            {
                source.Warehouses.Add(tempWarehouse);
            }
        }

        public void Delete(WarehouseBindingModel model)
        {
            for (int i = 0; i < source.WarehouseComponents.Count; ++i)
            {
                if (source.WarehouseComponents[i].WarehouseId == model.Id)
                {
                    source.WarehouseComponents.RemoveAt(i--);
                }
            }

            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == model.Id)
                {
                    source.Warehouses.RemoveAt(i);
                    return;
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddComponent(WarehouseComponentBindingModel model)
        {
            for (int i = 0; i < source.WarehouseComponents.Count; ++i)
            {
                if (source.WarehouseComponents[i].WarehouseId == model.WarehouseId &&
                    source.WarehouseComponents[i].ComponentId == model.ComponentId)
                {
                    source.WarehouseComponents[i].Count += model.Count;
                    model.Id = source.WarehouseComponents[i].Id;
                    return;
                }
            }

            int maxWCId = 0;

            for (int i = 0; i < source.WarehouseComponents.Count; ++i)
            {
                if (source.WarehouseComponents[i].Id > maxWCId)
                {
                    maxWCId = source.WarehouseComponents[i].Id;
                }
            }

            if (model.Id == 0)
            {
                source.WarehouseComponents.Add(new WarehouseComponent
                {
                    Id = ++maxWCId,
                    WarehouseId = model.WarehouseId,
                    ComponentId = model.ComponentId,
                    Count = model.Count
                });
            }
        }

        public List<WarehouseViewModel> Read(WarehouseBindingModel model)
        {
            List<WarehouseViewModel> result = new List<WarehouseViewModel>();

            foreach (var warehouse in source.Warehouses)
            {
                if (model != null)
                {
                    if (warehouse.Id == model.Id)
                    {
                        result.Add(CreateViewModel(warehouse));
                        break;
                    }

                    continue;
                }

                result.Add(CreateViewModel(warehouse));
            }

            return result;
        }

        private WarehouseViewModel CreateViewModel(Warehouse warehouse)
        {

            Dictionary<int, (string, int)> warehouseComponents = new Dictionary<int, (string, int)>();

            foreach (var wc in source.WarehouseComponents)
            {
                if (wc.WarehouseId == warehouse.Id)
                {
                    string componentName = string.Empty;

                    foreach (var component in source.Components)
                    {
                        if (wc.ComponentId == component.Id)
                        {
                            componentName = component.ComponentName;
                            break;
                        }
                    }

                    warehouseComponents.Add(wc.ComponentId, (componentName, wc.Count));
                }
            }

            return new WarehouseViewModel
            {
                Id = warehouse.Id,
                WarehouseName = warehouse.WarehouseName,
                WarehouseComponents = warehouseComponents
            };
        }

        public void WriteOffComponents(OrderViewModel model)
        {
            // Заглушка
        }
    }
}
