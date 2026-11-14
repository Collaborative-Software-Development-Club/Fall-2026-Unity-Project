// Applies a flat amount of heat to something. Composed onto machines that heat instead of baked in
public class HeatApplicator
{
    private readonly float _amountPerApply;

    public HeatApplicator(float amountPerApply)
    {
        _amountPerApply = amountPerApply;
    }

    public float AmountPerApply => _amountPerApply;

    /// <summary>
    /// Adds this applicator's heat to the target if there's any to add
    /// </summary>
    /// <param name="target">The thing to heat up</param>
    public void ApplyTo(IHeatable target)
    {
        if (target == null || _amountPerApply <= 0f) return;
        target.Heat.Add(_amountPerApply);
    }

    // Only heats the target when it can actually hold heat
    public void ApplyTo(object target)
    {
        if (target is IHeatable heatable)
            ApplyTo(heatable);
    }
}
