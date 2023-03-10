using System;
namespace ZooManager
{
    /// <summary>
    /// The abstract class for Skull and Boulder
    /// Should not be instantiate
    /// </summary>
	public abstract class Landscape: GameObject
	{
		public Landscape(Zone zone):base(zone)
		{
		}
        /// <summary>
        /// Used to be called by Game.
        /// </summary>
        public override void Activate()
        {
            base.Activate();
        }
    }
}

