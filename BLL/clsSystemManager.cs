using DAL;

namespace BLL
{
    public class clsSystemManager
    {
        public static void InitializeSystem()
        {
            DatabaseInitializer.Initialize();
        }
    }
}
