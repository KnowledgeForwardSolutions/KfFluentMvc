namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a binding that invokes a model method in response to a control 
///   event.
/// </summary>
/// <remarks>
///   The typical example is a control Click event triggering a model method.
/// </remarks>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="E">
///   The event argument type.
/// </typeparam>
public class ControlEventBinding<M, E> : MvcBindingBase<M>
   where M : IMvcModel
   where E : EventArgs
{
   protected EventInfo _controlEventInfo;
   protected MethodInfo _modelMethodInfo;
   protected Delegate _handlerDelegate;

   /// <summary>
   ///   Initialize a new <see cref="ControlEventBinding{M, E}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="control">
   ///   The control to monitor for event notifications.
   /// </param>
   /// <param name="controlEvent">
   ///   The name of the control event to monitor.
   /// </param>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="control"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="controlEvent"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelMethod"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="controlEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   - or -
   ///   <paramref name="modelMethod"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="control"/> does not implement an event named 
   ///   <paramref name="controlEvent"/>.
   ///   - or -
   ///   <paramref name="model"/> does not implement a method named 
   ///   <paramref name="modelMethod"/>.
   ///   - or -
   ///   Method <paramref name="modelMethod"/> of the 
   ///   <paramref name="model"/> does not have return type void (for 
   ///   synchronous methods) or <see cref="Task"/> for asynchronous methods.
   /// </exception>
   public ControlEventBinding(
      M model,
      Control control,
      String controlEvent,
      String modelMethod) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(controlEvent, nameof(controlEvent));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelMethod, nameof(modelMethod));

      _modelMethodInfo = model.GetMethodInfo(modelMethod);
      Control = control;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _controlEventInfo = Control.GetEventInfo(controlEvent);
      var handlerMethodInfo = _modelMethodInfo.ReturnType == typeof(void)
         ? this.GetMethodInfo(nameof(Control_Event))
         : _modelMethodInfo.ReturnType == typeof(Task)
            ? this.GetMethodInfo(nameof(Control_AsyncEvent))
            : throw new InvalidOperationException(Messages.InvalidBoundMethodSignatureMessage);
      _handlerDelegate = Delegate.CreateDelegate(
         _controlEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _controlEventInfo.AddEventHandler(Control, _handlerDelegate);
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Control { get; private set; }

#pragma warning disable IDE0060 // Remove unused parameter
   public void Control_Event(Object? sender, E e)
      => _modelMethodInfo.Invoke(Model, null);

   public async void Control_AsyncEvent(Object? sender, E e)
      => await (Task)_modelMethodInfo.Invoke(Model, null)!;
#pragma warning restore IDE0060 // Remove unused parameter

   protected override void ReleaseResources()
   {
      _controlEventInfo.RemoveEventHandler(Control, _handlerDelegate);

      _modelMethodInfo = default!;
      _controlEventInfo = default!;
      _handlerDelegate = default!;

      base.ReleaseResources();
   }
}
