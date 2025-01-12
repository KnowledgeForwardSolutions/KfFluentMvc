namespace KfFluentMvc.WinForms;

/// <summary>
///   Methods that extend <see cref="MvcBuilder{M}"/> capabilities with 
///   <see cref="Control"/>s.
/// </summary>
public static class ControlExtensions
{
   /// <summary>
   ///   Create a binding that invokes a model method in response to a control's 
   ///   Click event.
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
   public static MvcBuilder<M> BindFromControlClickEvent<M>(
      this MvcBuilder<M> builder,
      String modelMethod) where M : IMvcModel
      => builder.BindFromControlEvent<EventArgs>(nameof(Control.Click), modelMethod);

   /// <summary>
   ///   Create a binding that invokes an <see cref="Action{M,Control}"/> in
   ///   response to a control's Click event.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="action">
   ///   The action to perform when the control's Click event fires.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindFromControlClickEvent<M>(
      this MvcBuilder<M> builder,
      Action<M, Control> action) where M : IMvcModel
      => builder.BindFromControlEvent<EventArgs>(nameof(Control.Click), action);

   /// <summary>
   ///   Create a binding that invokes an action that involves a second control
   ///   in response to the primary control's Click event.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="secondaryControl">
   ///   The secondary control in the interaction.
   /// </param>
   /// <param name="action">
   ///   Action to perform when the primary control event occurs.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The <paramref name="builder"/> CurrentControl type does not match the 
   ///   <typeparamref name="C1"/> type.
   /// </exception>
   public static MvcBuilder<M> BindFromControlClickEventWithSecondaryControl<M, C1, C2>(
      this MvcBuilder<M> builder,
      C2 secondaryControl,
      Action<M, C1, C2> action) 
      where M : IMvcModel
      where C1 : Control
      where C2 : Control
   {
      if (builder.CurrentControl is not C1 primaryControl)
      {
         throw new InvalidOperationException(Messages.PrimaryControlIncorrectType);
      }

      var binding = new TwoControlInteractionBinding<M, C1, C2>(
         builder.Model,
         primaryControl,
         nameof(Control.Click),
         secondaryControl,
         action);
      builder.WithBinding(binding);

      return builder;
   }

   /// <summary>
   ///   Create a binding that invokes an action that involves a second control
   ///   in response to the primary control's DoubleClick event.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="secondaryControl">
   ///   The secondary control in the interaction.
   /// </param>
   /// <param name="action">
   ///   Action to perform when the primary control event occurs.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The <paramref name="builder"/> CurrentControl type does not match the 
   ///   <typeparamref name="C1"/> type.
   /// </exception>
   public static MvcBuilder<M> BindFromControlDoubleClickEventWithSecondaryControl<M, C1, C2>(
      this MvcBuilder<M> builder,
      C2 secondaryControl,
      Action<M, C1, C2> action)
      where M : IMvcModel
      where C1 : Control
      where C2 : Control
   {
      if (builder.CurrentControl is not C1 primaryControl)
      {
         throw new InvalidOperationException(Messages.PrimaryControlIncorrectType);
      }

      var binding = new TwoControlInteractionBinding<M, C1, C2>(
         builder.Model,
         primaryControl,
         nameof(Control.DoubleClick),
         secondaryControl,
         action);
      builder.WithBinding(binding);

      return builder;
   }

   /// <summary>
   ///   Create a one-way binding from a control's Enabled property to a model 
   ///   property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the control property and possibly converts
   ///   the control property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the control property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindFromControlEnabledProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<Control, Boolean>? propertyGetter) where M : IMvcModel
      => builder.BindFromControlProperty<EventArgs, Boolean>(
         nameof(Control.Enabled), 
         modelProperty, 
         propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a control's Text property to a model 
   ///   property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the control property and possibly converts
   ///   the control property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the control property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindFromControlTextProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<Control, String>? propertyGetter = null) where M : IMvcModel
      => builder.BindFromControlProperty<EventArgs, String>(
         nameof(Control.Text), 
         modelProperty, 
         propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a control's Visible property to a model 
   ///   property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the control property and possibly converts
   ///   the control property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the control property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindFromControlVisibleProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<Control, Boolean>? propertyGetter) where M : IMvcModel
      => builder.BindFromControlProperty<EventArgs, Boolean>(
         nameof(Control.Visible), 
         modelProperty, 
         propertyGetter: propertyGetter);

   /// <summary>
   ///   Create a binding to a model property that sets a control's Visible
   ///   property to <see langword="true"/> if the property has a validation
   ///   error.
   /// </summary>
   /// <remarks>
   ///   The model must implement <see cref="IValidatingMvcModel"/>.
   /// </remarks>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for errors.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindModelPropertyErrorToControlVisible<M>(
      this MvcBuilder<M> builder,
      String modelProperty) where M : IMvcModel
   {
      var binding = new ModelPropertyErrorToControlVisibleBinding<M>(
         builder.Model,
         builder.CurrentControl,
         modelProperty);
      builder.WithBinding(binding);

      return builder;
   }

   /// <summary>
   ///   Create a binding to a model property that sets a <see cref="ToolTip"/>
   ///   message if the property has a validation error or sets  the message
   ///   to <see cref="String.Empty"/> if the property has no errors.
   /// </summary>
   /// <remarks>
   ///   The model must implement <see cref="IValidatingMvcModel"/>.
   /// </remarks>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for errors.
   /// </param>
   /// <param name="toolTip">
   ///   The <see cref="ToolTip"/> to update when the model property changes.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindModelPropertyErrorToToolTip<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      ToolTip toolTip) where M : IMvcModel
   {
      var binding = new ModelPropertyErrorToToolTipBinding<M>(
         builder.Model,
         builder.CurrentControl,
         toolTip,
         modelProperty);
      builder.WithBinding(binding);

      return builder;
   }

   /// <summary>
   ///   Create a one-way binding from a model property to a control's BackColor
   ///   property.
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
   public static MvcBuilder<M> BindToControlBackColorProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Color>? propertyGetter = null) where M : IMvcModel
      => builder.BindToControlProperty<Color>(modelProperty, nameof(Control.BackColor), propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control's 
   ///   DataSource property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindToControlDataSourceProperty<M, I>(
      this MvcBuilder<M> builder,
      String modelProperty) where M : IMvcModel
      => builder.BindToControlProperty<BindingList<I>>(modelProperty, nameof(ListControl.DataSource));

   /// <summary>
   ///   Create a one-way binding from a model property to a control's Enabled
   ///   property.
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
   public static MvcBuilder<M> BindToControlEnabledProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Boolean>? propertyGetter = null) where M : IMvcModel
      => builder.BindToControlProperty<Boolean>(modelProperty, nameof(Control.Enabled), propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control's ForeColor
   ///   property.
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
   public static MvcBuilder<M> BindToControlForeColorProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Color>? propertyGetter = null) where M : IMvcModel
      => builder.BindToControlProperty<Color>(modelProperty, nameof(Control.ForeColor), propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control's Text
   ///   property.
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
   public static MvcBuilder<M> BindToControlTextProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, String>? propertyGetter = null) where M : IMvcModel
      => builder.BindToControlProperty<String>(modelProperty, nameof(Control.Text), propertyGetter);

   /// <summary>
   ///   Create a one-way binding from a model property to a control's Visible
   ///   property.
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
   public static MvcBuilder<M> BindToControlVisibleProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<M, Boolean>? propertyGetter = null) where M : IMvcModel
      => builder.BindToControlProperty<Boolean>(modelProperty, nameof(Control.Visible), propertyGetter);

   /// <summary>
   ///   Create a two-way binding between a model property and a control's Text
   ///   property. The type of the model property is assumed to be 
   ///   <see cref="String"/>.
   /// </summary>
   /// <remarks>
   ///   This is a convenience method that combines two one-way binding methods,
   ///   <see cref="BindToControlTextProperty{M}(MvcBuilder{M}, String, Func{M, String}?)"/>
   ///   and <see cref="BindFromControlTextProperty{M}(MvcBuilder{M}, String, Func{Control, String}?)"/>
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
   public static MvcBuilder<M> BindToFromControlTextProperty<M>(
      this MvcBuilder<M> builder,
      String modelProperty) where M : IMvcModel
   {
      builder.BindToControlTextProperty(modelProperty);
      builder.BindFromControlTextProperty(modelProperty);

      return builder;
   }
}
