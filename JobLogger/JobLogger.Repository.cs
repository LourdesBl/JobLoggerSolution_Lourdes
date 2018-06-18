using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EntityFramework.CodeFirst;
using EntityFramework.CodeFirst.Entities;

namespace JobLogger
{
	public interface IJobLoggerRepository
	{
		void SaveIntoLogValue(LogValue logValue);
		void DeleteFromLogValue(Guid logValueId);
		IList<LogValue> GetAllLogValues();
	}

	public class JobLoggerRepository : IJobLoggerRepository
	{
		public void SaveIntoLogValue(LogValue logValue)
		{
			using (var connection = new ApplicationDbContext())
			{
				connection.LogValues.Add(new LogValue()
				{
					Id = Guid.NewGuid(),
					Code = logValue.Code,
					Message = logValue.Message,
					InsertDateTime = DateTime.Now
				});
				connection.SaveChanges();
			}
		}

		public void DeleteFromLogValue(Guid logValueId)
		{
			using (var connection = new ApplicationDbContext())
			{
				var logValue = connection.LogValues.FirstOrDefault(x => x.Id.Equals(logValueId));
				if(logValue != null)
					connection.LogValues.Remove(logValue);

				connection.SaveChanges();
			}
		}

		public IList<LogValue> GetAllLogValues()
		{
			IList<LogValue> allLogValues;
			using (var connection = new ApplicationDbContext())
			{
				allLogValues = connection.LogValues.ToList();
			}
			
			return allLogValues;
		}
	}
}
