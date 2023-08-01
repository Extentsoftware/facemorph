using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FaceMorph
{
    public class FaceMorpher : IFaceMorpher
    {
        readonly IFaceMorphApi _faceMorphApi;

        public FaceMorpher(IFaceMorphApi faceMorphApi)
        {
            _faceMorphApi = faceMorphApi;
        }

        public async Task<bool> Morph(string directory, string left, string right, string dest, int from, int to, int step)
        {
            var leftFullpath = Path.Combine(directory, left);
            var leftResult = await _faceMorphApi.EncodeImage(leftFullpath, true);

            var rightFullpath = Path.Combine(directory, right);
            var rightResult = await _faceMorphApi.EncodeImage(rightFullpath, true);

            for (int i = from; i < to; i += step)
            {
                var bytes = await _faceMorphApi.MorphFrame(leftResult.Guid, rightResult.Guid, i, default);
                var destFullpath = Path.Combine(directory, dest);
                destFullpath = destFullpath.Replace("{0}", i.ToString("000"));
                File.WriteAllBytes(destFullpath, bytes);
            }

            return true;
        }
        
        public async Task<bool> MultiMorph(string directory, string inTemplate, int items, string dest, int iterations)
        {
            List<MultiMorphItem> morphRequest = new();

            

            for (int i = 1; i <= items; i++)
            {
                var fullpath = Path.Combine(directory, inTemplate).Replace("{0}",i.ToString());
                var image = await _faceMorphApi.EncodeImage(fullpath, true);
                var morphItem = new MultiMorphItem
                {
                    Amount = 0,
                    Guid = image.Guid,
                };
                morphRequest.Add(morphItem);
            }


            for (int i = 0; i < iterations; i++)
            {
                
                foreach (var req in morphRequest)
                    req.Amount = Random.Shared.NextInt64(0, 20) * 5 / 100f;

                string id = string.Join("_", morphRequest.Select(x => (x.Amount*100).ToString("00")));
                var bytes = await _faceMorphApi.MultiMorphFrame(morphRequest.ToArray(), default);
                var destFullpath = Path.Combine(directory, dest);
                destFullpath = destFullpath.Replace("{0}", id);
                File.WriteAllBytes(destFullpath, bytes);
            }

            return true;
        }

        public Task<bool> Status(CancellationToken cancellationToken)
        {
            return _faceMorphApi.Status(cancellationToken);
        }

    }
}
