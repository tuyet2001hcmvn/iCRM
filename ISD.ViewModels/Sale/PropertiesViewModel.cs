using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISD.ViewModels
{
	public class PropertiesViewModel
	{
		public Guid PropertiesId { get; set; }
		public string Subject { get; set; }
		public string Description { get; set; }
		public string X { get; set; }
		public string Y { get; set; }
		public string Image { get; set; }
	}
}
