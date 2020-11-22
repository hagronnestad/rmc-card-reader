using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RmcCardReaderGui {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormRmcCardReader.azureKey = args[0];
            FormRmcCardReader.azureUrl = args[1];
            Application.Run(new FormRmcCardReader());
        }
    }
}
