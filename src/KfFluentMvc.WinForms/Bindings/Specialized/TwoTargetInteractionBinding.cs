namespace KfFluentMvc.WinForms.Bindings.Specialized;

/// <summary>
///   Defines a binding that invokes an interaction between two target objects 
///   (and possibly the model) in response to an event on the primary control. A 
///   typical example would be a button click that invokes an action on the
///   secondary control (a "Collapse All" button that collapses a TreeView 
///   control).
/// </summary>
/// <typeparam name="M">
///   The bound model type.
/// </typeparam>
/// <typeparam name="T1">
///   The primary target type.
/// </typeparam>
/// <typeparam name="T2">
///   The secondary target type.
/// </typeparam>
public class TwoTargetInteractionBinding<M, T1, T2> : MvcBindingBase<M>
   where M : IMvcModel
{
   protected Action<M, T1, T2> _action;
   protected EventInfo _targetEventInfo;
   protected Delegate _handlerDelegate;

   /// <summary>
   ///   Initialize a new <see cref="TwoTargetInteractionBinding{M, T1, T2}"/>.
   /// </summary>
   /// <param name="model">
   ///   The bound model.
   /// </param>
   /// <param name="primaryTarget">
   ///   The target object to monitor for event notifications.
   /// </param>
   /// <param name="targetEvent">
   ///   The name of the primary target event to monitor.
   /// </param>
   /// <param name="secondaryTarget">
   ///   The secondary target in the interaction.
   /// </param>
   /// <param name="action">
   ///   Action to perform when the primary control event occurs.
   /// </param>
   /// <exception cref="ArgumentNullException">
   ///   <paramref name="primaryTarget"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="targetEvent"/> is <see langword="null"/>.
   ///   - or -
   ///   <paramref name="secondaryTarget"/> is <see langword="null"/>.
   /// </exception>
   /// <exception cref="ArgumentException">
   ///   <paramref name="targetEvent"/> is <see cref="String.Empty"/> or all
   ///   whitespace characters.
   /// </exception>
   /// <exception cref="InvalidOperationException">
   ///   <paramref name="primaryTarget"/> does not implement an event named 
   ///   <paramref name="targetEvent"/>.
   /// </exception>
   public TwoTargetInteractionBinding(
      M model,
      T1 primaryTarget,
      String targetEvent,
      T2 secondaryTarget,
      Action<M, T1, T2> action) : base(model)
   {
      ArgumentNullException.ThrowIfNull(primaryTarget, nameof(primaryTarget));
      ArgumentException.ThrowIfNullOrWhiteSpace(targetEvent, nameof(targetEvent));
      ArgumentNullException.ThrowIfNull(secondaryTarget, nameof(secondaryTarget));
      ArgumentNullException.ThrowIfNull(action, nameof(action));

      PrimaryTarget = primaryTarget;
      SecondaryTarget = secondaryTarget;
      _action = action;

      // see https://stackoverflow.com/questions/45779/c-sharp-dynamic-event-subscription
      _targetEventInfo = PrimaryTarget.GetEventInfo(targetEvent);
      var handlerMethodInfo = this.GetMethodInfo(nameof(Control_Event));
      _handlerDelegate = Delegate.CreateDelegate(
         _targetEventInfo.EventHandlerType!,
         this,
         handlerMethodInfo);
      _targetEventInfo.AddEventHandler(PrimaryTarget, _handlerDelegate);
   }

   /// <summary>
   ///   The bound control that triggers the interaction.
   /// </summary>
   public T1 PrimaryTarget { get; private set; }

   /// <summary>
   ///   The bound control that is the target of the interaction.
   /// </summary>
   public T2 SecondaryTarget { get; private set; }

   public void Control_Event(Object? sender, EventArgs e)
      => _action(Model, PrimaryTarget, SecondaryTarget);

   protected override void ReleaseResources()
   {
      _targetEventInfo.RemoveEventHandler(PrimaryTarget, _handlerDelegate);

      _targetEventInfo = default!;
      _handlerDelegate = default!;
      PrimaryTarget = default!;
      SecondaryTarget = default!;

      base.ReleaseResources();
   }
}
