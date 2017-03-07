﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView
{
    internal class GremlinMapVariable : GremlinTableVariable
    {
        public GremlinToSqlContext MapContext { get; set; }

        public GremlinMapVariable(GremlinToSqlContext mapContext, GremlinVariableType variableType)
            : base(variableType)
        {
            MapContext = mapContext;
        }

        internal override void Populate(string property)
        {
            if (ProjectedProperties.Contains(property)) return;
            base.Populate(property);

            MapContext.Populate(property);
        }

        internal override bool ContainsLabel(string label)
        {
            if (base.ContainsLabel(label)) return true;
            return false;
        }

        internal override GremlinVariableType GetUnfoldVariableType()
        {
            return MapContext.PivotVariable.GetUnfoldVariableType();
        }

        public override WTableReference ToTableReference()
        {
            List<WScalarExpression> parameters = new List<WScalarExpression>();
            parameters.Add(SqlUtil.GetScalarSubquery(MapContext.ToSelectQueryBlock(ProjectedProperties)));
            var tableRef = SqlUtil.GetFunctionTableReference(GremlinKeyword.func.Map, parameters, GetVariableName());

            return SqlUtil.GetCrossApplyTableReference(tableRef);
        }
    }
}
