namespace KfFluentMvc.WinForms;

/// <summary>
///   Methods that extend <see cref="MvcBuilder{M}"/> capabilities with 
///   <see cref="DataGridView"/> controls.
/// </summary>
public static class DataGridViewExtensions
{
   /// <summary>
   ///   Create a one-way binding from a model property to a 
   ///   <see cref="DataGridViewColumn"/>'s Visible property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
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
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindToDataGridViewColumnVisibleProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Boolean>? propertyGetter = null) where M : IMvcModel
      => builder.BindToTargetProperty<DataGridViewColumn, Boolean>(
         modelProperty,
         nameof(DataGridViewColumn.Visible),
         propertyGetter);
}
