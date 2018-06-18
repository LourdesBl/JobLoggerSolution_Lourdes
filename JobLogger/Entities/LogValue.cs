using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework.CodeFirst.Entities
{
	public class LogValue
	{
		[Key]
		[Required]
		public Guid Id { get; set; }

		public int Code { get; set; }
		public string Message { get; set; }

		public DateTime InsertDateTime { get; set; }


	}
}
