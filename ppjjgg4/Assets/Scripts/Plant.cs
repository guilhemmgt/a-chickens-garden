using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.XR.GoogleVr;
using UnityEngine;


[CreateAssetMenu (fileName = "Plant", menuName = "Garden/Plant")]
public class Plant : ScriptableObject {
    [field: SerializeField] public string Species { get; private set; }
    [TextArea (3, 5), SerializeField] private string description;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int score = 1;
    [SerializeField] private int growthTime = 3;
    [field: SerializeField] public Plot plot { get; private set; }
    [SerializeReference, SubclassPicker] ISkill skill;
    public int day;
    public bool hasMatured;

	[SerializeField] private Sprite seedSprite;
	[SerializeField] private Sprite shootSprite;

	public void OnPlanted (Plot plot) {
        day = 0;
        this.plot = plot;
        hasMatured = false;
        skill?.SetOwner (this);
    }

    public void OnMature () {
        hasMatured = true;
        plot.OnPlantMatured?.Invoke (this);
        Garden.OnPlantMatured?.Invoke (plot, this);
        skill?.OnMature ();
    }

    public void OnRemoved () {
        skill?.OnRemoved ();
    }

    public void EndDay () {
        day++;
        if (!hasMatured && day >= growthTime) OnMature ();
        skill?.OnDayEnd ();
    }

    //sprites[Mathf.Min(sprites.Count - 1, day)]
    public Sprite Sprite {
        get {
            if (day == 0) return seedSprite;
            if (day < growthTime) return shootSprite;
            return sprite;
        }
    }

    private void OnValidate()
    {
        Species = name;
    }

    public int GetScore()
    {
        return score;
    }


}
