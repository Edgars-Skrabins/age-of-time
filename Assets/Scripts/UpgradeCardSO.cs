using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade Card", menuName = "Upgrade Card")]
public class UpgradeCardSO : ScriptableObject
{
    public string m_name;
    public string m_description;
    public Sprite m_sprite;
    public int m_cost;
}