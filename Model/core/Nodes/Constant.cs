﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    public class NodeConstant : Node
    {
        public object Value { get; set; }

        public NodeConstant(object value)
        {
            Value = value;
            Outputs = [new OutputOfNode(() => Value)];
            NameOfOutputs = ["Значение"];
            NameOfNode = "Константа";
            Inputs = [];
        }
    }
}
