namespace School_pws.Helpers
{
    public interface IEncryptHelper
    {
        string EncryptString(string Message);

        string DecryptString(string Message);
    }
}
