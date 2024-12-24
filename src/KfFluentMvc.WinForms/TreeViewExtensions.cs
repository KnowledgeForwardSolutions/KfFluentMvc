namespace KfFluentMvc.WinForms;

/// <summary>
///   Methods that extend <see cref="MvcBuilder{M}"/> capabilities with 
///   <see cref="TreeView"/> controls.
/// </summary>
public static class TreeViewExtensions
{
   /// <summary>
   ///   Create a one-way binding from a <see cref="TreeView"/> control's 
   ///   AfterSelect event to a model property. When the AfterSelect event fires
   ///   the model property is updated with a value derived from the TreeView's
   ///   SelectedNode property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
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
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The current control is not of type <see cref="TreeView"/>.
   /// </exception>
   public static MvcBuilder<M> BindFromTreeViewAfterSelectEvent<M, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<TreeNode?, P?>? selectedValueGetter = null) where M : IMvcModel
   {
      if (builder.CurrentControl is not TreeView treeView)
      {
         throw new InvalidOperationException(Messages.ControlMustBeTreeView);
      }

      var binding = new FromTreeViewAfterSelectEventBinding<M, P>(
         builder.Model,
         treeView,
         modelProperty,
         selectedValueGetter);
      builder.WithBinding(binding);

      return builder;
   }

   /// <summary>
   ///   Create a one-way binding from a model items collection property to a 
   ///   <see cref="TreeView"/> control's Nodes collection. When the model 
   ///   property changes, the Nodes collection is cleared and then refreshed 
   ///   with the current model property contents.
   /// </summary>
   /// <typeparam name="I">
   ///   The model item type.
   /// </typeparam>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="collectionMapper">
   ///   Function that maps the model items property to a collection of 
   ///   <see cref="TreeNode"/>s. This function is responsible for handling the
   ///   node hierarchy if more than one level deep.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The current control is not of type <see cref="TreeView"/>.
   /// </exception>
   public static MvcBuilder<M> BindToTreeViewNodesProperty<M, I>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<IEnumerable<I>?, IEnumerable<TreeNode>> collectionMapper) where M : IMvcModel
   {
      if (builder.CurrentControl is not TreeView treeView)
      {
         throw new InvalidOperationException(Messages.ControlMustBeTreeView);
      }

      var binding = new ToTreeViewNodesPropertyBinding<M, I>(
         builder.Model,
         treeView,
         modelProperty,
         collectionMapper);
      builder.WithBinding(binding);

      return builder;
   }

   /// <summary>
   ///   Create a one-way binding from a model property to a 
   ///   <see cref="TreeView"/> control's SelectedItem property.
   /// </summary>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
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
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   /// <exception cref="InvalidOperationException">
   ///   The current control is not of type <see cref="TreeView"/>.
   /// </exception>
   public static MvcBuilder<M> BindToTreeViewSelectedItemProperty<M, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<P, String>? keyGetter = null) where M : IMvcModel
   {
      if (builder.CurrentControl is not TreeView treeView)
      {
         throw new InvalidOperationException(Messages.ControlMustBeTreeView);
      }

      var binding = new ToTreeViewSelectedItemPropertyBinding<M, P>(
         builder.Model,
         treeView,
         modelProperty,
         keyGetter);
      builder.WithBinding(binding);

      return builder;
   }

   /// <summary>
   ///   Create a two-way binding between a model property and a 
   ///   <see cref="TreeView"/> control's SelectedItem property.
   /// </summary>
   /// <remarks>
   ///   This is a convenience method that combines two one-way binding methods, 
   ///   <see cref="BindToTreeViewSelectedItemProperty{M, P}(MvcBuilder{M}, String, Func{P, String}?)"/>
   ///   and <see cref="BindFromTreeViewAfterSelectEvent{M, P}(MvcBuilder{M}, String, Func{TreeNode?, P?}?)"/>
   ///   that are commonly used together.
   /// </remarks>
   /// <param name="builder">
   ///   The <see cref="MvcBuilder{M}"/> object.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to bind to the TreeView SelectedItem
   ///   property.
   /// </param>
   /// <param name="keyGetter">
   ///   Optional. Function that converts the model property value to a 
   ///   <see cref="String"/> key used to locate the matching node in the
   ///   <see cref="TreeView.Nodes"/> collection. Defaults to invoking the model
   ///   property value's ToString method.
   /// </param>
   /// <param name="selectedValueGetter">
   ///   Optional. Function that converts the <see cref="TreeView"/>'s 
   ///   SelectedNode property to a value suitable to assign to the model
   ///   property. Defaults to a function that simply gets the 
   ///   <see cref="TreeNode.Tag"/> of the selected node.
   /// </param>
   /// <returns>
   ///   A reference to the <see cref="MvcBuilder{M}"/> to support method 
   ///   chaining.
   /// </returns>
   public static MvcBuilder<M> BindToFromTreeViewSelectedItemProperty<M, P>(
      this MvcBuilder<M> builder,
      String modelProperty,
      Func<P, String>? keyGetter = null,
      Func<TreeNode?, P?>? selectedValueGetter = null) where M : IMvcModel
   {
      builder.BindToTreeViewSelectedItemProperty(modelProperty, keyGetter);
      builder.BindFromTreeViewAfterSelectEvent(modelProperty, selectedValueGetter);

      return builder;
   }
}
