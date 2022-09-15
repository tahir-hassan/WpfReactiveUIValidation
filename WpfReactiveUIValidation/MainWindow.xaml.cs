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

namespace WpfReactiveUIValidation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        XMainWindowViewModel XViewModel = new XMainWindowViewModel();

        private CompositeDisposable XBind<TControlProp, XProp>(
            Expression<Func<MainWindow, TControlProp>> controlProp, 
            Expression<Func<XMainWindowViewModel, XProp>> modelProp)
        {
            var _1 = this.WhenAnyValue(controlProp).BindTo(XViewModel, modelProp!);
            var _2 = XViewModel.WhenAnyValue(modelProp).BindTo(this, controlProp!);
            return new CompositeDisposable(_1, _2);
        } 

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = new EmptyViewModel();

            this.WhenActivated(disposable =>
            {
                XViewModel.WhenAnyValue(x => x)
                    .BindTo(this, x => x.DataContext)
                    .DisposeWith(disposable);

                this.WhenAnyValue(x => x.XViewModel)
                    .BindTo(this, x => x.DataContext)
                    .DisposeWith(disposable);

                this.XBind(x => x.NameTextBox.Text, x => x.Name).DisposeWith(disposable);
                this.XBind(x => x.AddressTextBox.Text, x => x.Address).DisposeWith(disposable);

                XViewModel.ValidationContext.ValidationStatusChange
                    .Select(x => x.Text.ToSingleLine(Environment.NewLine))
                    .BindTo(this, view => view.MyErrors.Text)
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
