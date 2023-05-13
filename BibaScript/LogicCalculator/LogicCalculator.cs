using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCalculator
{
    public sealed class LogicCalculator
    {
        public bool Result { get; private set; }

        public void Calculate(string expression)
        {
            var table = new DataTable();
            table.Columns.Add(string.Empty, typeof(Boolean));
            table.Columns[0].Expression = expression;
            var row = table.NewRow();
            table.Rows.Add(row);
            Result = (Boolean)row[0];
        }
    }
}
