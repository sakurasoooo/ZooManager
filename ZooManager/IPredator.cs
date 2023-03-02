using System;
namespace ZooManager
{
	public interface IPredator
	{
        public int Preys { get; set; }
        public int Hungry { get;  set; }

        public bool Hunt();
    }

}

