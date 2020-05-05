using System;
using System.Windows.Forms;

namespace ReinforcedConcreteFactoryWarehouseView
{
    static class Program
    {
        public static bool IsLogined { get; set; }

        [STAThread]
        static void Main()
        {
            APIWarehouse.Connect();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new FormEnter();
            form.ShowDialog();

            if (IsLogined)
            {
                Application.Run(new FormMain());
            }
        }
    }
}
