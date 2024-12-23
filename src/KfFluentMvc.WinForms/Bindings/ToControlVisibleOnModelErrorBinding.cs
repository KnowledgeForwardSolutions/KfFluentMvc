namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one-way binding from a model property to a control's Visible
///   property. The control's Visible property is set to <see langword="true"/>
///   if the model property has an error, otherwise it is set to 
///   <see langword="false"/>.
/// </summary>
public class ToControlVisibleOnModelErrorBinding<M> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;

   /// <summary>
   ///   Initialize a new <see cref="ToControlVisibleOnModelErrorBinding{M}"/>.
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
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="control"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   ///   - or -
   ///   <paramref name="model"/> does not implement 
   ///   <see cref="IValidatingMvcModel"/>.
   /// </exception>
   public ToControlVisibleOnModelErrorBinding(
      M model,
      Control control,
      String modelProperty) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));
      if (model is not IValidatingMvcModel)
      {
         throw new InvalidOperationException(Messages.ModelMustBeIValidatingMvcModel);
      }

      Control = control;
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);

      Model.PropertyChanged += Model_PropertyChanged;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Control { get; private set; }

   protected override void ReleaseResources()
   {
      Model.PropertyChanged -= Model_PropertyChanged;
      _modelPropertyInfo = default!;
      Control = default!;

      base.ReleaseResources();
   }

   private void Model_PropertyChanged(Object? sender, PropertyChangedEventArgs e)
   {
      if (e.PropertyName == _modelPropertyInfo.Name || e.PropertyName == String.Empty)
      {
         Control.Visible = ((IValidatingMvcModel)Model).Errors.PropertyHasError(_modelPropertyInfo.Name);
      }
   }
}
