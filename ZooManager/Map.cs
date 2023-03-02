using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
namespace ZooManager
{
    public class Map
    {
        private List<List<Zone>> _animalZones = new List<List<Zone>>();
        // convert List<List<Zone>> to ReadOnlyCollection<ReadOnlyCollection<Zone>>, make the nested list readonly
        public ReadOnlyCollection<ReadOnlyCollection<Zone>> AnimalZones
               => _animalZones
                    .Select(zones => zones.AsReadOnly()) // convert each inner List<Zone> to ReadOnlyCollection<Zone>
                    .ToList()
                    .AsReadOnly(); // convrt List<ReadOnlyCollection<Zone>> to ReadOnlyCollection<ReadOnlyCollection<Zone>>

        /* Map
		 *   X 1 2 3 (width)
		 * Y 0
		 * 1
		 * 2
		 * 3
		 * (height)
		 * */
        public Map(int width, int height)
        {
            for (var y = 0; y < height; y++)
            {
                List<Zone> rowList = new List<Zone>();
                // Note one-line variation of for loop below!
                for (var x = 0; x < width; x++)
                {
                    var newZone = new Zone(new Point(x, y));
                    newZone.SetMap(this);
                    rowList.Add(newZone);
                }
                _animalZones.Add(rowList);
            }
        }

        public int Width => _animalZones[0].Count;

        public int Height => _animalZones.Count;

        /// <summary>
        /// use the postion of (0,0) zone as offset to calculate the real index
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        private Point GetIndex(Zone zone) => new Point(zone.position.x - _animalZones[0][0].position.x, zone.position.y - _animalZones[0][0].position.y);

        public void AddRow()
        {
            List<Zone> rowList = new List<Zone>();
            int width = Width;
            for (var x = 0; x < width; x++)
            {
                var newZone = new Zone(new Point(
                                _animalZones[Height - 1][x].position.x,
                                _animalZones[Height - 1][x].position.y + 1 // align the index to last row
                                ));
                newZone.SetMap(this);
                rowList.Add(newZone);
            }
            _animalZones.Add(rowList);
        }

        // Alow the position of Animals to be negative
        public void AddRowFront()
        {
            List<Zone> rowList = new List<Zone>();
            int width = this.Width;
            for (var x = 0; x < width; x++)
            {
                var newZone = new Zone(new Point(
                               _animalZones[0][x].position.x,
                               _animalZones[0][x].position.y - 1 // align the index to first row
                               ));

                newZone.SetMap(this);
                rowList.Add(newZone);
            }
            _animalZones.Insert(0, rowList);
        }

        public void AddColumn()
        {
            int height = this.Height;
            int width = this.Width;
            for (var y = 0; y < height; y++)
            {
                var newZone = new Zone(new Point(
                                _animalZones[y][width - 1].position.x + 1, // align the index to last column
                                _animalZones[y][width - 1].position.y
                                ));
                newZone.SetMap(this);
                _animalZones[y].Add(newZone);
            }
        }

        // Alow the position of Animals to be negative
        public void AddColumnFront()
        {
            int height = this.Height;
            for (var y = 0; y < height; y++)
            {
                var newZone = new Zone(new Point(
                                    _animalZones[y][0].position.x - 1, // align the index to last column
                                    _animalZones[y][0].position.y
                                    ));
                newZone.SetMap(this);
                _animalZones[y].Insert(0, newZone);
            }
        }
        /// <summary>
        /// swap the positions between two zones
        /// if one of the zone is not in the map, return error
        /// </summary>
        /// <param name="zoneA"></param>
        /// <param name="zoneB"></param>
        public void Swap(Zone zoneA, Zone zoneB)
        {
            Point zoneAPos = GetIndex(zoneA);
            Point zoneBPos = GetIndex(zoneB);
            if (_animalZones[zoneAPos.y][zoneAPos.x] == zoneA && _animalZones[zoneBPos.y][zoneBPos.x] == zoneB)
            {
                //use tuple to swap the position
                (_animalZones[zoneAPos.y][zoneAPos.x].position, _animalZones[zoneBPos.y][zoneBPos.x].position) = (_animalZones[zoneBPos.y][zoneBPos.x].position, _animalZones[zoneAPos.y][zoneAPos.x].position);
                //use tuple to swap the reference
                (_animalZones[zoneAPos.y][zoneAPos.x], _animalZones[zoneBPos.y][zoneBPos.x]) = (_animalZones[zoneBPos.y][zoneBPos.x], _animalZones[zoneAPos.y][zoneAPos.x]);
            }
            else
                Console.WriteLine("Zone A/B not found!");
        }
        /// <summary>
        /// swap the positions between zone and holding pen
        /// </summary>
        /// <param name="zoneIn"></param>
        /// <param name="ZoneOut"></param>
        public void SwapOut(ref Zone zoneIn, Zone zoneOut)
        {
            Point zoneOutPos = GetIndex(zoneOut);
            if (_animalZones[zoneOutPos.y][zoneOutPos.x] == zoneOut)
            {
                //use tuple to swap the position
                (zoneIn.position, _animalZones[zoneOutPos.y][zoneOutPos.x].position) = (_animalZones[zoneOutPos.y][zoneOutPos.x].position, zoneIn.position);
                //use tuple to swap the reference
                (zoneIn, _animalZones[zoneOutPos.y][zoneOutPos.x]) = (_animalZones[zoneOutPos.y][zoneOutPos.x], zoneIn);
            }
            else Console.WriteLine("Zone Out not found!");
        }
        /// <summary>
        /// Returns a reference to the zone at the specified location, or null if out of bounds
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Zone? GetZone(Point position)
        {
            Point index = new Point(position.x - _animalZones[0][0].position.x, position.y - _animalZones[0][0].position.y);
            if ((index.x >= 0 && index.x < Width) && (index.y >= 0 && index.y < Height))
            {
                return _animalZones[index.y][index.x];
            }

            return null;
        }
    }
}

