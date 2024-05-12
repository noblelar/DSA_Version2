using System;
namespace DAS_Coursework.models
{
	public class Delay
	{
        public Guid ID;
		public double Time;
		public Verticex StartStation;
		public Verticex EndStation;
		public string Direction;
		public string Line;

        public Delay(double delay, Verticex stationA, Verticex stationB,string direction, string line)
		{
            ID = Guid.NewGuid();
			Time = delay;
			StartStation = stationA;
			EndStation = stationB;
			Direction = direction;
			Line = line;
        }

		public void Display()
		{
			Console.WriteLine($"{Line} ({Direction}): {StartStation.Name} to {EndStation.Name} - {Time}mins delay");
		}
	}
}

