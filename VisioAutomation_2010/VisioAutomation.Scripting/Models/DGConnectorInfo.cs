using SXL = System.Xml.Linq;

namespace VisioAutomation.Scripting.Models
{
    internal class DGConnectorInfo
    {
        public string ID;
        public string Label;
        public string From;
        public string To;
        public SXL.XElement Element;

        public static DGConnectorInfo FromXml(Client client, SXL.XElement shape_el)
        {
            var info = new DGConnectorInfo();
            info.ID = shape_el.Attribute("id").Value;
            client.WriteVerbose("Reading connector id={0}", info.ID);

            info.Label = shape_el.Attribute("label").Value;
            info.From = shape_el.Attribute("from").Value;
            info.To = shape_el.Attribute("to").Value;

            info.Element = shape_el;
            return info;
        }
    }
}