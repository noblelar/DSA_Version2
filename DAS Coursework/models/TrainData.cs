using System;
namespace DAS_Coursework.models
{
	public class TrainData
	{
		public string Line;
		public string Direction;
		public string StationA;
		public string StationB;
		public double UmimpededTime;
		public double AmPeakTime;
		public double InterPeakTime;

		public TrainData(string line, string direction, string stationA, string stationB,
			double amPeakTime, double interPeakTime, double umimpededTime)
		{
			Line = line;
			Direction = direction;
			StationA = stationA;
			StationB = stationB;
			AmPeakTime = amPeakTime;
			InterPeakTime = interPeakTime;
			UmimpededTime = umimpededTime;
		}
	}
}

