using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Contexts;
using ReactiveUI.Validation.Extensions;
using WpfReactiveUIValidation.ReactiveUIExtensions;

namespace WpfReactiveUIValidation
{
    public class XMainWindowViewModel : ReactiveObject, IValidatableViewModel
    {
        public XMainWindowViewModel()
        {
            this.ValidationRule(
                vm => vm.Name,
                name => !string.IsNullOrWhiteSpace(name),
                "A name is required");

            this.ValidationRuleEx(vm => vm.Address,
                address =>
                {
                    address = address?.Trim();
                    if (string.IsNullOrEmpty(address))
                        return (false, "An address is required");
                    else if (!System.Text.RegularExpressions.Regex.IsMatch(address, @"^\d+"))
                        return (false, "The address must start with a number");
                    else return (true, "");
                });
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
