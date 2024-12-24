namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Abstract base class for a binding from a model property to a control 
///   property. Adds the infrastructure to monitor the model for property change
///   notifications.
/// </summary>
public abstract class ToControlPropertyBindingBase<M, P> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;
   protected Func<M, P> _propertyGetter = default!;

   /// <summary>
   ///   Initialize a new <see cref="ToControlPropertyBindingBase{M, P}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
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
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   /// </exception>
   public ToControlPropertyBindingBase(
      M model,
      String modelProperty,
      Func<M, P>? propertyGetter = null) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));

      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);

      Model.PropertyChanged += Model_PropertyChanged;
      _propertyGetter = propertyGetter ?? GetPropertyValue;
   }

   protected virtual void HandlePropertyChanged(PropertyChangedEventArgs e) { }

   protected override void ReleaseResources()
   {
      Model.PropertyChanged -= Model_PropertyChanged;
      _modelPropertyInfo = default!;

      base.ReleaseResources();
   }

   private P GetPropertyValue(M model) => (P)_modelPropertyInfo.GetValue(model)!;

   private void Model_PropertyChanged(Object? sender, PropertyChangedEventArgs e)
   {
      if (e.PropertyName == _modelPropertyInfo.Name || e.PropertyName == String.Empty)
      {
         HandlePropertyChanged(e);
      }
   }
}
