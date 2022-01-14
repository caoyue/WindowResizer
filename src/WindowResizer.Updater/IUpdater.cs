using System.Threading.Tasks;

namespace WindowResizer.Updater
{
    public interface IUpdater
    {
        Task Update();
    }
}
