using Spacebook.Models;

namespace Spacebook.Interfaces
{
	public interface IProfileService : IGenericService<Profile>
	{
		Profile GetByUsername(string username);
	}
}
