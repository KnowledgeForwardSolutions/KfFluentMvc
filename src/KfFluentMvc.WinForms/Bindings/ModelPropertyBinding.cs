namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one way binding from a model property to a target object
///   property. The target object property is updated whenever the model 
///   broadcasts a notification that its property has changed.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="T">
///   The type of the binding target object.
/// </typeparam>
/// <typeparam name="P">
///   The type of the target's bound property.
/// </typeparam>
public class ModelPropertyBinding<M, T, P> : ModelPropertyBindingBase<M, P>
   where M : IMvcModel
{
   protected PropertyInfo _targetPropertyInfo;

   /// <summary>
   ///   Initialize a new <see cref="ModelPropertyBinding{M, T, C}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="target">
   ///   The target object to update when the model property changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="targetProperty">
   ///   The name of the target property to set when the model property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the control
   ///   property. Defaults to a function that simply gets the model property
   ///   value.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="target"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="targetProperty"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   - or -
   ///   <paramref name="targetProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   ///   - or -
   ///   <paramref name="target"/> does not implement a property named 
   ///   <paramref name="targetProperty"/>.
   /// </exception>
   public ModelPropertyBinding(
      M model,
      T target,
      String modelProperty,
      String targetProperty,
      Func<M, P>? propertyGetter = null) : base(model, modelProperty, propertyGetter)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(target, nameof(target));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(targetProperty, nameof(targetProperty));

      Target = target;
      _targetPropertyInfo = Target.GetPropertyInfo(targetProperty);
   }

   /// <summary>
   ///   The bound target object.
   /// </summary>
   public T Target { get; private set; }

   protected override void ReleaseResources()
   {
      _targetPropertyInfo = default!;
      Target = default!;

      base.ReleaseResources();
   }

   protected override void HandlePropertyChanged(PropertyChangedEventArgs e)
   {
      var value = _propertyGetter(Model);
      _targetPropertyInfo.SetValue(Target, value);
   }
}
