#region

using System.Xml.Serialization;

#endregion

namespace HearthStone.Database.CardDefs
{
	public class PlayRequirement
	{
		[XmlAttribute("param")]
		public string Param { get; set; }

		[XmlAttribute("reqID")]
		public string ReqId { get; set; }
	}
}