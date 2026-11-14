/// <summary>
/// Reusable behavior that applies a fixed amount of heat to any <see cref="IHeatable"/>.
/// Heating is composed onto a machine (or anything else) rather than baked into it,
/// so only things that should add heat carry one of these.
/// </summary>
public class HeatApplicator
{
    private readonly float _amountPerApply;

    public HeatApplicator(float amountPerApply)
    {
        _amountPerApply = amountPerApply;
    }

    public float AmountPerApply => _amountPerApply;

    /// <summary>Adds this applicator's heat to the target, if it can hold heat.</summary>
    public void ApplyTo(IHeatable target)
    {
        if (target == null || _amountPerApply <= 0f) return;
        target.Heat.Add(_amountPerApply);
    }

    /// <summary>Convenience: applies heat only when the object is heatable.</summary>
    public void ApplyTo(object target)
    {
        if (target is IHeatable heatable)
            ApplyTo(heatable);
    }
}
