namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one way binding from a target object property to a model 
///   property. The model property is updated whenever the target object 
///   broadcasts a notification that its property has changed.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="T">
///   The target object type.
/// </typeparam>
/// <typeparam name="E">
///   The target event argument type.
/// </typeparam>
/// <typeparam name="P">
///   The type of the model's bound property.
/// </typeparam>
public class TargetPropertyBinding<M, T, E, P> : MvcBindingBase<M>
   where M : IMvcModel
   where E : EventArgs
{
   protected PropertyInfo _modelPropertyInfo;
   protected PropertyInfo _targetPropertyInfo;
   protected Func<T, P> _propertyGetter;
   protected EventInfo _targetPropertyChangedEventInfo;
   protected Delegate _handlerDelegate;

   /// <summary>
   ///   Initialize a new <see cref="TargetPropertyBinding{M, T, E, P}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to update when the control property changes.
   /// </param>
   /// <param name="target">
   ///   The target object to monitor for changes.
   /// </param>
   /// <param name="targetProperty">
   ///   The name of the control property to monitor for changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="targetPropertyChangedEvent">
   ///   Optional. The name of the target property changed event. Defaults to
   ///   <paramref name="targetProperty"/> + "Changed".
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the target object property value and 
   ///   possibly converts the value to one suitable to assign to the model
   ///   property. Defaults to a function that simply gets the target object
   ///   property value.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="target"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="targetProperty"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="targetProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="target"/> does not implement a property named 
   ///   <paramref name="targetProperty"/>.
   ///   - or -
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   /// </exception>
   public TargetPropertyBinding(
      M model,
      T target,
      String targetProperty,
      String modelProperty,
      String? targetPropertyChangedEvent = null,
      Func<T, P>? propertyGetter = null) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(target, nameof(target));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(targetProperty, nameof(targetProperty));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));

      Target = target;
      _targetPropertyInfo = target.GetPropertyInfo(targetProperty);
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      targetPropertyChangedEvent ??= targetProperty + "Changed";
      _targetPropertyChangedEventInfo = Target.GetEventInfo(targetPropertyChangedEvent);
      var handlerMethodInfo = this.GetMethodInfo(nameof(Source_PropertyChanged));
      _handlerDelegate = Delegate.CreateDelegate(
         _targetPropertyChangedEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _targetPropertyChangedEventInfo.AddEventHandler(Target, _handlerDelegate);

      _propertyGetter = propertyGetter ?? GetPropertyValue;
   }

   /// <summary>
   ///   The bound object that is the source of values to send to the model.
   /// </summary>
   public T Target { get; private set; }

#pragma warning disable IDE0060 // Remove unused parameter
   public void Source_PropertyChanged(Object? sender, E e)
   {
      var value = _propertyGetter(Target);
      _modelPropertyInfo.SetValue(Model, value);
   }
#pragma warning restore IDE0060 // Remove unused parameter

   protected override void ReleaseResources()
   {
      _targetPropertyChangedEventInfo.RemoveEventHandler(Target, _handlerDelegate);

      _modelPropertyInfo = default!;
      _targetPropertyInfo = default!;
      _targetPropertyChangedEventInfo = default!;
      _handlerDelegate = default!;
      Target = default!;

      base.ReleaseResources();
   }

   private P GetPropertyValue(T source) => (P)_targetPropertyInfo.GetValue(source)!;
}
