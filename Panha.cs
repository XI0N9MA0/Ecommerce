using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommercetestttt
{
    class Panha
    {
        int id;
        public int Id { get { return id;} }
        string name;
        public string Name { get { return name;} }

        public int Pid { get { return id;} set { id = value;} }
        public string PidStr { get {  return name; } set { name = value; } }
    }
}
