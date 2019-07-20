using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace SpeedConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Calculation Calc = new Calculation();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Calc;
            Calc.Clear();
        }
        private void BtnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (TxtSpeed.Text != "" && TxtTime.Text !="")
                Calc.CalculateDistance();
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            Calc.Clear();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Calc.SaveFile();
        }
        private void TxtSpeed_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^(\d)* (\.\d +)?$");
            e.Handled = !(regex.IsMatch(e.Text));
        }
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
