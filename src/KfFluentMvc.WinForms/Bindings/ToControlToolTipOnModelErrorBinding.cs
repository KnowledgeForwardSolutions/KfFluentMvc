namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one-way binding from a model property to a control's  
///   <see cref="ToolTip"/>. The ToolTip text is set to the bound property's
///   error message(s) or to <see cref="String.Empty"/> if the bound property
///   does not have errors.
/// </summary>
public class ToControlToolTipOnModelErrorBinding<M> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;

   /// <summary>
   ///   Initialize a new <see cref="ToControlToolTipOnModelErrorBinding{M}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="control">
   ///   The <see cref="Control"/> linked to the <see cref="ToolTip"/>.
   /// </param>
   /// <param name="toolTip">
   ///   The <see cref="ToolTip"/> to update when the model property changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="control"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="toolTip"/> is <see langword="null"/>.
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
   public ToControlToolTipOnModelErrorBinding(
      M model,
      Control control,
      ToolTip toolTip,
      String modelProperty) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNull(toolTip, nameof(toolTip));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));
      if (model is not IValidatingMvcModel)
      {
         throw new InvalidOperationException(Messages.ModelMustBeIValidatingMvcModel);
      }

      Control = control;
      ToolTip = toolTip;
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);

      Model.PropertyChanged += Model_PropertyChanged;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Control { get; private set; }

   /// <summary>
   ///   The bound <see cref="ToolTip"/>.
   /// </summary>
   public ToolTip ToolTip { get; private set; }

   protected override void ReleaseResources()
   {
      Model.PropertyChanged -= Model_PropertyChanged;
      Control = default!;
      ToolTip = default!;
      _modelPropertyInfo = default!;

      base.ReleaseResources();
   }

   private void Model_PropertyChanged(Object? sender, PropertyChangedEventArgs e)
   {
      if (e.PropertyName == _modelPropertyInfo.Name || e.PropertyName == String.Empty)
      {
         var messages = ((IValidatingMvcModel)Model).Errors[_modelPropertyInfo.Name];
         ToolTip.SetToolTip(Control, String.Join("\n\n", messages));
      }
   }
}
