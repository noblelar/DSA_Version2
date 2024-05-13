using ExcelDataReader;
using DAS_Coursework.models;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DAS_Coursework.data
{
    public static class GetData
    {
        // private static string FilePath = @"/Users/otcheredev/Projects/DAS Coursework/DAS Coursework/data/isst.xlsx";
        private static string FilePath = @"C:\Users\larte\source\repos\Data-structure-Coursework\DAS Coursework\data\isst.xlsx";
        private static IExcelDataReader GetExcelReader()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            return ExcelReaderFactory.CreateReader(stream);
        }

        private static List<string> GetColumnData(int columnIndex)
        {
            var data = new List<string>();
            using (var reader = GetExcelReader())
            {
                reader.Read(); // Skip the first row (header)
                reader.Read(); // Skip the second row
                while (reader.Read()) // Read each row of data
                {
                    var value = reader.GetString(columnIndex);
                    if (!string.IsNullOrEmpty(value))
                    {
                        data.Add(value);
                    }
                }
            }
            return data;
        }

        public static List<string> GetStationA() => GetColumnData(2);

        public static List<string> GetStationB() => GetColumnData(3);

        public static List<string> GetLines() => GetColumnData(0);

        public static List<string> GetUniqueLines() => GetLines().Distinct().ToList();

        public static List<double> GetEdgeTime()
        {
            var data = new List<double>();
            using (var reader = GetExcelReader())
            {
                reader.Read(); // Skip the first row (header)
                reader.Read(); // Skip the second row
                while (reader.Read()) // Read each row of data
                {
                    if (!reader.IsDBNull(5))
                    {
                        data.Add(reader.GetDouble(5));
                    }
                    else
                    {
                        data.Add(0);
                    }
                }
            }
            return data;
        }

        public static List<TrainData> GetAllTrainData()
        {
            var data = new List<TrainData>();
            using (var reader = GetExcelReader())
            {
                reader.Read(); // Skip the first row (header)
                reader.Read(); // Skip the second row
                while (reader.Read()) // Read each row of data
                {
                    string line = GetExcelStringData(reader, 0);
                    string direction = GetExcelStringData(reader,1);
                    string stationA = GetExcelStringData(reader,2);
                    string stationB = GetExcelStringData(reader,3);
                    double amPeakTime = GetExcelDoubleData(reader,6);
                    double interPeakTime = GetExcelDoubleData(reader, 7);
                    double umimpededTime= GetExcelDoubleData(reader, 5);

                   

                    data.Add(new TrainData(line, direction, stationA, stationB, amPeakTime, interPeakTime, umimpededTime));
                }
            }
            return data;
        }

        private static string GetExcelStringData(IExcelDataReader reader, int columnIndex)
        {
            var value = reader.GetString(columnIndex);
            if (!string.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                return "";
            }
        }

        private static double GetExcelDoubleData(IExcelDataReader reader, int columnIndex)
        {
            if (!reader.IsDBNull(5) && reader.GetDouble(columnIndex) != null)
            {
                return reader.GetDouble(columnIndex);
            }
            else
            {
                return 0;
            }
        }
    }
}
