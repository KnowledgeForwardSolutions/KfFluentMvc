namespace KfFluentMvc.WinForms.Bindings.Specialized;

/// <summary>
///   Defines a one-way binding from a <see cref="TreeView"/> control's 
///   AfterSelect event to a model property. When the AfterSelect event fires
///   the model property is updated with a value derived from the TreeView's
///   SelectedNode property.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="P">
///   The type of the model's bound property.
/// </typeparam>
public class FromTreeViewAfterSelectEventBinding<M, P> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;
   protected Func<TreeNode?, P?> _selectedValueGetter;

   /// <summary>
   ///   Initialize a new 
   ///   <see cref="FromTreeViewAfterSelectEventBinding{M, P}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="target">
   ///   The target <see cref="TreeView"/> control to monitor for event 
   ///   notifications.
   /// </param>
   /// <param name="modelProperty">
   ///   The model property to set when the control event changes.
   /// </param>
   /// <param name="selectedValueGetter">
   ///   Optional. Function that converts the <see cref="TreeView"/>'s 
   ///   SelectedNode property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the 
   ///   <see cref="TreeNode.Tag"/> of the selected node.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="target"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   /// </exception>
   public FromTreeViewAfterSelectEventBinding(
      M model,
      TreeView target,
      String modelProperty,
      Func<TreeNode?, P?>? selectedValueGetter = null) : base(model)
   {
      ArgumentNullException.ThrowIfNull(target, nameof(target));
      ArgumentException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));

      Target = target;
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);
      _selectedValueGetter = selectedValueGetter ?? GetSelectedValue;

      Target.AfterSelect += Control_AfterSelect;
   }

   /// <summary>
   ///   The target <see cref="TreeView"/> control.
   /// </summary>
   public TreeView Target { get; private set; }

   private static P GetSelectedValue(TreeNode? node) => (P?)node?.Tag!;

   protected override void ReleaseResources()
   {
      Target.AfterSelect -= Control_AfterSelect;
      _modelPropertyInfo = default!;
      _selectedValueGetter = default!;
      Target = default!;

      base.ReleaseResources();
   }

   protected void Control_AfterSelect(Object? sender, TreeViewEventArgs e)
   {
      var value = _selectedValueGetter(Target.SelectedNode);
      _modelPropertyInfo.SetValue(Model, value);
   }
}
