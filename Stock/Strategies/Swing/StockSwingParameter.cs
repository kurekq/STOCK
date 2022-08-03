using System;
using System.Collections.Generic;
using System.Text;

namespace Stock
{
    public class StockSwingParameter
    {
        public int Level;
        public decimal? ValueFrom;
        public decimal? ValueTo;
        protected decimal? _value;
        public StockSwingParameter(int Level)
        {
            this.Level = Level;
        }
        public virtual decimal? GetValue(Random r)
        {
            if (_value == null)
            {
                SetValue(r);
            }
            return _value;
        }

        protected virtual void SetValue(Random r)
        {
            if (ValueFrom == ValueTo)
            {
                _value = ValueFrom;
            }
            else if (ValueTo == null)
            {
                if (ValueFrom == null)
                {
                    _value = null;
                }
                else
                {
                    _value = ValueFrom;
                }
            }
            else
            {
                decimal valueFrom = ValueFrom == null ? 0 : (decimal)ValueFrom;
                _value = valueFrom + (ValueTo - valueFrom) * r.Next();
            }
        }
    }
}
