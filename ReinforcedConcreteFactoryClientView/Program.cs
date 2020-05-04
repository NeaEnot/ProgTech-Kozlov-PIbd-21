using ReinforcedConcreteFactoryBusinessLogic.ViewModels;
using System;
using System.Windows.Forms;

namespace ReinforcedConcreteFactoryClientView
{
    static class Program
    {
        public static ClientViewModel Client { get; set; }

        [STAThread]
        static void Main()
        {
            APIClient.Connect();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var form = new FormEnter();
            form.ShowDialog();

            if (Client != null)
            {
                Application.Run(new FormMain());
            }
        }
    }
}
