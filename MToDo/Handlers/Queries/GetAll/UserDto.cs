﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MToDo.Handlers.Queries.GetAll
{
    public class UserDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string State { get; set; }
        public DateTime DueDate { get; set; }
        public Boolean OverDue { get; set; }
    }
}
