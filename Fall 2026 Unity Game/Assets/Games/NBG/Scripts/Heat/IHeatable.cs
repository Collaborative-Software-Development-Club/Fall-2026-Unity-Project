/// <summary>
/// Implemented by anything that carries heat. Lets the heat system (machines
/// that add heat, conveyors that dissipate it, etc.) operate on a common
/// abstraction instead of reaching into a concrete type.
/// </summary>
public interface IHeatable
{
    /// <summary>The heat state owned by this object.</summary>
    Heat Heat { get; }
}
