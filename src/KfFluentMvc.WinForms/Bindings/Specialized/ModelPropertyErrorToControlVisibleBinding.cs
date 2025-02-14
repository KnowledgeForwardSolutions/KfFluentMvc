namespace KfFluentMvc.WinForms.Bindings.Specialized;

/// <summary>
///   Defines a one-way binding from a model property to a target control's 
///   Visible property. The target control's Visible property is set to 
///   <see langword="true"/> if the model property has an error, otherwise it is 
///   set to <see langword="false"/>.
/// </summary>
public class ModelPropertyErrorToControlVisibleBinding<M> : ModelPropertyBindingBase<M, Object>
   where M : IValidatingMvcModel
{
   /// <summary>
   ///   Initialize a new <see cref="ModelPropertyErrorToControlVisibleBinding{M}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="target">
   ///   The target control to update when the model property changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="target"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   /// </exception>
   public ModelPropertyErrorToControlVisibleBinding(
      M model,
      Control target,
      String modelProperty) : base(model, modelProperty)
   {
      ArgumentNullException.ThrowIfNull(target, nameof(target));

      Target = target;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public Control Target { get; private set; }

   protected override void HandlePropertyChanged(PropertyChangedEventArgs e) =>
      Target.Visible = Model.Errors.PropertyHasError(_modelPropertyInfo.Name);

   protected override void ReleaseResources()
   {
      Target = default!;

      base.ReleaseResources();
   }
}
