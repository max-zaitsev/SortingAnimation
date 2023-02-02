using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using SortingAnimation.ViewModel.ViewModels;
using System.Windows.Media.Media3D;
using System.Linq;

namespace SortingAnimation.View.Views
{
    /// <summary>
    /// Класс View главного окна
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                if (e.Action == ValidationErrorEventAction.Added)
                {
                    viewModel.ErrorsCount++;
                }
                else
                {
                    viewModel.ErrorsCount--;
                }
            }
        }
    }
}