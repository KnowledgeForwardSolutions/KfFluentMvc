namespace KfFluentMvc.WinForms;

/// <summary>
///   Methods that extend <see cref="MvcBuilder{M}"/> capabilities with 
///   <see cref="NumericUpDown"/> controls.
/// </summary>
public static class NumericUpDownExtensions
{
   /// <summary>
   ///   Create a one-way binding from a <see cref="NumericUpDown"/> control's 
   ///   Value property to a model property
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="targetPropertyGetter">
   ///   Optional. Function that gets the target control's Value property and 
   ///   possibly converts the value to one suitable to assign to the model
   ///   property. Defaults to a function that simply gets the target control's 
   ///   Value property value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindFromNumericUpDownValueProperty<M, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<NumericUpDown, P>? propertyGetter = null) where M : IMvcModel
      => builder.BindFromTargetProperty<NumericUpDown, EventArgs, P>(
         nameof(NumericUpDown.Value),
         modelProperty,
         propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a 
   ///   <see cref="NumericUpDown"/> control's Value property.
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
   ///   control's Value property. Defaults to a function that simply gets the 
   ///   model property value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindToNumericUpDownValueProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Decimal>? propertyGetter = null) where M : IMvcModel
      => builder.BindToTargetProperty<NumericUpDown, Decimal>(
         modelProperty,
         nameof(NumericUpDown.Value),
         propertyGetter);

   /// <summary>
   ///   Create a two-way binding between a model property and a
   ///   <see cref="NumericUpDown"/> control's Value property.
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
   /// <param name="modelPropertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the target 
   ///   control's Value property. Defaults to a function that simply gets the 
   ///   model property value.
   /// </param>
   /// <param name="targetPropertyGetter">
   ///   Optional. Function that gets the target control's Value property and 
   ///   possibly converts the value to one suitable to assign to the model
   ///   property. Defaults to a function that simply gets the target control's 
   ///   Value property value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindToFromNumericUpDownValueProperty<M, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Decimal>? modelPropertyGetter = null,
      Func<NumericUpDown, P>? targetPropertyGetter = null) where M : IMvcModel
   {
      builder.BindToNumericUpDownValueProperty(modelProperty, modelPropertyGetter);
      builder.BindFromNumericUpDownValueProperty<M, P>(modelProperty, targetPropertyGetter);

      return builder;
   }
}
