using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using ReinforcedConcreteFactoryBusinessLogic.BusinessLogic;
using ReinforcedConcreteFactoryBusinessLogic.Interfaces;
using System;
using System.Windows.Forms;
using Unity;

namespace ReinforcedConcreteFactoryView
{
    public partial class FormReportWarehouseComponents : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly ReportLogic logic;
        private readonly IWarehouseLogic warehouseLogic;

        public FormReportWarehouseComponents(ReportLogic logic, IWarehouseLogic warehouseLogic)
        {
            InitializeComponent();
            this.logic = logic;
            this.warehouseLogic = warehouseLogic;
        }

        private void FormReportWarehouseComponents_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var dict = warehouseLogic.Read(null);

                if (dict != null)
                {
                    dataGridView.Rows.Clear();

                    foreach (var warehouse in dict)
                    {
                        int componentsSum = 0;

                        dataGridView.Rows.Add(new object[] { warehouse.WarehouseName, "", "" });

                        foreach (var component in warehouse.WarehouseComponents.Values)
                        {
                            dataGridView.Rows.Add(new object[] { "", component.Item1, component.Item2 });
                            componentsSum += component.Item2;
                        }

                        dataGridView.Rows.Add(new object[] { "Итого", "", componentsSum });
                        dataGridView.Rows.Add(new object[] { });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonSaveToExcel_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog { Filter = "xlsx|*.xlsx" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        logic.SaveWarehouseComponentsToExcelFile(new ReportBindingModel { FileName = dialog.FileName });

                        MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
