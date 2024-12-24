namespace KfFluentMvc.WinForms.Bindings;

/// <summary>
///   Defines a one-way binding from a model items collection property to a 
///   <see cref="TreeView"/> control's Nodes collection. When the model property
///   changes, the Nodes collection is cleared and then refreshed with the
///   current model property contents.
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="I">
///   The model item type.
/// </typeparam>
public class ToTreeViewNodesPropertyBinding<M, I> : ToControlPropertyBindingBase<M, IEnumerable<I>>
   where M : IMvcModel
{
   protected Func<IEnumerable<I>?, IEnumerable<TreeNode>> _collectionMapper;

   /// <summary>
   ///   Initialize a new <see cref="ToTreeViewNodesPropertyBinding{M, I}"/>.
   /// </summary>
   /// <param name="model">
   ///   The model to monitor for property changes.
   /// </param>
   /// <param name="control">
   ///   The <see cref="TreeView"/> to update when the model property changes.
   /// </param>
   /// <param name="modelProperty">
   ///   The name of the model property to monitor for changes.
   /// </param>
   /// <param name="collectionMapper">
   ///   Function that maps the model items property to a collection of 
   ///   <see cref="TreeNode"/>s. This function is responsible for handling the
   ///   node hierarchy if more than one level deep.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="model"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="control"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="modelProperty"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="collectionMapper"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="modelProperty"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="model"/> does not implement a property named 
   ///   <paramref name="modelProperty"/>.
   /// </exception>
   public ToTreeViewNodesPropertyBinding(
      M model,
      TreeView control,
      String modelProperty,
      Func<IEnumerable<I>?, IEnumerable<TreeNode>> collectionMapper) : base(model, modelProperty)
   {
      ArgumentNullException.ThrowIfNull(model, nameof(model));
      ArgumentNullException.ThrowIfNull(control, nameof(control));
      ArgumentNullException.ThrowIfNullOrWhiteSpace(modelProperty, nameof(modelProperty));
      ArgumentNullException.ThrowIfNull(collectionMapper, nameof(collectionMapper));

      Control = control;
      _collectionMapper = collectionMapper;
   }

   /// <summary>
   ///   The bound control.
   /// </summary>
   public TreeView Control { get; private set; }

   protected override void ReleaseResources()
   {
      _collectionMapper = default!;
      Control = default!;

      base.ReleaseResources();
   }

   protected override void HandlePropertyChanged(PropertyChangedEventArgs e)
   {
      Control.BeginUpdate();
      Control.Nodes.Clear();

      var hierarchy = (IEnumerable<I>)_modelPropertyInfo.GetValue(Model)!;
      foreach (var item in _collectionMapper(hierarchy))
      {
         Control.Nodes.Add(item);
      }

      Control.EndUpdate();
   }
}
