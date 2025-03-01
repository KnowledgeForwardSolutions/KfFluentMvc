namespace KfFluentMvc.WinForms.Bindings;

public class ModelPropertyActionBinding<M, T> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;
   protected Action<M, T> _action;

   public ModelPropertyActionBinding(
      M model,
      T target,
      String modelProperty,
      Action<M, T> action) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));
      ArgumentNullException.ThrowIfNull(action, nameof(action));

      Target = target;
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);
      _action = action;

      Model.PropertyChanged += Model_PropertyChanged;
   }

   /// <summary>
   ///   The bound target object.
   /// </summary>
   public T Target { get; private set; }

   protected override void ReleaseResources()
   {
      Model.PropertyChanged -= Model_PropertyChanged;
      Target = default!;
      _modelPropertyInfo = default!;
      _action = default!;

      base.ReleaseResources();
   }

   private void Model_PropertyChanged(Object? sender, PropertyChangedEventArgs e)
   {
      if (e.PropertyName == _modelPropertyInfo.Name || e.PropertyName == String.Empty)
      {
         _action(Model, Target);
      }
   }
}
