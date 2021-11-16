using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwapper : MonoBehaviour
{
    public Image backgroundSpriteRef;

    public Image overlapSpriteRef;

    public Sprite dashIMG;

    public Sprite rootIMG;

    public Sprite shroomIMG;

    public Sprite walljumpIMG;
    
    public Sprite noneIMG;
    
    // Start is called before the first frame update
    public void SetAbilityGUISprite(Ability set)
    {
        Sprite selected = noneIMG;
        switch (set)
        {
            case Ability.Dash:
                selected = dashIMG;
                break;
            case Ability.Roots:
                selected = rootIMG;
                break;
            case Ability.Shroom:
                selected = shroomIMG;
                break;
            case Ability.WallJump:
                selected = walljumpIMG;
                break;
        }

        backgroundSpriteRef.sprite = selected;
        overlapSpriteRef.sprite = selected;
        Debug.Log("changed sprite");
    }
}
