using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp_SMS.VM;

namespace WpfApp_SMS.Views
{
    /// <summary>
    /// Interaction logic for EditWin.xaml
    /// </summary>
    public partial class EditWin : Window
    {
        public EditWin(EditWinVM vm)
        {
            InitializeComponent();
            DataContext = vm;
            vm.CloseAction = () => Close();
        }
    }
}
