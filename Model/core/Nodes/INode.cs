﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L_system.Model.core.Nodes
{
    internal interface INode
    {
        public IOutputNode<Object>[] Outputs { get; protected set; }
    }