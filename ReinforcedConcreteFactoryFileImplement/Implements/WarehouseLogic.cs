using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.Interfaces;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using ReinforcedConcreteFactoryFileImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReinforcedConcreteFactoryFileImplement.Implements
{
    public class WarehouseLogic : IWarehouseLogic
    {
        private readonly FileDataListSingleton source;

        public WarehouseLogic()
        {
            source = FileDataListSingleton.GetInstance();
        }

        public void CreateOrUpdate(WarehouseBindingModel model)
        {
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.WarehouseName == model.WarehouseName && rec.Id != model.Id);

            if (element != null)
            {
                throw new Exception("Уже есть склад с таким названием");
            }

            if (model.Id.HasValue)
            {
                element = source.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);

                if (element == null)
                {
                    throw new Exception("Элемент не найден");
                }
            }
            else
            {
                int maxId = source.Warehouses.Count > 0 ? source.Warehouses.Max(rec => rec.Id) : 0;
                element = new Warehouse { Id = maxId + 1 };
                source.Warehouses.Add(element);
            }

            element.WarehouseName = model.WarehouseName;
        }

        public void Delete(WarehouseBindingModel model)
        {
            source.WarehouseComponents.RemoveAll(rec => rec.WarehouseId == model.Id);
            Warehouse element = source.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);

            if (element != null)
            {
                source.Warehouses.Remove(element);
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public void AddComponent(WarehouseComponentBindingModel model)
        {
            Warehouse warehouse = source.Warehouses.FirstOrDefault(rec => rec.Id == model.WarehouseId);

            if (warehouse == null)
            {
                throw new Exception("Склад не найден");
            }

            Component component = source.Components.FirstOrDefault(rec => rec.Id == model.ComponentId);

            if (component == null)
            {
                throw new Exception("Компонент не найден");
            }

            WarehouseComponent element = source.WarehouseComponents
                        .FirstOrDefault(rec => rec.WarehouseId == model.WarehouseId && rec.ComponentId == model.ComponentId);

            if (element != null)
            {
                element.Count += model.Count;
                return;
            }

            source.WarehouseComponents.Add(new WarehouseComponent
            {
                Id = source.WarehouseComponents.Count > 0 ? source.WarehouseComponents.Max(rec => rec.Id) + 1 : 0,
                WarehouseId = model.WarehouseId,
                ComponentId = model.ComponentId,
                Count = model.Count
            });
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

        public List<WarehouseViewModel> Read(WarehouseBindingModel model)
        {
            return source.Warehouses
            .Where(rec => model == null || rec.Id == model.Id)
            .Select(rec => new WarehouseViewModel
            {
                Id = rec.Id,
                WarehouseName = rec.WarehouseName,
                WarehouseComponents = source.WarehouseComponents
                                    .Where(recWC => recWC.WarehouseId == rec.Id)
                                    .ToDictionary(
                                        recWC => recWC.ComponentId,
                                        recWC => (
                                            source.Components.FirstOrDefault(recC => recC.Id == recWC.ComponentId)?.ComponentName, recWC.Count
                                            )
                                        )
            })
            .ToList();
        }

        public bool WriteOffComponents(OrderViewModel model)
        {
            var product = source.Products.Where(rec => rec.Id == model.ProductId).FirstOrDefault();

            if (product == null)
            {
                throw new Exception("Заказ не найден");
            }

            var productComponents = source.ProductComponents.Where(rec => rec.ProductId == product.Id).ToList();

            if (productComponents == null)
            {
                throw new Exception("Не найдена связь продукта с компонентами");
            }

            foreach (var pc in productComponents)
            {
                var warehouseComponent = source.WarehouseComponents.Where(rec => rec.ComponentId == pc.ComponentId);
                int sum = warehouseComponent.Sum(rec => rec.Count);

                if (sum < pc.Count * model.Count)
                {
                    return false;
                }
            }

            foreach (var pc in productComponents)
            {
                var warehouseComponent = source.WarehouseComponents.Where(rec => rec.ComponentId == pc.ComponentId);
                int neededCount = pc.Count;

                foreach (var wc in warehouseComponent)
                {
                    if (wc.Count >= neededCount)
                    {
                        wc.Count -= neededCount;
                        break;
                    }
                    else
                    {
                        neededCount -= wc.Count;
                        wc.Count = 0;
                    }
                }
            }

            return true;
        }
    }
}
