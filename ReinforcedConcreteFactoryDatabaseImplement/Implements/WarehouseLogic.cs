using Microsoft.EntityFrameworkCore;
using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.Interfaces;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using ReinforcedConcreteFactoryDatabaseImplement.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReinforcedConcreteFactoryDatabaseImplement.Implements
{
    public class WarehouseLogic : IWarehouseLogic
    {
        public void CreateOrUpdate(WarehouseBindingModel model)
        {
            using (var context = new ReinforcedConcreteFactoryDatabase())
            {
                Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.WarehouseName == model.WarehouseName && rec.Id != model.Id);

                if (element != null)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }

                if (model.Id.HasValue)
                {
                    element = context.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);

                    if (element == null)
                    {
                        throw new Exception("Элемент не найден");
                    }
                }
                else
                {
                    element = new Warehouse();
                    context.Warehouses.Add(element);
                }

                element.WarehouseName = model.WarehouseName;

                context.SaveChanges();
            }
        }

        public void Delete(WarehouseBindingModel model)
        {
            using (var context = new ReinforcedConcreteFactoryDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.WarehouseComponents.RemoveRange(context.WarehouseComponents.Where(rec => rec.WarehouseId == model.Id));
                        Warehouse element = context.Warehouses.FirstOrDefault(rec => rec.Id == model.Id);

                        if (element != null)
                        {
                            context.Warehouses.Remove(element);
                            context.SaveChanges();
                        }
                        else
                        {
                            throw new Exception("Элемент не найден");
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void AddComponent(WarehouseComponentBindingModel model)
        {
            using (var context = new ReinforcedConcreteFactoryDatabase())
            {
                WarehouseComponent element = 
                    context.WarehouseComponents.FirstOrDefault(rec => rec.WarehouseId == model.WarehouseId && rec.ComponentId != model.ComponentId);

                if (element != null)
                {
                    element.Count += model.Count;
                }
                else
                {
                    element = new WarehouseComponent
                    {
                        Id = context.WarehouseComponents.Max(rec => rec.Id) + 1,
                        WarehouseId = model.WarehouseId,
                        ComponentId = model.ComponentId,
                        Count = model.Count
                    };

                    context.WarehouseComponents.Add(element);
                }

                context.SaveChanges();
            }
        }

        public List<WarehouseViewModel> Read(WarehouseBindingModel model)
        {
            using (var context = new ReinforcedConcreteFactoryDatabase())
            {
                return context.Warehouses
                .Where(rec => model == null || rec.Id == model.Id)
                .ToList()
                .Select(rec => new WarehouseViewModel
                {
                    Id = rec.Id,
                    WarehouseName = rec.WarehouseName,
                    WarehouseComponents = context.WarehouseComponents
                                                .Include(recWC => recWC.Component)
                                                .Where(recWC => recWC.WarehouseId == rec.Id)
                                                .ToDictionary(recWC => recWC.ComponentId, recWC => (
                                                    recWC.Component?.ComponentName, recWC.Count
                                                ))
                })
                .ToList();
            }
        }

        public void WriteOffComponents(OrderViewModel model)
        {
            using (var context = new ReinforcedConcreteFactoryDatabase())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var productComponents = context.ProductComponents.Where(rec => rec.ProductId == model.ProductId).ToList();

                        foreach (var pc in productComponents)
                        {
                            var warehouseComponent = context.WarehouseComponents.Where(rec => rec.ComponentId == pc.ComponentId);
                            int sum = warehouseComponent.Sum(rec => rec.Count);
                            int neededCount = pc.Count;

                            foreach (var wc in warehouseComponent)
                            {
                                if (wc.Count >= neededCount)
                                {
                                    wc.Count -= neededCount;
                                    neededCount = 0;
                                    break;
                                }
                                else
                                {
                                    neededCount -= wc.Count;
                                    wc.Count = 0;
                                }
                            }

                            if (neededCount > 0)
                            {
                                throw new Exception("На складах не достаточно компонентов");
                            }

                        }

                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
