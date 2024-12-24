namespace KfFluentMvc.WinForms.Bindings;

public class FromTreeViewAfterSelectEventBinding<M, P> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected PropertyInfo _modelPropertyInfo;
   protected Func<TreeNode?, P?> _selectedValueGetter;

   /// <summary>
   ///   Defines a one-way binding from a <see cref="TreeView"/> control's 
   ///   AfterSelect event to a model property. When the AfterSelect event fires
   ///   the model property is updated with a value derived from the TreeView's
   ///   SelectedNode property.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="control">
   ///   The control to monitor for event notifications.
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
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="control"/> is <see langword="null"/>.
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
      TreeView control,
      String modelProperty,
      Func<TreeNode?, P?>? selectedValueGetter = null) : base(model)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));

      Control = control;
      _modelPropertyInfo = Model.GetPropertyInfo(modelProperty);
      _selectedValueGetter = selectedValueGetter ?? GetSelectedValue;

      Control.AfterSelect += Control_AfterSelect;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public TreeView Control { get; private set; }

   private static P GetSelectedValue(TreeNode? node) => (P?)node?.Tag!;

   protected override void ReleaseResources()
   {
      Control.AfterSelect -= Control_AfterSelect;
      _modelPropertyInfo = default!;
      _selectedValueGetter = default!;
      Control = default!;

      base.ReleaseResources();
   }

   protected void Control_AfterSelect(Object? sender, TreeViewEventArgs e)
   {
      var value = _selectedValueGetter(Control.SelectedNode);
      _modelPropertyInfo.SetValue(Model, value);
   }
}
