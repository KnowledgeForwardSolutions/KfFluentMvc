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
public class ToControlPropertyBinding<M, P> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;
   protected PropertyInfo _controlPropertyInfo;
   protected Func<M, P> _propertyGetter;

   /// <summary>
   ///   Initialize a new <see cref="ToControlPropertyBinding{M, C}"/>.
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
   public ToControlPropertyBinding(
      M model, 
      Control control,
      String modelProperty,
      String controlProperty,
      Func<M, P>? propertyGetter = null) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(controlProperty, nameof(controlProperty));

      Control = control;
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);
      _controlPropertyInfo = Control.GetPropertyInfo(controlProperty);

      Model.PropertyChanged += Model_PropertyChanged;
      _propertyGetter = propertyGetter ?? GetPropertyValue;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Control { get; private set; }

   protected override void ReleaseResources()
   {
      Model.PropertyChanged -= Model_PropertyChanged;
      _modelPropertyInfo = default!;
      _controlPropertyInfo = default!;
      Control = default!;

      base.ReleaseResources();
   }

   private P GetPropertyValue(M model) => (P)_modelPropertyInfo.GetValue(model)!;

   private void Model_PropertyChanged(Object? sender, PropertyChangedEventArgs e)
   {
      if (e.PropertyName == _modelPropertyInfo.Name || e.PropertyName == String.Empty)
      {
         var value = _propertyGetter(Model);
         _controlPropertyInfo.SetValue(Control, value);
      }
   }
}
