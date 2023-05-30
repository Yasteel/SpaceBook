namespace Spacebook.Interfaces
{
    using Spacebook.Models;
    public interface IProfileService: IGenericService<Profile>
	{
		Profile GetByUsername(string username);

		Profile GetByEmail(string email);
	}
}
