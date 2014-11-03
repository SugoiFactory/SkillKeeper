using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillKeeper
{
    class ImportPlayer
    {
        private String id, name, skLink;

        public String ID
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String SKLink
        {
            get { return skLink; }
            set { skLink = value; }
        }
    }
}
