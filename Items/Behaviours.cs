using YARL.Actors;

namespace YARL.Items
{
    public interface IPickBehaviour
    {
	public void Pick(Player player, Pickable self);
    }

    public interface IEquipBehaviour
    {
	public void Equip(Player player, Equipable self);
	public void UnEquip(Player player, Equipable self);
    }
}

