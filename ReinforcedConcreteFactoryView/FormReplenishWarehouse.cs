using ReinforcedConcreteFactoryBusinessLogic.Interfaces;
using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using ReinforcedConcreteFactoryBusinessLogic.BindingModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;

namespace ReinforcedConcreteFactoryView
{
    public partial class FormReplenishWarehouse : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IComponentLogic componentLogic;
        private readonly IWarehouseLogic warehouseLogic;

        public FormReplenishWarehouse(IComponentLogic componenmtLogic, IWarehouseLogic warehouseLogic)
        {
            InitializeComponent();
            this.componentLogic = componenmtLogic;
            this.warehouseLogic = warehouseLogic;
        }

        private void FormReplenishWarehouse_Load(object sender, EventArgs e)
        {
            try
            {
                List<ComponentViewModel> list = componentLogic.GetList();
                if (list != null)
                {
                    comboBoxComponent.DisplayMember = "ComponentName";
                    comboBoxComponent.ValueMember = "Id";
                    comboBoxComponent.DataSource = list;
                    comboBoxComponent.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                List<WarehouseViewModel> list = warehouseLogic.GetList();
                if (list != null)
                {
                    comboBoxWarehouse.DisplayMember = "WarehouseName";
                    comboBoxWarehouse.ValueMember = "Id";
                    comboBoxWarehouse.DataSource = list;
                    comboBoxWarehouse.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxWarehouse.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                List<WarehouseComponentBindingModel> warehouseComponentBM = new List<WarehouseComponentBindingModel>();

                WarehouseViewModel warehouse = warehouseLogic.GetElement(Convert.ToInt32(comboBoxWarehouse.SelectedValue));

                for (int i = 0; i < warehouse.WarehouseComponents.Count; ++i)
                {
                    warehouseComponentBM.Add(new WarehouseComponentBindingModel
                    {
                        Id = warehouse.WarehouseComponents[i].Id,
                        WarehouseId = warehouse.WarehouseComponents[i].WarehouseId,
                        ComponentId = warehouse.WarehouseComponents[i].ComponentId,
                        Count = warehouse.WarehouseComponents[i].Count
                    });
                }

                warehouseComponentBM.Add(new WarehouseComponentBindingModel
                {
                    WarehouseId = Convert.ToInt32(comboBoxWarehouse.SelectedValue),
                    ComponentId = Convert.ToInt32(comboBoxComponent.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });

                warehouseLogic.UpdElement(new WarehouseBindingModel
                {
                    Id = Convert.ToInt32(comboBoxWarehouse.SelectedValue),
                    WarehouseName = warehouseLogic.GetElement(Convert.ToInt32(comboBoxWarehouse.SelectedValue)).WarehouseName,
                    WarehouseComponent = warehouseComponentBM
                });

                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
