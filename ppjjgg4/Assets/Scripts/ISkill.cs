using System.Collections.Generic;
using UnityEngine;

public interface ISkill
{
    string Name { get; }
    string Description { get; }
    void OnDayEnd() { }
    void OnMature() { }
    void OnRemoved() { }
    void SetOwner(Plant plant);
    Plant GetOwner();
}

public abstract class Skill : ISkill
{
    public abstract string Name { get; }

    public abstract string Description { get; }

    protected Plant owner;
    protected Dictionary<Effect, Plot> effect_table;

    public virtual void OnDayEnd() { }
    public virtual void OnMature() { }
    public virtual void OnRemoved()
    {
        foreach (KeyValuePair<Effect, Plot> kvp in effect_table)
        {
            kvp.Value.effects.Remove(kvp.Key);
        }
        GameManager.Instance.UpdateScore();
    }

    public virtual void SetOwner(Plant plant)
    {
        owner = plant;
        effect_table = new();
    }

    public Plant GetOwner() => owner;

    protected void AddEffect(Effect effect, Plot plot)
    {
        effect_table.Add(effect, plot);
        plot.effects.Add(effect);
        GameManager.Instance.UpdateScore();
    }

    public void RemoveEffect(Effect effect)
    {
        effect_table[effect].effects.Remove(effect);
        GameManager.Instance.UpdateScore();
    }
}
