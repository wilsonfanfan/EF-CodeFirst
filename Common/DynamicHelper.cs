using System;
using System.Dynamic;
using System.Reflection;
using System.Xml.Linq;

namespace X.Common
{
    public class DynamicHelper
    {
        public static string ToXml(dynamic dynamicObject)
        {
            DynamicXElement xmlNode = dynamicObject;
            return xmlNode.XContent.ToString();
        }

        public static dynamic ToObject(string xml, dynamic dynamicResult)
        {
            XElement element = XElement.Parse(xml);
            dynamicResult = new DynamicXElement(element);
            return dynamicResult;
        }

        public static dynamic ToObject(string xml)
        {
            XElement element = XElement.Parse(xml);
            dynamic dynamicResult = new DynamicXElement(element);
            return dynamicResult;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class DynamicXElement : DynamicObject
    {
        public DynamicXElement(XElement node)
        {
            this.XContent = node;
        }

        public DynamicXElement()
        {
        }

        public DynamicXElement(String name)
        {
            this.XContent = new XElement(name);
        }

        public XElement XContent
        {
            get;
            private set;
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            XElement setNode = this.XContent.Element(binder.Name);
            if (setNode != null)
                setNode.SetValue(value);
            else
            {
                //creates an XElement without a value.
                if (value.GetType() != typeof(DynamicXElement))
                    this.XContent.Add(new XElement(binder.Name, value));
                else
                    this.XContent.Add(new XElement((value as DynamicXElement).XContent));
            }
            return true;
        }

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            XElement getNode = this.XContent.Element(binder.Name);
            if (getNode != null)
            {
                result = new DynamicXElement(getNode);
            }
            else
            {
                result = new DynamicXElement(binder.Name);
            }
            return true;
        }

        public override bool TryConvert(
    ConvertBinder binder, out object result)
        {
            if (binder.Type == typeof(String))
            {
                result = this.XContent.Value;
                return true;
            }
            else
            {
                result = null;
                return false;
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            Type xmlType = typeof(XElement);
            try
            {
                result = xmlType.InvokeMember(
                          binder.Name,
                          BindingFlags.InvokeMethod |
                          BindingFlags.Public |
                          BindingFlags.Instance,
                          null, this.XContent, args);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
