using System;
namespace ZooManager
{
	public interface IPrey
	{
        public int Predators { get; set; }
        public bool Flee();
    }
}

