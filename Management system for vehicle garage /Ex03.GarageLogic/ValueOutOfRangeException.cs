using System;
using System.Collections.Generic;
using System.Text;

namespace Ex03.GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_minValue;
        private float m_maxValue;

        internal ValueOutOfRangeException(float i_MinValue, float i_MaxValue)
            : base(string.Format("Invalid value entered. The minimum is: {0}, and the maximum value is {1}. You reached out of the limit.", i_MinValue, i_MaxValue))
        {
            m_minValue = i_MinValue;
            m_maxValue = i_MaxValue;
        }

        public float MinValue
        {
            get { return m_minValue; }
        }

        public float MaxValue
        {
            get { return m_maxValue; }
        }
    }
}

