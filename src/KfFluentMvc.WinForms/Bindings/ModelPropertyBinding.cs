namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one way binding from a model property to a control property.
///   The control property is updated whenever the model broadcasts a 
///   notification that its property has changed.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="P">
///   The type of the control's bound property.
/// </typeparam>
public class ModelPropertyBinding<M, P> : ModelPropertyBindingBase<M, P>
   where M : IMvcModel
{
   protected PropertyInfo _controlPropertyInfo;

   /// <summary>
   ///   Initialize a new <see cref="ModelPropertyBinding{M, C}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="control">
   ///   The <see cref="Control"/> to update when the model property changes.
   /// </param>
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
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="control"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="controlProperty"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   - or -
   ///   <paramref name="controlProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   ///   - or -
   ///   <paramref name="control"/> does not implement a property named 
   ///   <paramref name="controlProperty"/>.
   /// </exception>
   public ModelPropertyBinding(
      M model, 
      Control control,
      String modelProperty,
      String controlProperty,
      Func<M, P>? propertyGetter = null) : base(model, modelProperty, propertyGetter)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(controlProperty, nameof(controlProperty));

      Control = control;
      _controlPropertyInfo = Control.GetPropertyInfo(controlProperty);
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Control { get; private set; }

   protected override void ReleaseResources()
   {
      _controlPropertyInfo = default!;
      Control = default!;

      base.ReleaseResources();
   }

   protected override void HandlePropertyChanged(PropertyChangedEventArgs e)
   {
      var value = _propertyGetter(Model);
      _controlPropertyInfo.SetValue(Control, value);
   }
}
