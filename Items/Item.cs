using System.Collections.Generic;
using System.Numerics;
using YARL.Actors;
using YARL.Actions;
using YARL.Topography;
using YARL.Drawing;

namespace YARL.Items
{
    public abstract class Pickable : IDrawable
    {
	public abstract char glyph { get; }
	public IDrawingBehaviour drawingBehaviour { get; protected set; }

	public Vector2 position { get; protected set; }
	public abstract string name { get; }
	public IPickBehaviour pickBehaviour { get; protected set; }	

	public void Pick(Player player)
	{
	    pickBehaviour.Pick(player, this);
	}

	public void SetBehaviour(IPickBehaviour b)
	{
	    pickBehaviour = b;
	}

	public char Draw()
	{
	    return drawingBehaviour.Draw(glyph);
	}

	public void SetDrawingBehaviour(IDrawingBehaviour b)
	{
	    drawingBehaviour = b;
	}
    }

    public abstract class Equipable
    {
	public bool equipped { get; protected set; }
	public abstract string name { get; }
	public Entity possessedBy { get; }
	public IEquipBehaviour behaviour { get; set; }
	public void Equip(Player player)
	{
	    behaviour.Equip(player, this);
	}
	public void UnEquip(Player player)
	{
	    behaviour.UnEquip(player, this);
	}
    }
}
