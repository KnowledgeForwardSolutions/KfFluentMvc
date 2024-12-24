﻿namespace KfFluentMvc.WinForms;

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
      => builder.BindFromControlEvent(nameof(Control.Click), modelMethod);

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
      => builder.BindFromControlProperty<Boolean>(nameof(Control.Enabled), modelProperty, propertyGetter: propertyGetter);

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
      => builder.BindFromControlProperty<String>(nameof(Control.Text), modelProperty, propertyGetter: propertyGetter);

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
      => builder.BindFromControlProperty<Boolean>(nameof(Control.Visible), modelProperty, propertyGetter: propertyGetter);

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
