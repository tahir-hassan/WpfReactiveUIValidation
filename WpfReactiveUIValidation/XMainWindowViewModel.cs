using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;

namespace WpfReactiveUIValidation
{
    public class XMainWindowViewModel : ReactiveObject, IValidatableViewModel
    {
        public XMainWindowViewModel()
        {
            this.ValidationRule(
                vm => vm.Name,
                name => !string.IsNullOrWhiteSpace(name),
                "You must specify a valid name");

            this.ValidationRule(
                vm => vm.Address,
                address => !string.IsNullOrWhiteSpace(address),
                "You must specify a valid address");
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private string _address = "";
        public string Address
        {
            get => _address;
            set => this.RaiseAndSetIfChanged(ref _address, value);
        }

        public ValidationContext ValidationContext { get; } = new ValidationContext();
    }
}
