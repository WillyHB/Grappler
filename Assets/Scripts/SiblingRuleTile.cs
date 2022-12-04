using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SiblingRuleTile : RuleTile
{

    public enum SiblingGroup
    {
        Connector1,
        Connector2,
        Connector3,
        Connector4
    }
    public SiblingGroup siblingGroup;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRuleOutput.Neighbor.This:
                {
                    return other is SiblingRuleTile
                        && (other as SiblingRuleTile).siblingGroup == siblingGroup;
                }
            case TilingRuleOutput.Neighbor.NotThis:
                {
                    return !(other is SiblingRuleTile
                        && (other as SiblingRuleTile).siblingGroup == siblingGroup);
                }
        }

        return base.RuleMatch(neighbor, other);
    }
}