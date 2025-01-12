namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one way binding from a control property to a model property.
///   The model property is updated whenever the control broadcasts a 
///   notification that its property has changed.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="E">
///   The event argument type.
/// </typeparam>
/// <typeparam name="P">
///   The type of the model's bound property.
/// </typeparam>
public class ControlPropertyBinding<M, E, P> : MvcBindingBase<M>
   where M : IMvcModel
   where E : EventArgs
{
   protected PropertyInfo _modelPropertyInfo;
   protected PropertyInfo _controlPropertyInfo;
   protected Func<Control, P> _propertyGetter;
   protected EventInfo _controlPropertyChangedEventInfo;
   protected Delegate _handlerDelegate;

   /// <summary>
   ///   Initialize a new <see cref="ControlPropertyBinding{M, E, P}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to update when the control property changes.
   /// </param>
   /// <param name="control">
   ///   The <see cref="Control"/> to monitor for changes.
   /// </param>
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
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="control"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="controlProperty"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="controlProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="control"/> does not implement a property named 
   ///   <paramref name="controlProperty"/>.
   ///   - or -
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   /// </exception>
   public ControlPropertyBinding(
      M model,
      Control control,
      String controlProperty,
      String modelProperty,
      String? controlPropertyChangedEvent = null,
      Func<Control, P>? propertyGetter = null) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(controlProperty, nameof(controlProperty));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));

      Control = control;
      _controlPropertyInfo = Control.GetPropertyInfo(controlProperty);
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      controlPropertyChangedEvent ??= controlProperty + "Changed";
      _controlPropertyChangedEventInfo = Control.GetEventInfo(controlPropertyChangedEvent);
      var handlerMethodInfo = this.GetMethodInfo(nameof(Control_PropertyChanged));
      _handlerDelegate = Delegate.CreateDelegate(
         _controlPropertyChangedEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _controlPropertyChangedEventInfo.AddEventHandler(Control, _handlerDelegate);

      _propertyGetter = propertyGetter ?? GetPropertyValue;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Control { get; private set; }

#pragma warning disable IDE0060 // Remove unused parameter
   public void Control_PropertyChanged(Object? sender, E e)
   {
      var value = _propertyGetter(Control);
      _modelPropertyInfo.SetValue(Model, value);
   }
#pragma warning restore IDE0060 // Remove unused parameter

   protected override void ReleaseResources()
   {
      _controlPropertyChangedEventInfo.RemoveEventHandler(Control, _handlerDelegate);

      _modelPropertyInfo = default!;
      _controlPropertyInfo = default!;
      _controlPropertyChangedEventInfo = default!;
      _handlerDelegate = default!;
      Control = default!;

      base.ReleaseResources();
   }

   private P GetPropertyValue(Control control) => (P)_controlPropertyInfo.GetValue(control)!;
}
