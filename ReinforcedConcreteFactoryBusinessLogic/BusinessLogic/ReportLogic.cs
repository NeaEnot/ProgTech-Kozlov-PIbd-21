using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.HelperModels;
using ReinforcedConcreteFactoryBusinessLogic.Interfaces;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReinforcedConcreteFactoryBusinessLogic.BusinessLogic
{
    public class ReportLogic
    {
        private readonly IProductLogic productLogic;
        private readonly IOrderLogic orderLogic;
        private readonly IWarehouseLogic warehouseLogic;

        public ReportLogic(IProductLogic productLogic, IOrderLogic orderLogic, IWarehouseLogic warehouseLogic)
        {
            this.productLogic = productLogic;
            this.orderLogic = orderLogic;
            this.warehouseLogic = warehouseLogic;
        }

        public List<ReportProductComponentViewModel> GetProductComponents()
        {
            var products = productLogic.Read(null);
            var list = new List<ReportProductComponentViewModel>();

            foreach (var product in products)
            {
                foreach (var pc in product.ProductComponents)
                {
                    var record = new ReportProductComponentViewModel
                    {
                        ProductName = product.ProductName,
                        ComponentName = pc.Value.Item1,
                        Count = pc.Value.Item2
                    };

                    list.Add(record);
                }
            }
            return list;
        }

        public List<ReportWarehouseComponentViewModel> GetWarehouseComponents()
        {
            var wahehouses = warehouseLogic.Read(null);
            var list = new List<ReportWarehouseComponentViewModel>();

            foreach (var wahehouse in wahehouses)
            {
                foreach (var wc in wahehouse.WarehouseComponents)
                {
                    var record = new ReportWarehouseComponentViewModel
                    {
                        WarehouseName = wahehouse.WarehouseName,
                        ComponentName = wc.Value.Item1,
                        Count = wc.Value.Item2
                    };

                    list.Add(record);
                }
            }
            return list;
        }

        public List<IGrouping<DateTime, OrderViewModel>> GetOrders(ReportBindingModel model)
        {
            var list = orderLogic
            .Read(new OrderBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            })
            .GroupBy(rec => rec.DateCreate.Date)
            .OrderBy(recG => recG.Key)
            .ToList();

            return list;
        }

        public void SaveProductsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список изделий",
                Products = productLogic.Read(null),
                Warehouses = null
            });
        }

        public void SaveOrdersToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                Orders = GetOrders(model),
                Warehouses = null
            });
        }

        public void SaveProductComponentsToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список издлий с компонентами",
                ProductComponents = GetProductComponents(),
                WarehouseComponents = null
            });
        }

        public void SaveWarehousesToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список складов",
                Products = null,
                Warehouses = warehouseLogic.Read(null)
            });
        }

        public void SaveWarehouseComponentsToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список компонентов в складах",
                Orders = null,
                Warehouses = warehouseLogic.Read(null)
            });
        }

        public void SaveComponentsToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Компонентов",
                ProductComponents = null,
                WarehouseComponents = GetWarehouseComponents()
            });
        }
    }
}
