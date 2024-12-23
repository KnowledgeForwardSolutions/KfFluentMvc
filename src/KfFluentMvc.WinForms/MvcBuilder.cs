// Ignore Spelling: Mvc

namespace KfFluentMvc.WinForms;

/// <summary>
///   Fluent builder for creating MVC bindings.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
public class MvcBuilder<M>
   where M : IMvcModel
{
   private MvcBindingCollection<M> _bindingCollection = new();
   private Control _currentControl = default!;
   //private Component _currentComponent = default!;

   /// <summary>
   ///   Initialize a new <see cref="MvcBuilder{M}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to bind to <see cref="Component"/>s or 
   ///   <see cref="Control"/>s.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   /// </exception>
   public MvcBuilder(M model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));

      Model = model;
   }

   /// <summary>
   ///   The bound model.
   /// </summary>
   public M Model { get; private init; }

   /// <summary>
   ///   Create a binding that invokes a model method in response to a control's 
   ///   Click event.
   /// </summary>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindFromControlClickEvent(String modelMethod)
      => BindFromControlEvent(nameof(Control.Click), modelMethod);

   /// <summary>
   ///   Create a binding that invokes a model method in response to a control 
   ///   event.
   /// </summary>
   /// <param name="controlEvent">
   ///   The name of the control event to monitor.
   /// </param>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindFromControlEvent(
      String controlEvent,
      String modelMethod)
   {
      ThrowIfControlNotSet();

      var binding = new FromControlEventBinding<M>(Model, _currentControl, controlEvent, modelMethod);
      _bindingCollection.AddBinding(binding);

      return this;
   }

   /// <summary>
   ///   Create a one-way binding from a control's Enabled property to a model 
   ///   property.
   /// </summary>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the control property and possibly converts
   ///   the control property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the control property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindFromControlEnabledProperty(
      String modelProperty,
      Func<Control, Boolean>? propertyGetter)
      => BindFromControlProperty<Boolean>(nameof(Control.Enabled), modelProperty, propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a control property to a model property.
   /// </summary>
   /// <typeparam name="P">
   ///   The type of the model's bound property.
   /// </typeparam>
   /// <param name="controlProperty">
   ///   The name of the control property to monitor for changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="controlPropertyChangedEvent">
   ///   Optional. The name of the control property changed event. Defaults to
   ///   <paramref name="controlProperty"/> + "Changed".
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the control property and possibly converts
   ///   the control property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the control property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   Attempt to invoke this method without first invoking the 
   ///   <see cref="WithControl(Control)"/> method.
   /// </exception>
   public MvcBuilder<M> BindFromControlProperty<P>(
      String controlProperty,
      String modelProperty,
      String? controlPropertyChangedEvent = null,
      Func<Control, P>? propertyGetter = null)
   {
      ThrowIfControlNotSet();

      var binding = new FromControlPropertyBinding<M, P>(
         Model,
         _currentControl,
         controlProperty,
         modelProperty,
         controlPropertyChangedEvent,
         propertyGetter);
      _bindingCollection.AddBinding(binding);

      return this;
   }

   /// <summary>
   ///   Create a one-way binding from a control's Text property to a model 
   ///   property.
   /// </summary>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the control property and possibly converts
   ///   the control property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the control property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindFromControlTextProperty(
      String modelProperty,
      Func<Control, String>? propertyGetter = null)
      => BindFromControlProperty<String>(nameof(Control.Text), modelProperty, propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a control's Visible property to a model 
   ///   property.
   /// </summary>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the control property and possibly converts
   ///   the control property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the control property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindFromControlVisibleProperty(
      String modelProperty,
      Func<Control, Boolean>? propertyGetter)
      => BindFromControlProperty<Boolean>(nameof(Control.Visible), modelProperty, propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control's Enabled
   ///   property.
   /// </summary>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the control
   ///   property. Defaults to a function that simply gets the model property
   ///   value.
   /// </param>
   public MvcBuilder<M> BindToControlEnabledProperty(
      String modelProperty,
      Func<M, Boolean>? propertyGetter = null)
      => BindToControlProperty<Boolean>(modelProperty, nameof(Control.Enabled), propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control property. 
   /// </summary>
   /// <typeparam name="P">
   ///   The type of the control's bound property.
   /// </typeparam>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="controlProperty">
   ///   The control property to set when the model property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the control
   ///   property. Defaults to a function that simply gets the model property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   Attempt to invoke this method without first invoking the 
   ///   <see cref="WithControl(Control)"/> method.
   /// </exception>
   public MvcBuilder<M> BindToControlProperty<P>(
      String modelProperty,
      String controlProperty,
      Func<M, P>? propertyGetter = null)
   {
      ThrowIfControlNotSet();

      var binding = new ToControlPropertyBinding<M, P>(
         Model, 
         _currentControl, 
         modelProperty, 
         controlProperty, 
         propertyGetter);
      _bindingCollection.AddBinding(binding);

      return this;
   }

   /// <summary>
   ///   Create a one-way binding from a model property to a control's Text
   ///   property.
   /// </summary>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the control
   ///   property. Defaults to a function that simply gets the model property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindToControlTextProperty(
      String modelProperty, 
      Func<M, String>? propertyGetter = null)
      => BindToControlProperty<String>(modelProperty, nameof(Control.Text), propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control's Visible
   ///   property.
   /// </summary>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the control
   ///   property. Defaults to a function that simply gets the model property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindToControlVisibleProperty(
      String modelProperty,
      Func<M, Boolean>? propertyGetter = null)
      => BindToControlProperty<Boolean>(modelProperty, nameof(Control.Visible), propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control's  
   ///   <see cref="ToolTip"/>. The ToolTip text is set to the bound property's
   ///   error message(s) or to <see cref="String.Empty"/> if the bound property
   ///   does not have errors.
   /// </summary>
   /// <remarks>
   ///   The model must implement <see cref="IValidatingMvcModel"/>.
   /// </remarks>
   /// <param name="toolTip">
   ///   The bound <see cref="ToolTip"/> component.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindToControlToolTipOnModelError(
      ToolTip toolTip,
      String modelProperty)
   {
      var binding = new ToControlToolTipOnModelErrorBinding<M>(
         Model,
         _currentControl,
         toolTip,
         modelProperty);
      _bindingCollection.AddBinding(binding);

      return this;
   }

   /// <summary>
   ///   Create a one-way binding from a model property to a control's Visible
   ///   property that makes the control visible when the model property has an
   ///   error.
   /// </summary>
   /// <remarks>
   ///   <para>
   ///      The model must implement <see cref="IValidatingMvcModel"/>.
   ///   </para>
   ///   <para>
   ///      Typically used to display error indicators next to a control that
   ///      is bound to a model property that has an error.
   ///   </para>
   /// </remarks>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindToControlVisibleOnModelError(String modelProperty)
   {
      var binding = new ToControlVisibleOnModelErrorBinding<M>(
         Model,
         _currentControl,
         modelProperty);
      _bindingCollection.AddBinding(binding);

      return this;
   }

   /// <summary>
   ///   Complete the build process.
   /// </summary>
   /// <returns>
   ///   A collection of the completed bindings.
   /// </returns>
   public MvcBindingCollection<M> Build()
   {
      var result = _bindingCollection;
      _bindingCollection = default!;

      return result;
   }

   /// <summary>
   ///   Create a new <see cref="MvcBuilder{M}"/> to bind a model to 
   ///   <see cref="Component"/>s or <see cref="Control"/>s.
   /// </summary>
   /// <param name="model">
   ///   The model to bind.
   /// </param>
   /// <returns>
   ///   A new builder object.
   /// </returns>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   /// </exception>
   public static MvcBuilder<M> CreateBuilder(M model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));

      return new(model);
   }

   /// <summary>
   ///   Set the <see cref="Control"/> that future bindings will bind to.
   /// </summary>
   /// <param name="control">
   ///   The next <see cref="Control"/> to bind the model to.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> WithControl(Control control)
   {
      ArgumentNullException.ThrowIfNull(control, nameof(control));

      _currentControl = control;

      return this;
   }

   private void ThrowIfControlNotSet()
   {
      if (_currentControl is null)
      {
         throw new InvalidOperationException(Messages.ControlNotSet);
      }
   }
}
