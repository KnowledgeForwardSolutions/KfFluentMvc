namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a binding that invokes a model method in response to a target 
///   object event.
/// </summary>
/// <remarks>
///   The typical example is a control Click event triggering a model method.
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
public class TargetEventBinding<M, T, E> : MvcBindingBase<M>
   where M : IMvcModel
   where E : EventArgs
{
   protected EventInfo _targetEventInfo;
   protected MethodInfo _modelMethodInfo;
   protected Delegate _handlerDelegate;

   /// <summary>
   ///   Initialize a new <see cref="TargetEventBinding{M, T, E}"/>.
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
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="target"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="targetEvent"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelMethod"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="targetEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   - or -
   ///   <paramref name="modelMethod"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="target"/> does not implement an event named 
   ///   <paramref name="targetEvent"/>.
   ///   - or -
   ///   <paramref name="model"/> does not implement a method named 
   ///   <paramref name="modelMethod"/>.
   ///   - or -
   ///   Method <paramref name="modelMethod"/> of the 
   ///   <paramref name="model"/> does not have return type void (for 
   ///   synchronous methods) or <see cref="Task"/> for asynchronous methods.
   /// </exception>
   public TargetEventBinding(
      M model,
      T target,
      String targetEvent,
      String modelMethod) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(target, nameof(target));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(targetEvent, nameof(targetEvent));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelMethod, nameof(modelMethod));

      _modelMethodInfo = model.GetMethodInfo(modelMethod);
      Target = target;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _targetEventInfo = Target.GetEventInfo(targetEvent);
      var handlerMethodInfo = _modelMethodInfo.ReturnType == typeof(void)
         ? this.GetMethodInfo(nameof(Target_Event))
         : _modelMethodInfo.ReturnType == typeof(Task)
            ? this.GetMethodInfo(nameof(Target_AsyncEvent))
            : throw new InvalidOperationException(Messages.InvalidBoundMethodSignatureMessage);
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
      => _modelMethodInfo.Invoke(Model, null);

   public async void Target_AsyncEvent(Object? sender, E e)
      => await (Task)_modelMethodInfo.Invoke(Model, null)!;
#pragma warning restore IDE0060 // Remove unused parameter

   protected override void ReleaseResources()
   {
      _targetEventInfo.RemoveEventHandler(Target, _handlerDelegate);

      _modelMethodInfo = default!;
      _targetEventInfo = default!;
      _handlerDelegate = default!;

      base.ReleaseResources();
   }
}
