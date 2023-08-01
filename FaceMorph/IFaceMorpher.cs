using System.Threading.Tasks;
using System.Threading;

namespace FaceMorph
{
    public interface IFaceMorpher
    {

        Task<bool> Status(CancellationToken cancellationToken);
        Task<bool> Morph(string directory, string left, string right, string dest, int from, int to, int step);
        Task<bool> MultiMorph(string directory, string inTemplate, int items, string dest, int iterations);
    }
}