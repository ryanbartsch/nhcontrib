using System;
using System.Collections.Generic;
using NHibernate.Validator.Cfg.MappingSchema;
using NHibernate.Validator.Exceptions;
using NHibernate.Validator.Mappings;
using NHibernate.Validator.Tests.Base;
using NUnit.Framework;
using NHibernate.Validator.Cfg;
using System.Reflection;

namespace NHibernate.Validator.Tests.Configuration
{
	[TestFixture]
	public class RuleAttributeFactoryFixture
	{
		[Test]
		public void CreateAttributeFromClass()
		{
			Attribute found = RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "AssertAnimal");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof(AssertAnimalAttribute), found.GetType());

			found = RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "AssertAnimalAttribute");
			Assert.IsNotNull(found);
			Assert.AreEqual(typeof(AssertAnimalAttribute), found.GetType());
		}

		[Test, ExpectedException(typeof(InvalidAttributeNameException))]
		public void CreateAttributeFromClassWrongName()
		{
			RuleAttributeFactory.CreateAttributeFromClass(typeof(Suricato), "assertanimal");
		}

		[Test, ExpectedException(typeof(ValidatorConfigurationException))]
		public void CreateAttributeFromRule()
		{
			// Testing exception when a new element is added in XSD but not in factory
			RuleAttributeFactory.CreateAttributeFromRule("AnyObject", "", "");

			// For wellKnownRules we can't do a sure tests because we don't have a way to auto-check all
			// classes derivered from serialization.
		}

		[Test]
		public void KnownRulesConvertAssing()
		{
			NhvMapping map = MappingLoader.GetMappingFor(typeof(WellKnownRules));
			NhvmClass cm = map.@class[0];
			XmlClassMapping rm = new XmlClassMapping(cm);
			MemberInfo mi;
			List<Attribute> attributes;

			mi = typeof(WellKnownRules).GetField("AP");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			Assert.AreEqual("A string value", ((ACustomAttribute)attributes[0]).Value1);
			Assert.AreEqual(123, ((ACustomAttribute)attributes[0]).Value2);
			Assert.AreEqual("custom message", ((ACustomAttribute)attributes[0]).Message);

			mi = typeof(WellKnownRules).GetField("StrProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			NotEmptyAttribute nea = FindAttribute<NotEmptyAttribute>(attributes);
			Assert.AreEqual("not-empty message", nea.Message);
			
			NotNullAttribute nna = FindAttribute<NotNullAttribute>(attributes);
			Assert.AreEqual("not-null message", nna.Message);
			
			NotNullOrEmptyAttribute nnea = FindAttribute<NotNullOrEmptyAttribute>(attributes);
			Assert.AreEqual("notnullorempty message", nnea.Message);
			
			LengthAttribute la = FindAttribute<LengthAttribute>(attributes);
			Assert.AreEqual("length message", la.Message);
			Assert.AreEqual(1, la.Min);
			Assert.AreEqual(10, la.Max);
			
			PatternAttribute pa = FindAttribute<PatternAttribute>(attributes);
			Assert.AreEqual("pattern message", pa.Message);
			Assert.AreEqual("[0-9]+", pa.Regex);
			// TODO : Change the XSD to allow the definition of PatternAttribute.RegexOptions

			EmailAttribute ea = FindAttribute<EmailAttribute>(attributes);
			Assert.AreEqual("email message", ea.Message);

			IPAddressAttribute ipa = FindAttribute<IPAddressAttribute>(attributes);
			Assert.AreEqual("ipAddress message", ipa.Message);

			mi = typeof(WellKnownRules).GetField("DtProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			FutureAttribute fa = FindAttribute<FutureAttribute>(attributes);
			Assert.AreEqual("future message", fa.Message);
			PastAttribute psa = FindAttribute<PastAttribute>(attributes);
			Assert.AreEqual("past message", psa.Message);

			mi = typeof(WellKnownRules).GetField("DecProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			DigitsAttribute dga = FindAttribute<DigitsAttribute>(attributes);
			Assert.AreEqual("digits message", dga.Message);
			Assert.AreEqual(5, dga.IntegerDigits);
			Assert.AreEqual(2, dga.FractionalDigits);

			MinAttribute mina = FindAttribute<MinAttribute>(attributes);
			Assert.AreEqual("min message", mina.Message);
			Assert.AreEqual(100, mina.Value);

			MaxAttribute maxa = FindAttribute<MaxAttribute>(attributes);
			Assert.AreEqual("max message", maxa.Message);
			Assert.AreEqual(200, maxa.Value);

			mi = typeof(WellKnownRules).GetField("BProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			AssertTrueAttribute ata = FindAttribute<AssertTrueAttribute>(attributes);
			Assert.AreEqual("asserttrue message", ata.Message);

			mi = typeof(WellKnownRules).GetField("ArrProp");
			attributes = new List<Attribute>(rm.GetMemberAttributes(mi));
			SizeAttribute sa = FindAttribute<SizeAttribute>(attributes);
			Assert.AreEqual("size message", sa.Message);
			Assert.AreEqual(2, sa.Min);
			Assert.AreEqual(9, sa.Max);
		}

		private static T FindAttribute<T>(List<Attribute> attr) where T : Attribute
		{
			return (T)attr.Find(delegate(Attribute a)
														{ return a is T; });
		}
	}
}