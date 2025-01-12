namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a binding that invokes a model method in response to a component 
///   event.
/// </summary>
/// <remarks>
///   The typical example is a component Click event triggering a model method.
/// </remarks>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="E">
///   The event argument type.
/// </typeparam>
class ComponentEventBinding<M, E> : MvcBindingBase<M>
   where M : IMvcModel
   where E : EventArgs
{
   protected EventInfo _componentEventInfo;
   protected MethodInfo _modelMethodInfo;
   protected Delegate _handlerDelegate;

   /// <summary>
   ///   Initialize a new <see cref="ComponentEventBinding{M, E}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="component">
   ///   The component to monitor for event notifications.
   /// </param>
   /// <param name="componentEvent">
   ///   The name of the component event to monitor.
   /// </param>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="component"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="componentEvent"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelMethod"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="componentEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   - or -
   ///   <paramref name="modelMethod"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="component"/> does not implement an event named 
   ///   <paramref name="componentEvent"/>.
   ///   - or -
   ///   <paramref name="model"/> does not implement a method named 
   ///   <paramref name="modelMethod"/>.
   ///   - or -
   ///   Method <paramref name="modelMethod"/> of the 
   ///   <paramref name="model"/> does not have return type void (for 
   ///   synchronous methods) or <see cref="Task"/> for asynchronous methods.
   /// </exception>
   public ComponentEventBinding(
      M model,
      Component component,
      String componentEvent,
      String modelMethod) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(component, nameof(component));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(componentEvent, nameof(componentEvent));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelMethod, nameof(modelMethod));

      _modelMethodInfo = model.GetMethodInfo(modelMethod);
      Component = component;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _componentEventInfo = Component.GetEventInfo(componentEvent);
      var handlerMethodInfo = _modelMethodInfo.ReturnType == typeof(void)
         ? this.GetMethodInfo(nameof(Component_Event))
         : _modelMethodInfo.ReturnType == typeof(Task)
            ? this.GetMethodInfo(nameof(Component_AsyncEvent))
            : throw new InvalidOperationException(Messages.InvalidBoundMethodSignatureMessage);
      _handlerDelegate = Delegate.CreateDelegate(
         _componentEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _componentEventInfo.AddEventHandler(Component, _handlerDelegate);
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Component Component { get; private set; }

#pragma warning disable IDE0060 // Remove unused parameter
   public void Component_Event(Object? sender, E e)
      => _modelMethodInfo.Invoke(Model, null);

   public async void Component_AsyncEvent(Object? sender, E e)
      => await (Task)_modelMethodInfo.Invoke(Model, null)!;
#pragma warning restore IDE0060 // Remove unused parameter

   protected override void ReleaseResources()
   {
      _componentEventInfo.RemoveEventHandler(Component, _handlerDelegate);

      _modelMethodInfo = default!;
      _componentEventInfo = default!;
      _handlerDelegate = default!;

      base.ReleaseResources();
   }
}
