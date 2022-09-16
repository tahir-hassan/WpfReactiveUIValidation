using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Collections;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using ReactiveUI.Validation.Formatters.Abstractions;
using ReactiveUI;
using System.Reactive.Disposables;
using ReactiveUI.Validation.Helpers;

namespace WpfReactiveUIValidation.ReactiveUIExtensions
{
    public static class ValidationExtensions
    {
        private static IValidationState SingleValidationState(IList<IValidationState> validationStates)
        {
            var valid = validationStates.All(v => v.IsValid);
            var message = ValidationText.Create(validationStates.Where(v => !v.IsValid).Select(v => v.Text));
            return new ValidationState(valid, message);
        }

        public static IObservable<IValidationState> ValidationStatusChangeFor<TViewModel, TViewModelProperty>(
            this TViewModel model,
            Expression<Func<TViewModel, TViewModelProperty>> viewModelProperty
            ) where TViewModel : IValidatableViewModel
        {
            return model.ValidationContext
                .ObserveFor(viewModelProperty)
                .Select(SingleValidationState);
        }

        public static IDisposable BindValidationEx<TView, TViewModel, TViewProperty>(this TView view,
            TViewModel viewModel,
            Expression<Func<TView, TViewProperty>> viewProperty,
            IValidationTextFormatter<string> formatter)
            where TView : class
            where TViewModel : class, IReactiveObject, IValidatableViewModel
        {
            return viewModel.ValidationContext.ValidationStatusChange
                .Select(x => formatter.Format(x.Text))
                .BindTo(view, viewProperty!);
        }

        public static IDisposable BindValidationEx<TView, TViewModel, TViewModelProperty, TViewProperty>(
            this TView view,
            TViewModel viewModel,
            Expression<Func<TViewModel, TViewModelProperty>> viewModelProperty,
            Expression<Func<TView, TViewProperty>> viewProperty,
            IValidationTextFormatter<string> formatter)
        where TView : class
        where TViewModel : class, IReactiveObject, IValidatableViewModel
        {
            return viewModel.ValidationStatusChangeFor(viewModelProperty)
                .Select(x => formatter.Format(x.Text))
                .BindTo(view, viewProperty!);
        }

        public static ValidationHelper ValidationRuleEx<TViewModel, TViewModelProperty>
            (this TViewModel viewModel,
            Expression<Func<TViewModel, TViewModelProperty>> viewModelProperty,
            Func<TViewModelProperty, (bool isValid, string message)> validationFunc
        )
        where TViewModel : class, IReactiveObject, IValidatableViewModel
        {
            return viewModel.ValidationRule(viewModelProperty,
                viewModel.WhenAnyValue(viewModelProperty)
                    .Select(validationFunc),
                    v => v.Item1,
                    v => v.Item2);
        }
    }

    public static class BindingExtensions 
    {
        public static IDisposable BindEx<TViewModel, TView, TVMProp, TVProp>(
            this TView view,
            TViewModel? viewModel,
            Expression<Func<TViewModel, TVMProp?>> vmProperty,
            Expression<Func<TView, TVProp>> viewProperty)
            where TViewModel : class
            where TView : class, IViewFor
        {
            var _1 = view.WhenAnyValue(viewProperty).BindTo(viewModel, vmProperty);
            var _2 = viewModel.WhenAnyValue(vmProperty).BindTo(view, viewProperty!);
            return new CompositeDisposable(_1, _2);
        }
    }
}
