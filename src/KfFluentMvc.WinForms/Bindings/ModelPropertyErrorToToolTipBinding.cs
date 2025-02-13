namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one-way binding from a model property to a target control's  
///   associated <see cref="ToolTip"/>. The ToolTip text is set to the model 
///   property's error message(s) or to <see cref="String.Empty"/> if the model
///   property does not have errors.
/// </summary>
public class ModelPropertyErrorToToolTipBinding<M> : ModelPropertyBindingBase<M, Object>
   where M : IValidatingMvcModel
{
   /// <summary>
   ///   Initialize a new <see cref="ModelPropertyErrorToToolTipBinding{M}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="target">
   ///   The <see cref="Control"/> linked to the <see cref="ToolTip"/>.
   /// </param>
   /// <param name="toolTip">
   ///   The <see cref="ToolTip"/> to update when the model property changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="target"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="toolTip"/> is <see langword="null"/>.
   /// </exception>
   public ModelPropertyErrorToToolTipBinding(
      M model,
      Control target,
      ToolTip toolTip,
      String modelProperty) : base(model, modelProperty)
   {
      ArgumentNullException.ThrowIfNull(target, nameof(target));
      ArgumentNullException.ThrowIfNull(toolTip, nameof(toolTip));

      Target = target;
      ToolTip = toolTip;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Target { get; private set; }

   /// <summary>
   ///   The bound <see cref="ToolTip"/>.
   /// </summary>
   public ToolTip ToolTip { get; private set; }

   protected override void HandlePropertyChanged(PropertyChangedEventArgs e)
   {
      var messages = Model.Errors[_modelPropertyInfo.Name];
      ToolTip.SetToolTip(Target, String.Join("\n\n", messages));
   }

   protected override void ReleaseResources()
   {
      Target = default!;
      ToolTip = default!;

      base.ReleaseResources();
   }
}
