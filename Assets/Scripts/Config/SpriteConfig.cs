using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "SpriteConfig", menuName = "Config/SpriteConfig")]
public class SpriteConfig : AbstractConfig
{
    [SerializeField] private List<Sprite> spritesList;
    private readonly Dictionary<string, Sprite> spriteArgsDict = new();

    protected override void LoadConfigFile()
    {
        spriteArgsDict.Clear();
        foreach (var sprite in spritesList)
        {
            AssertUtil.IsNotNull(sprite);
            if (!spriteArgsDict.TryAdd(sprite.name, sprite))
            {
                Debug.LogError($"Unable to add sprite name : {sprite.name}");
            }
        }
    }

    public Sprite GetSprite(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            Debug.LogError($"Invalid sprite name: {name}");
            return null;
        }

        if (spriteArgsDict.TryGetValue(name, out var sprite))
        {
            return sprite;
        }

        Debug.LogError($"Sprite not found - name : {name}");
        return null;
    }

}
