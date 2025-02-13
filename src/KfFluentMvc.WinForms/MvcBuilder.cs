// Ignore Spelling: Mvc

namespace KfFluentMvc.WinForms;

/// <summary>
///   Fluent builder for creating MVC bindings.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
public class MvcBuilder<M>
   where M : IMvcModel
{
   private readonly MvcBindingCollection<M> _bindingCollection = new();

   /// <summary>
   ///   Initialize a new <see cref="MvcBuilder{M}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to bind to <see cref="Component"/>s or 
   ///   <see cref="Control"/>s.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   /// </exception>
   public MvcBuilder(M model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));

      Model = model;
   }

   /// <summary>
   ///   The bound model.
   /// </summary>
   public M Model { get; private init; }

   /// <summary>
   ///   The <see cref="Control"/> to bind to.
   /// </summary>
   public Control CurrentControl { get; private set; } = default!;

   /// <summary>
   ///   The target <see cref="Object"/> to bind to.
   /// </summary>
   public Object CurrentTarget { get; private set; } = default!;

   /// <summary>
   ///   Create a binding that invokes a model method in response to a control 
   ///   event.
   /// </summary>
   /// <param name="targetEvent">
   ///   The name of the target object event to monitor.
   /// </param>
   /// <param name="modelMethod">
   ///   The name of the method to invoke on the model.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindFromTargetEvent<T, E>(
      String targetEvent,
      String modelMethod) where E : EventArgs
   {
      ThrowIfTargetNotSet();
      if (CurrentTarget is not T target)
      {
         var message = String.Format(Messages.BoundObjectInvalidType, typeof(T).Name);
         throw new InvalidOperationException(message);
      }

      var binding = new TargetEventBinding<M, T, E>(Model, target, targetEvent, modelMethod);
      WithBinding(binding);

      return this;
   }

   /// <summary>
   ///   Create a binding that performs an action in response to a control 
   ///   event.
   /// </summary>
   /// <param name="targetEvent">
   ///   The name of the control event to monitor.
   /// </param>
   /// <param name="action">
   ///   The action to perform when the control event fires.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> BindFromTargetEvent<T, E>(
      String targetEvent,
      Action<M, T> action) where E : EventArgs
   {
      ThrowIfTargetNotSet();
      if (CurrentTarget is not T target)
      {
         var message = String.Format(Messages.BoundObjectInvalidType, typeof(T).Name);
         throw new InvalidOperationException(message);
      }

      var binding = new TargetEventActionBinding<M, T, E>(Model, target, targetEvent, action);
      WithBinding(binding);

      return this;
   }

   /// <summary>
   ///   Create a one-way binding from a target object property to a model 
   ///   property.
   /// </summary>
   /// <typeparam name="T">
   ///   The target object type.
   /// </typeparam>
   /// <typeparam name="E">
   ///   The target event type that notifies that a target property has changed.
   /// </typeparam>
   /// <typeparam name="P">
   ///   The type of the model's bound property.
   /// </typeparam>
   /// <param name="targetProperty">
   ///   The name of the target object property to monitor for changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to set when the target property changes.
   /// </param>
   /// <param name="targetPropertyChangedEvent">
   ///   Optional. The name of the target property changed event. Defaults to
   ///   <paramref name="targetProperty"/> + "Changed".
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the target object property value and 
   ///   possibly converts the value to one suitable to assign to the model
   ///   property. Defaults to a function that simply gets the target object
   ///   property value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   Attempt to invoke this method without first invoking the 
   ///   <see cref="WithTarget(Object)"/> method.
   ///   - or -
   ///   The <see cref="CurrentTarget"/> is not of type {T}.
   /// </exception>
   public MvcBuilder<M> BindFromTargetProperty<T, E, P>(
      String targetProperty,
      String modelProperty,
      String? targetPropertyChangedEvent = null,
      Func<T, P>? propertyGetter = null) where E : EventArgs
   {
      ThrowIfTargetNotSet();
      if (CurrentTarget is not T target)
      {
         var message = String.Format(Messages.BoundObjectInvalidType, typeof(T).Name);
         throw new InvalidOperationException(message);
      }

      var binding = new TargetPropertyBinding<M, T, E, P>(
         Model,
         target,
         targetProperty,
         modelProperty,
         targetPropertyChangedEvent,
         propertyGetter);
      WithBinding(binding);

      return this;
   }

   /// <summary>
   ///   Create a one-way binding from a model property to a target object
   ///   property. 
   /// </summary>
   /// <typeparam name="T">
   ///   The target object type.
   /// </typeparam>
   /// <typeparam name="P">
   ///   The target object property type.
   /// </typeparam>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="targetProperty">
   ///   The name of the target property to set when the model property changes.
   /// </param>
   /// <param name="propertyGetter">
   ///   Optional. Function that gets the model property and possibly converts
   ///   the model property to a value suitable to assign to the target
   ///   property. Defaults to a function that simply gets the model property
   ///   value.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   Attempt to invoke this method without first invoking the 
   ///   <see cref="WithTarget(Object)"/> method.
   ///   - or -
   ///   The <see cref="CurrentTarget"/> is not of type {T}.
   /// </exception>
   public MvcBuilder<M> BindToTargetProperty<T, P>(
      String modelProperty,
      String targetProperty,
      Func<M, P>? propertyGetter = null)
   {
      ThrowIfTargetNotSet();
      if (CurrentTarget is not T target)
      {
         var message = String.Format(Messages.BoundObjectInvalidType, typeof(T).Name);
         throw new InvalidOperationException(message);
      }

      var binding = new ModelPropertyBinding<M, T, P>(
         Model,
         target,
         modelProperty,
         targetProperty,
         propertyGetter);
      WithBinding(binding);

      return this;
   }

   /// <summary>
   ///   Complete the build process.
   /// </summary>
   /// <returns>
   ///   A collection of the completed bindings.
   /// </returns>
   public MvcBindingCollection<M> Build() => _bindingCollection;

   /// <summary>
   ///   Create a new <see cref="MvcBuilder{M}"/> to bind a model to 
   ///   <see cref="Component"/>s or <see cref="Control"/>s.
   /// </summary>
   /// <param name="model">
   ///   The model to bind.
   /// </param>
   /// <returns>
   ///   A new builder object.
   /// </returns>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   /// </exception>
   public static MvcBuilder<M> CreateBuilder(M model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));

      return new(model);
   }

   /// <summary>
   ///   Append a new binding.
   /// </summary>
   /// <param name="binding">
   ///   The binding to append.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="binding"/> is <see langword="null"/>.
   /// </exception>
   public MvcBuilder<M> WithBinding(IModelBinding<M> binding)
   {
      ArgumentNullException.ThrowIfNull(binding, nameof(binding));

      _bindingCollection.AddBinding(binding);

      return this;
   }

   /// <summary>
   ///   Set the <see cref="Control"/> that future bindings will bind to.
   /// </summary>
   /// <param name="control">
   ///   The next <see cref="Control"/> to bind the model to.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> WithControl(Control control)
   {
      ArgumentNullException.ThrowIfNull(control, nameof(control));

      CurrentControl = control;
      CurrentTarget = default!;

      return this;
   }

   /// <summary>
   ///   Set the target <see cref="Object"/> that future bindings will bind to.
   /// </summary>
   /// <param name="target">
   ///   The next target <see cref="Object"/> to bind the model to.
   /// </param>
   /// <returns>
   ///   A reference to this <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public MvcBuilder<M> WithTarget(Object target)
   {
      ArgumentNullException.ThrowIfNull(target, nameof(target));

      CurrentTarget = target!;
      CurrentControl = default!;

      return this;
   }

   private void ThrowIfTargetNotSet()
   {
      if (CurrentTarget is null)
      {
         throw new InvalidOperationException(Messages.BindingTargetNotSet);
      }
   }
}
