using Flurl;
using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FaceMorph
{
    /// <summary>
    /// https://checkface.facemorph.me/api
    /// </summary>
    public class FaceMorphApi : IFaceMorphApi
    {
        readonly string _baseUrl;

        public FaceMorphApi(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<bool> Status(CancellationToken cancellationToken)
        {
            var result = await (_baseUrl + "/status").GetAsync(cancellationToken);
            return result.StatusCode == 200;
        }

        public async Task<EncodeImageResult> EncodeImage(string filename, bool tryalign)
        {
            //https://stackoverflow.com/questions/41042591/how-can-i-upload-a-file-and-form-data-using-flurl
            var resp = await (_baseUrl + "/api/encodeimage")
                .PostMultipartAsync(mp => mp
                    .AddString("tryalign", tryalign.ToString())
                    .AddFile("usrimg", filename) ).ReceiveJson<EncodeImageResult>();
            return resp;
        }
        public async Task<byte[]> MorphFrame(Guid from, Guid to, int frameNumber, CancellationToken cancellationToken)
        {
            var result = await (_baseUrl + "/api/morphframe")
                .SetQueryParam("num_frames", 100)
                .SetQueryParam("frame_num", frameNumber)
                .SetQueryParam("from_guid", from)
                .SetQueryParam("to_guid", to)
                .GetBytesAsync(cancellationToken);
            return result;
        }

        public async Task<byte[]> MultiMorphFrame(MultiMorphItem[] items, CancellationToken cancellationToken)
        {
            string url = (_baseUrl + "/api/face")
                .SetQueryParam("num_multi", items.Length);

            for( int i=0; i< items.Length; i++)
            {
                url = url.SetQueryParam($"guid{i}", items[i].Guid);
                url = url.SetQueryParam($"amount{i}", items[i].Amount);
            }
            var result = await url.GetBytesAsync(cancellationToken);
            return result;
        }
    }
    public class MultiMorphItem
    {
        public Guid Guid { get; set; }
        public float Amount { get; set; }
    }
    
    public class EncodeImageResult
    {
        [JsonProperty("did_align")]
        public bool DidAlign { get; set; }

        [JsonProperty("guid")]
        public Guid Guid { get; set; }
    }
    
}
