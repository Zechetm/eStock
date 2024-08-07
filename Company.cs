using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eStock.Core
{
    internal class Company
    {
        public string name;
        public List<float> course;

        public Company(string _name, List<float> _course)
        {
            this.name = _name;
            this.course = _course;
        }
    }
}
