﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.42
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Xml.Serialization;

namespace Northwind.Entities
{
	[Serializable]
	[XmlInclude(typeof (CustomerCustomerDemo))]
	[SoapInclude(typeof (CustomerCustomerDemo))]
	public class AbstractCustomerDemographic
	{
		public virtual string CustomerTypeID { get; set; }

		public virtual string CustomerDesc { get; set; }

		public virtual IList CustomerCustomerDemos { get; set; }
	}

	[Serializable]
	public class CustomerDemographic : AbstractCustomerDemographic
	{
	}
}