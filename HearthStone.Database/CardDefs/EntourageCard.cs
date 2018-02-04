#region

using System.Xml.Serialization;

#endregion

namespace HearthDb.CardDefs
{
	public class EntourageCard
	{
		[XmlAttribute("cardID")]
		public string CardId { get; set; }
	}
}