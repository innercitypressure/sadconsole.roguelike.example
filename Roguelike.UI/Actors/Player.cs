using Roguelike.UI.Entities;
using Roguelike.UI.Infrastructure;
using SadRogue.Primitives;

namespace Roguelike.UI.Actors;

public class Player : Actor
{
    public Player(string name, Color foreground, Color background, Point position,
         int layer = (int)MapLayer.PLAYER) :
        base(name, foreground, background, '@', position, layer)
    {
        // Anatomy.Limbs = LimbTemplate.BasicHumanoidBody(this);
        Weight = GoRogue.Random.GlobalRandom.DefaultRNG.Next(50, 95);
        Size = GoRogue.Random.GlobalRandom.DefaultRNG.Next(155, 200);
        // Anatomy.Update(this);
        // Anatomy.Lifespan = GoRogue.Random.GlobalRandom.DefaultRNG.Next(75, 125);
    }

    public static Player TestPlayer()
    {
        Player player = new Player("Magus", Color.White, Color.Black, Point.None);
        /*player.Stats.SetAttributes(
            viewRadius: 7,
            health: 10,
            baseHpRegen: 0.1f,
            bodyStat: 1,
            mindStat: 1,
            soulStat: 1,
            baseAttack: 10,
            attackChance: 40,
            protection: 5,
            defenseChance: 20,
            speed: 1.0f,
            _baseManaRegen: 0.1f,
            personalMana: 12
            );
        player.Stats.Precision = 3;

        player.Anatomy.Limbs = LimbTemplate.BasicHumanoidBody(player);

        player.Magic.ShapingSkill = 9;
        */

        

        return player;
    }

    public static Player ReturnPlayerFromActor(Actor actor)
    {
        Player player = new Player(actor.Name,
            actor.Appearance.Foreground,
            actor.Appearance.Background,
            actor.Position)
        {
            // Inventory = actor.Inventory,
            // Magic = actor.Magic,
            // Stats = actor.Stats,
            Size = actor.Size,
            Weight = actor.Weight,
            // Abilities = actor.Abilities,
            // Anatomy = actor.Anatomy,
            // Equipment = actor.Equipment,
            // Material = actor.Material,
            // XP = actor.XP
        };

        return player;
    }
}