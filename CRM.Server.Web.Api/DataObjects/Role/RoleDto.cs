using CRM.Server.Models;
using System.Collections.Generic;

namespace CRM.Server.Web.Api.DataObjects
{
	public class CreateRoleDto
	{
		public string RoleName { get; set; }
	}
	public class ViewRoleDto
	{
		public int Id { get; set; }
		public string RoleName { get; set; }
		public IEnumerable<string> Users { get; set; }
	}
	public class EditRoleDto
	{
		public int Id { get; set; }
		public string RoleName { get; set; }
	}
	public class UserRoleDto
	{
		public long UserId { get; set; }
		public string UserName { get; set; }
		public bool IsSelected { get; set; }

		public string FirstName { get; set; }

        public string LastName { get; set; }
    }
	public class UpdateUsersInRoleDto
	{
		public long RoleId { get; set; }
		public IEnumerable<UserRoleDto> userRoleModelList { get; set; }		
	}
}

