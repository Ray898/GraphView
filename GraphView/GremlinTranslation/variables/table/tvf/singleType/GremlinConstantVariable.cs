﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphView
{
    internal class GremlinConstantVariable: GremlinScalarTableVariable
    {
        public object ConstantValue { get; set; }

        public GremlinConstantVariable(object value)
        {
            ConstantValue = value;
        }

        public override WTableReference ToTableReference()
        {
            List<WScalarExpression> parameters = new List<WScalarExpression>();
            if (ConstantValue is List<object>)
            {
                foreach (var value in ConstantValue as List<object>)
                {
                    parameters.Add(SqlUtil.GetValueExpr(value));
                }
            }
            else if (ConstantValue is List<string>)
            {
                foreach (var value in ConstantValue as List<string>)
                {
                    parameters.Add(SqlUtil.GetValueExpr(value));
                }
            }
            else
            {
                parameters.Add(SqlUtil.GetValueExpr(ConstantValue));
            }
            var tableRef = SqlUtil.GetFunctionTableReference(GremlinKeyword.func.Constant, parameters, GetVariableName());
            return SqlUtil.GetCrossApplyTableReference(tableRef);
        }
    }
}
