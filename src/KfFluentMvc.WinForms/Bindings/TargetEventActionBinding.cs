namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a binding that invokes a supplied <see cref="Action{M,Control}"/> 
///   in response to a control event.
/// </summary>
/// <remarks>
///   The typical example is a control Click event that sets a model property.
/// </remarks>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="T">
///   The target object type.
/// </typeparam>
/// <typeparam name="E">
///   The event argument type.
/// </typeparam>
public class TargetEventActionBinding<M, T, E> : MvcBindingBase<M>
   where M : IMvcModel
   where E : EventArgs
{
   protected EventInfo _targetEventInfo;
   protected Delegate _handlerDelegate;
   protected Action<M, T> _action = default!;
   protected Action<M, T, E> _eventArgsAction = default!;

   /// <summary>
   ///   Initialize a new <see cref="TargetEventActionBinding{M, T, E}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="target">
   ///   The target object to monitor for event notifications.
   /// </param>
   /// <param name="targetEvent">
   ///   The name of the target object event to monitor.
   /// </param>
   /// <param name="action">
   ///   Action to perform when target object event is fired.
   /// </param>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <remarks>
   ///   Use this overload when the event parameters are not required by the
   ///   bound action.
   /// </remarks>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="target"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="targetEvent"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="action"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="targetEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="target"/> does not implement an event named 
   ///   <paramref name="targetEvent"/>.
   /// </exception>
   public TargetEventActionBinding(
      M model,
      T target,
      String targetEvent,
      Action<M, T> action) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(target, nameof(target));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(targetEvent, nameof(targetEvent));
      ArgumentNullException.ThrowIfNull(action, nameof(action));

      Target = target;
      _action = action;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _targetEventInfo = Target.GetEventInfo(targetEvent);
      var handlerMethodInfo = this.GetMethodInfo(nameof(Target_Event));
      _handlerDelegate = Delegate.CreateDelegate(
         _targetEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _targetEventInfo.AddEventHandler(Target, _handlerDelegate);
   }

   /// <summary>
   ///   Initialize a new <see cref="TargetEventActionBinding{M, T, E}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="target">
   ///   The target object to monitor for event notifications.
   /// </param>
   /// <param name="targetEvent">
   ///   The name of the target object event to monitor.
   /// </param>
   /// <param name="action">
   ///   Action to perform when target object event is fired.
   /// </param>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <remarks>
   ///   Use this overload when the event parameters are required by the bound
   ///   action.
   /// </remarks>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="target"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="targetEvent"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="action"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="targetEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="target"/> does not implement an event named 
   ///   <paramref name="targetEvent"/>.
   /// </exception>
   public TargetEventActionBinding(
      M model,
      T target,
      String targetEvent,
      Action<M, T, E> action) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(target, nameof(target));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(targetEvent, nameof(targetEvent));
      ArgumentNullException.ThrowIfNull(action, nameof(action));

      Target = target;
      _eventArgsAction = action;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _targetEventInfo = Target.GetEventInfo(targetEvent);
      var handlerMethodInfo = this.GetMethodInfo(nameof(Target_EventWithEventArgs));
      _handlerDelegate = Delegate.CreateDelegate(
         _targetEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _targetEventInfo.AddEventHandler(Target, _handlerDelegate);
   }

   /// <summary>
   ///   The bound target object.
   /// </summary>
   public T Target { get; private set; }

#pragma warning disable IDE0060 // Remove unused parameter
   public void Target_Event(Object? sender, E e)
      => _action(Model, Target);

   public void Target_EventWithEventArgs(Object? sender, E e)
      => _eventArgsAction(Model, Target, e);
#pragma warning restore IDE0060 // Remove unused parameter

   protected override void ReleaseResources()
   {
      _targetEventInfo.RemoveEventHandler(Target, _handlerDelegate);

      _targetEventInfo = default!;
      _handlerDelegate = default!;
      _action = default!;
      _eventArgsAction = default!;
      Target = default!;

      base.ReleaseResources();
   }
}
