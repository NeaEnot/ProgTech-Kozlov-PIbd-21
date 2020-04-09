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

        public List<(DateTime, List<ReportOrdersViewModel>)> GetOrders(ReportBindingModel model)
        {
            List<(DateTime, List<ReportOrdersViewModel>)> list = new List<(DateTime, List<ReportOrdersViewModel>)>();

            var orders = orderLogic.Read(new OrderBindingModel
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
            });

            List<DateTime> dates = new List<DateTime>();
            foreach (var order in orders)
            {
                if (!dates.Contains(order.DateCreate.Date))
                {
                    dates.Add(order.DateCreate.Date);
                }
            }

            foreach(var date in dates)
            {
                (DateTime, List<ReportOrdersViewModel>) record;
                record.Item2 = new List<ReportOrdersViewModel>();

                record.Item1 = date;

                foreach(var order in orders.Where(rec => rec.DateCreate.Date == date))
                {
                    record.Item2.Add(order);
                }

                list.Add(record);
            }

            return list;
        }

        public void SaveProductsToWordFile(ReportBindingModel model)
        {
            SaveToWord.CreateDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список изделий",
                Products = productLogic.Read(null)
            });
        }

        public void SaveOrdersToExcelFile(ReportBindingModel model)
        {
            var a = GetOrders(model);

            SaveToExcel.CreateDoc(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                Orders = GetOrders(model)
            });
        }

        public void SaveProductComponentsToPdfFile(ReportBindingModel model)
        {
            SaveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список издлий с компонентами",
                ProductComponents = GetProductComponent()
            });
        }
    }
}
