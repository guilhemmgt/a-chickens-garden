using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class SharingIsCaring : ISkill
{
    public string Name => "Pheonix";

    public string Description => "Blablabla";

    private Plant owner;

    private Dictionary<Effect, Plot> effect_table;

    public void OnMature()
    {
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot plot in neighbours)
        {
            Effect effect = new(Effect.Flag.Score1, this, new());
            effect_table.Add(effect, plot);
            plot.effects.Add(effect);
        }
        GameManager.Instance.UpdateScore();
    }

    public void OnRemoved()
    {
        foreach (KeyValuePair<Effect, Plot> kvp in effect_table)
        {
            kvp.Value.effects.Remove(kvp.Key);
        }
        GameManager.Instance.UpdateScore();
    }



    public void SetOwner(Plant plant)
    {
        this.owner = plant;
        effect_table = new();
    }
}
