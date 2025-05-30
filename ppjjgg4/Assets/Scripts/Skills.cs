using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class SharingIsCaring : Skill
{
    public override string Name => "Sharing is caring";

    public override string Description => "+10 score for every flower in the garden";

    public override void OnMature()
    {
        Garden garden = Garden.Instance;
        for (int i = 0; i < garden.height; i++)
        {
            for (int j = 0; j < garden.width; j++)
            {
                if (i != owner.plot.i && j != owner.plot.j) AddEffect(new(Effect.Flag.Score10, this, new()), garden.GetPlot(i, j));
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class LookingForTheSun : Skill
{
    public override string Name => "Looking for the sun";

    public override string Description => "+40 score if next to a Blue Sun";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantEnter += (_, _) => UpdateSkill();
    }

    public override void OnMature()
    {
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot n in neighbours)
        {
            if (n.plant != null)
            {
                Debug.Log("-----");
                Debug.Log(n.plant.Species);
                Debug.Log("Blue Sun");
                if (n.plant.Species.Equals("Blue Sun")) AddEffect(new(Effect.Flag.Score40, this, new() { n }), owner.plot);
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class LookingForTheMoon : Skill
{
    public override string Name => "Looking for the moon";

    public override string Description => "+40 score if next to a Yellow Moon";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantEnter += (_, _) => UpdateSkill();
    }
    public override void OnMature()
    {
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot n in neighbours)
        {
            if (n.plant != null)
            {
                Debug.Log("-----");
                Debug.Log(n.plant.Species);
                Debug.Log("Yellow Moon");
                if (n.plant.Species.Equals("Yellow Moon")) AddEffect(new(Effect.Flag.Score40, this, new() { n }), owner.plot);
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class HappyNeighbours : Skill
{
    public override string Name => "Happy Neighbours";

    public override string Description => "+10 score for each surrounding Primula";

    public override void SetOwner(Plant plant)
    {
        Debug.Log("SET OWNER");
        Debug.Log(plant);
        Debug.Log(plant.plot);
        base.SetOwner(plant);
        Garden.OnPlantEnter += (_, _) => UpdateSkill();
    }

    public override void OnMature()
    {
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        Debug.Log("UPDATE SKILL");
        Debug.Log(owner);
        Debug.Log(owner.plot);
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot plot in neighbours)
        {
            if (plot.plant != null && plot.plant.Species.Equals("Primula")) AddEffect(new(Effect.Flag.Score10, this, new() { plot }), owner.plot);
        }
        GameManager.Instance.UpdateScore();
    }
}