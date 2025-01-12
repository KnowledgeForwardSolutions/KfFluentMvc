namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a binding that invokes a supplied <see cref="Action{M,Control}"/> 
///   in response to a control event.
/// </summary>
/// <remarks>
///   <para>
///      The typical example is a control Click event that sets a model property.
///   </para>
///   <para>
///   The control event handler must be an <see cref="EventArgs"/> handler.
///   </para>
/// </remarks>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="E">
///   The event argument type.
/// </typeparam>
public class ControlEventActionBinding<M, E> : MvcBindingBase<M>
   where M : IMvcModel
   where E : EventArgs
{
   protected EventInfo _controlEventInfo;
   protected Delegate _handlerDelegate;
   protected Action<M, Control> _action;

   /// <summary>
   ///   Initialize a new <see cref="ControlEventActionBinding{M}"/>.
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
   /// <param name="action">
   ///   Action to perform when control event is fired.
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
   ///   <paramref name="action"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="controlEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="control"/> does not implement an event named 
   ///   <paramref name="controlEvent"/>.
   /// </exception>
   public ControlEventActionBinding(
      M model,
      Control control,
      String controlEvent,
      Action<M, Control> action) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(controlEvent, nameof(controlEvent));
      ArgumentNullException.ThrowIfNull(action, nameof(action));

      Control = control;
      _action = action;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _controlEventInfo = Control.GetEventInfo(controlEvent);
      var handlerMethodInfo = this.GetMethodInfo(nameof(Control_Event));
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
      => _action(Model, Control);
#pragma warning restore IDE0060 // Remove unused parameter

   protected override void ReleaseResources()
   {
      _controlEventInfo.RemoveEventHandler(Control, _handlerDelegate);

      _controlEventInfo = default!;
      _handlerDelegate = default!;
      _action = default!;
      Control = default!;

      base.ReleaseResources();
   }
}
