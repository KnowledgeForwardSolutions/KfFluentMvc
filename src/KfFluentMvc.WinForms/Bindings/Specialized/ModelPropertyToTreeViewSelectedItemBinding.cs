namespace KfFluentMvc.WinForms.Bindings.Specialized;

/// <summary>
///   Defines a one-way binding from a model property to a 
///   <see cref="TreeView"/> control's SelectedItem property.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="P">
///   The type of the control's bound property.
/// </typeparam>
public class ModelPropertyToTreeViewSelectedItemBinding<M, P> : ModelPropertyBindingBase<M, P>
   where M : IMvcModel
{
   protected Func<P, String> _keyGetter;

   /// <summary>
   ///   Initialize a new <see cref="ModelPropertyToTreeViewSelectedItemBinding{M,P}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="target">
   ///   The <see cref="TreeView"/> to update when the model property changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="keyGetter">
   ///   Optional. Function that converts the model property value to a 
   ///   <see cref="String"/> key used to locate the matching node in the
   ///   <see cref="TreeView.Nodes"/> collection. Defaults to invoking the model
   ///   property value's ToString method.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="target"/> is <see langword="null"/>.
   /// </exception>
   public ModelPropertyToTreeViewSelectedItemBinding(
      M model,
      TreeView target,
      String modelProperty,
      Func<P, String>? keyGetter = null) : base(model, modelProperty)
   {
      ArgumentNullException.ThrowIfNull(target, nameof(target));

      Target = target;
      _keyGetter = keyGetter ?? GetKey;
   }

   /// <summary>
   ///   The target control.
   /// </summary>
   public TreeView Target { get; private set; }

   private static String GetKey(P propertyValue) => propertyValue!.ToString() ?? String.Empty;

   protected override void ReleaseResources()
   {
      _keyGetter = default!;
      Target = default!;

      base.ReleaseResources();
   }

   protected override void HandlePropertyChanged(PropertyChangedEventArgs e)
   {
      var propertyValue = (P)_modelPropertyInfo.GetValue(Model)!;
      TreeNode node = default!;
      if (propertyValue is not null)
      {
         var key = _keyGetter(propertyValue);
         var matchingNodes = Target.Nodes.Find(key, true);
         if (matchingNodes.Length > 0)
         {
            node = matchingNodes[0];
         }
      }

      Target.SelectedNode = node;
   }
}
