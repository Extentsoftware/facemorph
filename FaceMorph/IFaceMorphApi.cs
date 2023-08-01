using System;
using System.Threading;
using System.Threading.Tasks;

namespace FaceMorph
{
    public interface IFaceMorphApi
    {
        Task<bool> Status(CancellationToken cancellationToken);
        Task<EncodeImageResult> EncodeImage(string filename, bool tryalign);
        Task<byte[]> MorphFrame(Guid from, Guid to, int frameNumber, CancellationToken cancellationToken);
        Task<byte[]> MultiMorphFrame(MultiMorphItem[] items, CancellationToken cancellationToken);
    }
}