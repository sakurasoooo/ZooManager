using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace ZooManager
{
    
    public static class Game
    {

        private const int maxCellsX = 10;
        private const int maxCellsY = 10;


        static public Map map;
        static public Zone holdingPen;

        static public void SetUpGame()
        {
            map = new Map(4, 4);
            holdingPen = new Zone(new Point(999, 999));
            holdingPen.SetMap(map);
        }

        static public void AddZones(Direction d)
        {
            if (d == Direction.down || d == Direction.up)
            {
                if (map.Height >= maxCellsY) return; // hit maximum height!

                if (d == Direction.down) map.AddRow();
                if (d == Direction.up) map.AddRowFront();
            }
            else // must be left or right...
            {
                if (map.Width >= maxCellsX) return; // hit maximum width!

                if (d == Direction.left) map.AddColumnFront();
                if (d == Direction.right) map.AddColumn();

            }
        }

        static public void ZoneClick(Zone clickedZone)
        {
            Console.Write("Got animal ");
            Console.WriteLine(clickedZone.emoji == "" ? "none" : clickedZone.emoji);
            Console.Write("Held animal is ");
            Console.WriteLine(holdingPen.emoji == "" ? "none" : holdingPen.emoji);
            if (clickedZone.occupant != null) clickedZone.occupant.ReportLocation();
            if (holdingPen.occupant == null && clickedZone.occupant != null)
            {
                // take animal from zone to holding pen
                Console.WriteLine("Taking " + clickedZone.emoji);
                map.SwapOut(ref holdingPen, clickedZone);
                //ActivateAnimals();
            }
            else if (holdingPen.occupant != null && clickedZone.occupant == null)
            {
                // put animal in zone from holding pen
                Console.WriteLine("Placing " + holdingPen.emoji);
                map.SwapOut(ref holdingPen, clickedZone);
                Console.WriteLine("Empty spot now holds: " + clickedZone.emoji);
                ActivateAnimals();
            }
            else if (holdingPen.occupant != null && clickedZone.occupant != null)
            {
                Console.WriteLine("Could not place animal.");
                // Don't activate animals since user didn't get to do anything
            }
        }

        static public void AddAnimalToHolding(string animalType)
        {
            if (holdingPen.occupant != null) return;
            if (animalType == "cat") { var animal = new Cat(holdingPen); }
            if (animalType == "mouse") { var animal = new Mouse(holdingPen); }
            if (animalType == "raptor") { var animal = new Raptor(holdingPen); }
            if (animalType == "chick") { var animal = new Chick(holdingPen); }
            Console.WriteLine($"Holding pen occupant at {holdingPen.occupant.zone.position.x},{holdingPen.occupant.zone.position.y}");
            //ActivateAnimals();
        }

        static public void ActivateAnimals()
        {
            // The game has been modified to move base on swaping zone, while not swaping occupant
            // Thus create a new reference for all zone and iterate them, there will be no repeated traversal.
            List<Zone> map1D = new List<Zone>();
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++)
                {
                    var zone = map.AnimalZones[y][x];
                    map1D.Add(zone);
                }
                var zones = map.AnimalZones;
            }
            Random rng = new Random();
            for (var r = 1; r < 11; r++) // reaction times from 1 to 10
            {
                map1D.OrderBy(a => rng.Next()).ToList();
                foreach (var zone in map1D)
                {
                    if (zone.occupant != null && zone.occupant.reactionTime == r)
                    {
                        zone.occupant.Activate();
                    }
                }

            }
        }
    }
}