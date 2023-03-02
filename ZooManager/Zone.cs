using System;
namespace ZooManager
{
    public class Zone
    {
        public Map? Map { get; private set; }

        public GameObject? occupant
        {
            get;
            set;

        }

        public Point position { get; set; }

        public string emoji
        {
            get
            {
                if (occupant == null) return "";
                return occupant.emoji;
            }
        }

        public string rtLabel
        {
            get
            {
                if (occupant == null) return "";
                return occupant.reactionTime.ToString();
            }
        }

        public string turnLabel
        {
            get
            {
                if (occupant == null) return "";
                return occupant.turnsActivated.ToString();
            }
        }

        public Zone(Point position, Animal animal)
        {
            this.position = position;

            occupant = animal;
        }

        public Zone(Point position)
        {
            this.position = position;

            occupant = null;
        }

        public void SetMap(Map map) => this.Map = map;


        /// <summary>
        /// calculate the Manhattan distance between the destination and self
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public int Distance(Zone destination)
            => Math.Abs(position.x - destination.position.x) + Math.Abs(position.y - destination.position.y);


        /// <summary>
        /// calculate the Manhattan distance between the destination and start point
        /// </summary>
        /// <param name="start"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        static public int Distance(Zone start, Zone destination)
            => Math.Abs(start.position.x - destination.position.x) + Math.Abs(start.position.y - destination.position.y);


        /// <summary>
        /// A wrapper of Map.Swap
        /// swap self with the destination
        /// use for gameobejct movement
        /// </summary>
        /// <param name="destination"></param>
        public void MoveTo(Zone destination)
        {
            Console.WriteLine($"{occupant.name} {position.x},{position.y} is moving to {destination.position.x},{destination.position.y}");
            Map?.Swap(this, destination);
        }

        public void MoveTo(Point position)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the adjacent zone in the specified direction of this zone
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public Zone? FindZone(Direction d)
        {
            switch (d)
            {
                case Direction.up:
                    return Map?.GetZone(new Point(position.x, position.y - 1));
                case Direction.down:
                    return Map?.GetZone(new Point(position.x, position.y + 1));
                case Direction.left:
                    return Map?.GetZone(new Point(position.x - 1, position.y));
                case Direction.right:
                    return Map?.GetZone(new Point(position.x + 1, position.y));
            }
            return null;
        }
    }
}
