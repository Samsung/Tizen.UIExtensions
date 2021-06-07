using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tizen.Applications;
using NImage = Tizen.NUI.BaseComponents.ImageView;

namespace Tizen.UIExtensions.NUI
{
    internal class StreamImageSourceService
    {
        Dictionary<WeakReference<NImage>, string> _imageCache = new Dictionary<WeakReference<NImage>, string>();

        public static StreamImageSourceService Instance { get; } = new StreamImageSourceService();

        StreamImageSourceService() { }

        /// <summary>
        /// Stream to file
        /// </summary>
        /// <param name="view">A view that owns the stream </param>
        /// <param name="stream">A stream to convert file</param>
        /// <returns>a temporary file path</returns>
        public async Task<string> AddStream(NImage view, Stream stream)
        {
            // Remove a cache that duplicated or released object
            foreach (var weakRef in _imageCache.Keys.ToList())
            {
                if (!weakRef.TryGetTarget(out var target) || target == view)
                {
                    RemoveTemporaryFile(_imageCache[weakRef]);
                    _imageCache.Remove(weakRef);
                }
            }

            var tempfile = Path.Combine(Application.Current.DirectoryInfo.Cache, Path.GetRandomFileName());
            using (var fs = new FileStream(tempfile, FileMode.OpenOrCreate))
            {
                await stream.CopyToAsync(fs);
            }
            _imageCache[new WeakReference<NImage>(view)] = tempfile;

            return tempfile;
        }

        void RemoveTemporaryFile(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                }
            }
            catch (Exception)
            {
                Common.Log.Error("Fail to remove temporary file");
            }
        }
    }
}
