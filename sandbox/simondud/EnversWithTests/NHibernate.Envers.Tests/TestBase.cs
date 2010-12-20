using System.Collections.Generic;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace NHibernate.Envers.Tests
{
	public abstract class TestBase
	{
		private Cfg.Configuration cfg;
		private const string MappingAssembly = "NHibernate.Envers.Tests";
		public ISession Session { get; private set; }
		public IAuditReader AuditReader { get; private set; }


		[SetUp]
		public void BaseSetup()
		{
			cfg = new Cfg.Configuration();
			cfg.Configure();
			addMappings();
			cfg.IntegrateWithEnvers();
			var sf = cfg.BuildSessionFactory();
			createDropSchema(true);
			using (Session = sf.OpenSession())
			{
				Initialize();				
			}
			Session = sf.OpenSession();
			AuditReader = AuditReaderFactory.Get(Session);
		}

		protected abstract void Initialize();

		private void createDropSchema(bool both)
		{
			var se = new SchemaExport(cfg);
			se.Drop(false, true);
			if(both)
				se.Create(false,true);
		}

		[TearDown]
		public void BaseTearDown()
		{
			Session.Close();
			createDropSchema(false);
		}

		protected abstract IEnumerable<string> Mappings { get; }

		private void addMappings()
		{
			var ass = Assembly.Load(MappingAssembly);
			foreach (var mapping in Mappings)
			{
				cfg.AddResource(MappingAssembly + "." + mapping, ass);
			}
		}
	}
}