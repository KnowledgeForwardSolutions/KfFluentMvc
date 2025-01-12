namespace KfFluentMvc.WinForms;

/// <summary>
///   Methods that extend <see cref="MvcBuilder{M}"/> capabilities with 
///   <see cref="Component"/>s.
/// </summary>
public static class ComponentExtensions
{
   /// <summary>
   ///   Create a binding that invokes a model method in response to a 
   ///   component's Click event.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="builder"/> CurrentComponent is not of type
   ///   <see cref="ToolStripItem"/>.
   /// </exception>
   public static MvcBuilder<M> BindFromToolStripItemClickEvent<M>(
      this MvcBuilder<M> builder,
      String modelMethod) where M : IMvcModel
      => builder.CurrentComponent is not ToolStripItem
         ? throw new InvalidOperationException(Messages.ComponentMustBeToolStripItem)
         : builder.BindFromComponentEvent<EventArgs>(nameof(ToolStripItem.Click), modelMethod);
}
