namespace KfFluentMvc.WinForms;

/// <summary>
///   Methods that extend <see cref="MvcBuilder{M}"/> capabilities with 
///   <see cref="NumericUpDown"/> controls.
/// </summary>
public static class NumericUpDownExtensions
{
   public static MvcBuilder<M> BindFromNumericUpDownValueProperty<M, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<NumericUpDown, P>? propertyGetter = null) where M : IMvcModel
      => builder.BindFromTargetProperty<NumericUpDown, EventArgs, P>(
         nameof(NumericUpDown.Value),
         modelProperty,
         propertyGetter: propertyGetter);

   public static MvcBuilder<M> BindToNumericUpDownValueProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Decimal>? propertyGetter = null) where M : IMvcModel
      => builder.BindToTargetProperty<NumericUpDown, Decimal>(
         modelProperty,
         nameof(NumericUpDown.Value),
         propertyGetter);

   public static MvcBuilder<M> BindToFromNumericUpDownValueProperty<M, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Decimal>? modelPropertyGetter = null,
      Func<NumericUpDown, P>? controlPropertyGetter = null) where M : IMvcModel
   {
      builder.BindToNumericUpDownValueProperty(modelProperty, modelPropertyGetter);
      builder.BindFromNumericUpDownValueProperty<M, P>(modelProperty, controlPropertyGetter);

      return builder;
   }
}
