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
        private readonly IComponentLogic componentLogic;
        private readonly IProductLogic productLogic;
        private readonly IOrderLogic orderLogic;

        public ReportLogic(IProductLogic productLogic, IComponentLogic componentLogic, IOrderLogic orderLogic)
        {
            this.productLogic = productLogic;
            this.componentLogic = componentLogic;
            this.orderLogic = orderLogic;
        }

        public List<ReportProductComponentViewModel> GetProductComponent()
        {
            var components = componentLogic.Read(null);
            var products = productLogic.Read(null);
            var list = new List<ReportProductComponentViewModel>();

            foreach (var component in components)
            {
                var record = new ReportProductComponentViewModel
                {
                    ComponentName = component.ComponentName,
                    Products = new List<Tuple<string, int>>(),
                    TotalCount = 0
                };

                foreach (var product in products)
                {
                    if (product.ProductComponents.ContainsKey(component.Id))
                    {
                        record.Products.Add(new Tuple<string, int>(product.ProductName,
                        product.ProductComponents[component.Id].Item2));
                        record.TotalCount += product.ProductComponents[component.Id].Item2;
                    }
                }
                list.Add(record);
            }
            return list;
        }

        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return orderLogic.Read(new OrderBindingModel
            {
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            })
            .Select(x => new ReportOrdersViewModel
            {
                DateCreate = x.DateCreate,
                ProductName = x.ProductName,
                Count = x.Count,
                Sum = x.Sum,
                Status = x.Status
            })
            .ToList();
        }

        public void SaveComponentsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список компонент",
                Components = componentLogic.Read(null)
            });
        }

        public void SaveProductComponentToExcelFile(ReportBindingModel model)
        {
            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список компонент",
                ProductComponents = GetProductComponent()
            });
        }

        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom.Value,
                DateTo = model.DateTo.Value,
                Orders = GetOrders(model)
            });
        }
    }
}
