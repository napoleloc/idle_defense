using System.Threading;
using Cysharp.Threading.Tasks;

namespace Module.GameUI.InGame.Attribute.GridSheet
{
    public interface IAttributeControlGridSheet
    {
        void Initialize();
        void Cleanup();
    }
}
