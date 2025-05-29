using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class SharingIsCaring : Skill
{
    public override string Name => "SharingIsCaring";

    public override string Description => "Blablabla";

    public override void OnMature()
    {
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot plot in neighbours)
        {
            AddEffect(new(Effect.Flag.Score1, this, new()), plot);
        }
        GameManager.Instance.UpdateScore();
    }
}

public class HandInHand : Skill
{
    public override string Name => "Hand in hand";

    public override string Description => "Blablabla";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantEnter += (_,_) => UpdateEffects();
        Garden.OnPlantExit += (_, _) => UpdateEffects();
    }

    public override void OnMature()
    {
        UpdateEffects();
    }


    public void UpdateEffects()
    {
        foreach (Effect effect in effect_table.Keys)
        {
            RemoveEffect(effect);
        }
        Garden garden = Garden.Instance;
        for (int i = 0; i < garden.height; i++)
        {
            for (int j = 0; j < garden.width; j++)
            {
                if (garden.GetNeighbours(i, j).Where((Plot p) => p.plant != null).Count() >= 1)
                {
                    AddEffect(new(Effect.Flag.Score1, this, new()), garden.GetPlot(i, j));
                }
            }
        }
    }
}