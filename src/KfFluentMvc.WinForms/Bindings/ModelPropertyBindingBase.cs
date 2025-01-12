namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Abstract base class for a binding a model property. Extends 
///   <see cref="MvcBindingBase{M}"/> to handle monitoring the model for 
///   property change notifications and for retrieving the model property value.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="P">
///   The type of the control's bound property.
/// </typeparam>
public abstract class ModelPropertyBindingBase<M, P> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;
   protected Func<M, P> _propertyGetter = default!;

   /// <summary>
   ///   Initialize a new <see cref="ModelPropertyBindingBase{M, P}"/>.
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
   public ModelPropertyBindingBase(
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
