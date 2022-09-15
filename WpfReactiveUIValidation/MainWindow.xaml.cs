using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Formatters;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reactive.Linq;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.States;
using ReactiveUI.Validation.Collections;
using WpfReactiveUIValidation.ReactiveUIExtensions;

namespace WpfReactiveUIValidation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        XMainWindowViewModel XViewModel; 

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new EmptyViewModel();
            XViewModel = new XMainWindowViewModel();

            this.WhenActivated(disposable =>
            {
                this.BindEx(XViewModel, vm => vm.Name, view => view.NameTextBox.Text)
                    .DisposeWith(disposable);

                this.BindEx(XViewModel, vm => vm.Address, view => view.AddressTextBox.Text)
                    .DisposeWith(disposable);

                var formatter = new SingleLineFormatter(Environment.NewLine);
                
                this.BindValidationEx(XViewModel, view => view.AllErrors.Text, formatter)
                    .DisposeWith(disposable);

                this.BindValidationEx(XViewModel, vm => vm.Name, 
                    view => view.NameErrors.Text, formatter)
                    .DisposeWith(disposable);

                this.BindValidationEx(XViewModel, vm => vm.Address,
                    view => view.AddressErrors.Text, formatter)
                    .DisposeWith(disposable);

                XViewModel.IsValid()
                    .BindTo(this, view => view.SaveButton.IsEnabled)
                    .DisposeWith(disposable);
            });
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                $"{nameof(XViewModel.Name)}: {XViewModel.Name}{Environment.NewLine}{nameof(XViewModel.Address)}: {XViewModel.Address}", 
                $"From {nameof(XViewModel)}");
        }
    }
}
