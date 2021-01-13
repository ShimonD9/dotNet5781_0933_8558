using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DO;
namespace DO
{
    public class XmlTimeSpan
    {
        private TimeSpan m_internal = TimeSpan.Zero;

        public XmlTimeSpan()
            : this(TimeSpan.Zero)
        {
        }

        public XmlTimeSpan(TimeSpan input)
        {
            m_internal = input;
        }

        public static implicit operator TimeSpan(XmlTimeSpan input)
        {
            return (input != null) ? input.m_internal : TimeSpan.Zero;
        }

        // Alternative to the implicit operator TimeSpan(XmlTimeSpan input)
        public TimeSpan ToTimeSpan()
        {
            return m_internal;
        }

        public static implicit operator XmlTimeSpan(TimeSpan input)
        {
            return new XmlTimeSpan(input);
        }

        // Alternative to the implicit operator XmlTimeSpan(TimeSpan input)
        public void FromTimeSpan(TimeSpan input)
        {
            this.m_internal = input;
        }

        [XmlText]
        public string Value
        {
            get
            {
                return XmlConvert.ToString(m_internal);
            }
            set
            {
                m_internal = XmlConvert.ToTimeSpan(value);
            }
        }
    }
}
