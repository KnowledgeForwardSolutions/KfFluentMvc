namespace KfFluentMvc.WinForms;

/// <summary>
///   Methods that extend <see cref="MvcBuilder{M}"/> capabilities with 
///   <see cref="ListControl"/> controls.
/// </summary>
public static class ListControlExtensions
{
   /// <summary>
   ///   Create a one-way binding from a <see cref="ComboBox"/> or a 
   ///   <see cref="ListBox"/> control's SelectedItem to a model property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="targetPropertyGetter">
   ///   Optional. Function that gets the target control's SelectedItem property 
   ///   and possibly converts the value to one suitable to assign to the model
   ///   property. Defaults to a function that simply gets the target control's 
   ///   Value property value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <remarks>
   ///   Neither <see cref="ComboBox"/> nor <see cref="ListBox"/> have a 
   ///   SelectedItemChanged event so this binding listens for the 
   ///   SelectedIndexChanged event and retrieves the SelectedItem when the
   ///   event fires.
   /// </remarks>
   public static MvcBuilder<M> BindFromListControlSelectedItemProperty<M, T, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<T, P>? propertyGetter = null)
      where M : IMvcModel
      where T : ListControl
      => builder.BindFromTargetProperty<T, EventArgs, P>(
         nameof(ComboBox.SelectedItem),
         modelProperty,
         nameof(ComboBox.SelectedIndexChanged),
         propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a 
   ///   <see cref="ComboBox"/> or a <see cref="ListBox"/> control's 
   ///   SelectedItem property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="modelPropertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the target 
   ///   control's SelectedItem property. Defaults to a function that simply 
   ///   gets the model property value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindToListControlSelectedItemProperty<M, T, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, P>? propertyGetter = null) 
      where M : IMvcModel
      where T : ListControl
      => builder.BindToTargetProperty<T, P>(
         modelProperty,
         nameof(ComboBox.SelectedItem),
         propertyGetter);

   /// <summary>
   ///   Create a two-way binding between a model property and a
   ///   <see cref="ComboBox"/> or a <see cref="ListBox"/> control's 
   ///   SelectedItem property.
   /// </summary>
   /// <remarks>
   ///   This is a convenience method that combines two one-way binding methods,
   ///   <see cref="BindToNumericUpDownValueProperty{M}(MvcBuilder{M}, String, Func{M, Decimal}?)"/>
   ///   and <see cref="BindFromNumericUpDownValueProperty{M, P}(MvcBuilder{M}, String, Func{NumericUpDown, P}?)"/>
   ///   that are commonly used together.
   /// </remarks>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to bind to the control Text property.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindToFromListControlSelectedItemProperty<M, T, P>(
      this MvcBuilder<M> builder,
      String modelProperty) 
      where M : IMvcModel
      where T : ListControl
   {
      builder.BindToListControlSelectedItemProperty<M, T, P>(modelProperty);
      builder.BindFromListControlSelectedItemProperty<M, T, P>(modelProperty);

      return builder;
   }
}
