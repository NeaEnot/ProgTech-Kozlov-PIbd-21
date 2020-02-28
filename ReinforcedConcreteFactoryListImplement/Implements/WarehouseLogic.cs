﻿using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
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

        public List<WarehouseViewModel> GetList()
        {
            List<WarehouseViewModel> result = new List<WarehouseViewModel>();

            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                List<WarehouseComponentViewModel> warehouseComponents = new List<WarehouseComponentViewModel>();

                for (int j = 0; j < source.WarehouseComponents.Count; ++j)
                {
                    if (source.WarehouseComponents[j].WarehouseId == source.Warehouses[i].Id)
                    {
                        string componentName = string.Empty;

                        for (int k = 0; k < source.Components.Count; ++k)
                        {
                            if (source.WarehouseComponents[j].ComponentId == source.Components[k].Id)
                            {
                                componentName = source.Components[k].ComponentName;
                                break;
                            }
                        }

                        warehouseComponents.Add(new WarehouseComponentViewModel
                        {
                            Id = source.WarehouseComponents[j].Id,
                            WarehouseId = source.WarehouseComponents[j].WarehouseId,
                            ComponentId = source.WarehouseComponents[j].ComponentId,
                            ComponentName = componentName,
                            Count = source.WarehouseComponents[j].Count
                        });
                    }
                }

                result.Add(new WarehouseViewModel
                {
                    Id = source.Warehouses[i].Id,
                    WarehouseName = source.Warehouses[i].WarehouseName,
                    WarehouseComponents = warehouseComponents
                });
            }

            return result;
        }

        public WarehouseViewModel GetElement(int id)
        {
            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                List<WarehouseComponentViewModel> warehouseComponents = new List<WarehouseComponentViewModel>();

                for (int j = 0; j < source.WarehouseComponents.Count; ++j)
                {
                    if (source.WarehouseComponents[j].WarehouseId == source.Warehouses[i].Id)
                    {
                        string componentName = string.Empty;

                        for (int k = 0; k < source.Components.Count; ++k)
                        {
                            if (source.WarehouseComponents[j].ComponentId == source.Components[k].Id)
                            {
                                componentName = source.Components[k].ComponentName;
                                break;
                            }
                        }

                        warehouseComponents.Add(new WarehouseComponentViewModel
                        {
                            Id = source.WarehouseComponents[j].Id,
                            WarehouseId = source.WarehouseComponents[j].WarehouseId,
                            ComponentId = source.WarehouseComponents[j].ComponentId,
                            ComponentName = componentName,
                            Count = source.WarehouseComponents[j].Count
                        });
                    }
                }

                if (source.Warehouses[i].Id == id)
                {
                    return new WarehouseViewModel
                    {
                        Id = source.Warehouses[i].Id,
                        WarehouseName = source.Warehouses[i].WarehouseName,
                        WarehouseComponents = warehouseComponents
                    };
                }
            }

            throw new Exception("Элемент не найден");
        }

        public void AddElement(WarehouseBindingModel model)
        {
            int maxId = 0;

            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id > maxId)
                {
                    maxId = source.Warehouses[i].Id;
                }

                if (source.Warehouses[i].WarehouseName == model.WarehouseName)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }

            source.Warehouses.Add(new Warehouse
            {
                Id = maxId + 1,
                WarehouseName = model.WarehouseName
            });
        }

        public void UpdElement(WarehouseBindingModel model)
        {
            int index = -1;

            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == model.Id)
                {
                    index = i;
                }

                if (source.Warehouses[i].WarehouseName == model.WarehouseName && source.Warehouses[i].Id != model.Id)
                {
                    throw new Exception("Уже есть склад с таким названием");
                }
            }

            if (index == -1)
            {
                throw new Exception("Элемент не найден");
            }

            source.Warehouses[index].WarehouseName = model.WarehouseName;

            int maxPCId = 0;

            for (int i = 0; i < source.WarehouseComponents.Count; ++i)
            {
                if (source.WarehouseComponents[i].Id > maxPCId)
                {
                    maxPCId = source.WarehouseComponents[i].Id;
                }
            }

            for (int i = 0; i < source.WarehouseComponents.Count; ++i)
            {
                if (source.WarehouseComponents[i].WarehouseId == model.Id)
                {
                    bool flag = true;

                    for (int j = 0; j < model.WarehouseComponent.Count; ++j)
                    {

                        if (source.WarehouseComponents[i].Id == model.WarehouseComponent[j].Id)
                        {
                            source.WarehouseComponents[i].Count =
                            model.WarehouseComponent[j].Count;
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        source.WarehouseComponents.RemoveAt(i--);
                    }
                }
            }

            for (int i = 0; i < model.WarehouseComponent.Count; ++i)
            {
                if (model.WarehouseComponent[i].Id == 0)
                {
                    for (int j = 0; j < source.WarehouseComponents.Count; ++j)
                    {
                        if (source.WarehouseComponents[j].WarehouseId == model.Id &&
                            source.WarehouseComponents[j].ComponentId == model.WarehouseComponent[i].ComponentId)
                        {
                            source.WarehouseComponents[j].Count += model.WarehouseComponent[i].Count;
                            model.WarehouseComponent[i].Id = source.WarehouseComponents[j].Id;
                            break;
                        }
                    }

                    if (model.WarehouseComponent[i].Id == 0)
                    {
                        source.WarehouseComponents.Add(new WarehouseComponent
                        {
                            Id = ++maxPCId,
                            WarehouseId = model.Id,
                            ComponentId = model.WarehouseComponent[i].ComponentId,
                            Count = model.WarehouseComponent[i].Count
                        });
                    }
                }
            }
        }

        public void DelElement(int id)
        {
            for (int i = 0; i < source.WarehouseComponents.Count; ++i)
            {
                if (source.WarehouseComponents[i].WarehouseId == id)
                {
                    source.WarehouseComponents.RemoveAt(i--);
                }
            }

            for (int i = 0; i < source.Warehouses.Count; ++i)
            {
                if (source.Warehouses[i].Id == id)
                {
                    source.Warehouses.RemoveAt(i);
                    return;
                }
            }

            throw new Exception("Элемент не найден");
        }
    }
}
