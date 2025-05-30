using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class SharingIsCaring : Skill
{
    public override string Name => "Sharing is caring";

    public override string Description => "+10 score for every flower in the garden.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }

    public override void OnMature()
    {
        base.OnMature();
        UpdateSkill();
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        if (owner.hasMatured && plant != owner)
        {
            AddEffect(new(Effect.Flag.Score10, this, new() { plot }), owner.plot);
            GameManager.Instance.UpdateScore();
        }
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        Garden garden = Garden.Instance;
        for (int i = 0; i < garden.height; i++)
        {
            for (int j = 0; j < garden.width; j++)
            {
                Plot plot = garden.GetPlot(i, j);
                if (plot != owner.plot && plot.plant != null && plot.plant.hasMatured) AddEffect(new(Effect.Flag.Score10, this, new() { plot }), owner.plot);
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class LookingForTheSun : Skill
{
    public override string Name => "Looking for the sun";

    public override string Description => "+40 score if next to a Blue Sun.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }

    public override void OnMature()
    {
        UpdateSkill();
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        if (plant.Species.Equals("Blue Sun")) UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured || effect_table.Keys.Count > 0) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot n in neighbours)
        {
            if (n.plant != null)
            {
                if (n.plant.Species.Equals("Blue Sun") && n.plant.hasMatured)
                {
                    AddEffect(new(Effect.Flag.Score40, this, new() { n }), owner.plot);
                    break;
                }
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class LookingForTheMoon : Skill
{
    public override string Name => "Looking for the moon";

    public override string Description => "+40 score if next to a Yellow Moon.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }
    public override void OnMature()
    {
        UpdateSkill();
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        if (plant.Species.Equals("Yellow Moon")) UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured || effect_table.Keys.Count > 0) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot n in neighbours)
        {
            if (n.plant != null)
            {
                if (n.plant.Species.Equals("Yellow Moon") && n.plant.hasMatured)
                {
                    AddEffect(new(Effect.Flag.Score40, this, new() { n }), owner.plot);
                    break;
                }
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class HappyNeighbours : Skill
{
    public override string Name => "Happy Neighbours";

    public override string Description => "+10 score for each surrounding Primula.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }

    public override void OnMature()
    {
        UpdateSkill();
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        if (plant.Species.Equals("Primula")) UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot plot in neighbours)
        {
            if (plot.plant != null && plot.plant.hasMatured && plot.plant.Species.Equals("Primula"))
                AddEffect(new(Effect.Flag.Score10, this, new() { plot }), owner.plot);
        }
        GameManager.Instance.UpdateScore();
    }
}

public class QueenOfLove : Skill
{
    public override string Name => "Queen of Love";

    public override string Description => "+20 score for each Rose in the garden.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnMature()
    {
        base.OnMature();
        UpdateSkill();
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        if (plant.Species.Equals("Rose"))
        {
            AddEffect(new(Effect.Flag.Score20, this, new() { plot }), owner.plot);
            GameManager.Instance.UpdateScore();
        }
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        Garden garden = Garden.Instance;
        for (int i = 0; i < garden.height; i++)
        {
            for (int j = 0; j < garden.width; j++)
            {

                Plot plot = garden.GetPlot(i, j);
                if (plot.plant != null && plot.plant.hasMatured && plot.plant.Species.Equals("Rose"))
                    AddEffect(new(Effect.Flag.Score20, this, new() { plot }), owner.plot);
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class ToxicLove : Skill
{
    public override string Name => "Toxic Love";

    public override string Description => "-5 score for each White Rose in the garden.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }

    public override void OnMature()
    {
        base.OnMature();
        UpdateSkill();
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        if (owner.hasMatured && plant.Species.Equals("White Rose"))
        {
            AddEffect(new(Effect.Flag.ScoreM5, this, new() { plot }), owner.plot);
            GameManager.Instance.UpdateScore();
        }
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        Garden garden = Garden.Instance;
        for (int i = 0; i < garden.height; i++)
        {
            for (int j = 0; j < garden.width; j++)
            {
                Plot plot = garden.GetPlot(i, j);
                if (plot.plant != null && plot.plant.hasMatured && plot != owner.plot && plot.plant.Species.Equals("White Rose"))
                    AddEffect(new(Effect.Flag.ScoreM5, this, new() { plot }), owner.plot);
            }
        }
        GameManager.Instance.UpdateScore();
    }
}

public class Silence : Skill
{
    public override string Name => "Silence";

    public override string Description => "Negates effects of surrounding flowers.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        foreach (Plot plot in Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j))
            AddEffect(new(Effect.Flag.EffectCancelled, this, new()), plot);
    }
}

public class LeaderOfFlowers : Skill
{
    public override string Name => "Leader of Flowers";

    public override string Description => "+100 score if surrounded by 8 different flowers.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        int n_species = neighbours.Where(p => p.plant != null).Select(p => p.plant.Species).Distinct().Count();
        Debug.Log("N SPECIES");
        Debug.Log(n_species);
        if (n_species == 8)
        {
            AddEffect(new(Effect.Flag.Score100, this, new(neighbours)), owner.plot);
            GameManager.Instance.UpdateScore();
        }
    }
}

public class LonelyBloom : Skill
{
    public override string Name => "Lonely Bloom";

    public override string Description => "-10 score for every surrounding flower.";

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        Garden.OnPlantMatured += OnPlantMatured;
    }

    public override void OnRemoved()
    {
        base.OnRemoved();
        Garden.OnPlantMatured -= OnPlantMatured;
    }

    private void OnPlantMatured(Plot plot, Plant plant)
    {
        UpdateSkill();
    }

    private void UpdateSkill()
    {
        if (!owner.hasMatured) return;
        foreach (Effect effect in effect_table.Keys) RemoveEffect(effect);
        List<Plot> neighbours = Garden.Instance.GetNeighbours(owner.plot.i, owner.plot.j);
        foreach (Plot plot in neighbours)
        {
            if (!(plot.plant == null)) AddEffect(new(Effect.Flag.ScoreM10, this, new() { plot }), owner.plot);
        }
    }
}

public class StrongWill : Skill
{
    public override string Name => "Strong Will";

    public override string Description => "+1 score every day. Maximum +20.";

    private int count;

    public override void SetOwner(Plant plant)
    {
        base.SetOwner(plant);
        count = 0;
    }

    public override void OnDayEnd()
    {
        base.OnDayEnd();
        if (owner.hasMatured && count < 20) AddEffect(new(Effect.Flag.Score1, this, new()), owner.plot);
    }
}