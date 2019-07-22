﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZorkLike
{
    public class Identifiable
    {
        private string[] ids;
        public Identifiable(string[] ids)
        {
            this.ids = ids;
        }

        public bool HasID(string id)
        {
            foreach (var i in ids)
            {
                if (id == i) return true;
            }
            return false;
        }
    }
}
