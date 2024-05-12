using System;
namespace DAS_Coursework.models
{
	public class Verticex
	{
        private Guid id;
        private string name;


        public Verticex(string name)
		{
            id = Guid.NewGuid();
            this.name = name;
        }

        public string Name { get { return name; } }
    }
}

