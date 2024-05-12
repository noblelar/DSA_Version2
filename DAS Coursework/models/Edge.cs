using System;
using QuikGraph;
namespace DAS_Coursework.models
{
	public class WeightedEdge<TVertex>: Edge<TVertex>
    {
        public Guid id;
        public string line;
        private double weightField;
        public double delay = 0;
        public Boolean isClosed = false;
        public string direction;


        public double weight {
            get { return weightField + delay; }
            private set {}
        }

        public WeightedEdge(string line, TVertex fromVerticex, TVertex toVerticex, double weight, string direction):
            base(fromVerticex, toVerticex)
		{
            id = Guid.NewGuid();
            this.line = line;
            this.weightField = weight;
            this.direction = direction;
        }

        public void AddDelay(double delayTime)
        {
            delay += delayTime;
        }

        public void RemoveDelay()
        {
            delay = 0;
        }

        public void Close()
        {
            isClosed = true;
        }

        public void Open()
        {
            isClosed = false;
        }
	}
}

