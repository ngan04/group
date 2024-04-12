using System;
namespace SIMS_Demo.Models
{
	public class Teacher
	{
		public int Id { get; set; }
		public String Name { get; set; }
		public DateTime DoB { get; set; }
        public string ImageUrl { get; set; } // Thêm thuộc tính cho URL của ảnh đại diện
        public Teacher()
		{
		}
	}
}

