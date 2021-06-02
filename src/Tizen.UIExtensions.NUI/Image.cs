using System;
using System.IO;
using Tizen.UIExtensions.Common;
using CSize = Tizen.UIExtensions.Common.Size;
using ImageView = Tizen.NUI.BaseComponents.ImageView;

namespace Tizen.UIExtensions.NUI
{
    /// <summary>
    /// View that holds an image.
    /// </summary>
    public class Image : ImageView, IMeasurable
    {
        string? _temporaryFile;

        /// <summary>
        /// Gets or sets the scaling mode for the image.
        /// </summary>
        public Aspect Aspect
        {
            get => this.GetAspect();
            set => this.SetAspect(value);
        }

        /// <summary>
        /// Measures the size of the control in order to fit it into the available area.
        /// </summary>
        /// <param name="availableWidth">Available width.</param>
        /// <param name="availableHeight">Available height.</param>
        /// <returns>Size of the control that fits the available area.</returns>
        CSize IMeasurable.Measure(double availableWidth, double availableHeight)
        {
            return this.Measure(availableWidth, availableHeight);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                RemoveTemporaryFile();
            }
            base.Dispose(disposing);
        }

        internal void SetTemporaryFile(string path)
        {
            _temporaryFile = path;
        }

        internal void RemoveTemporaryFile()
        {
            try
            {
                if (!string.IsNullOrEmpty(_temporaryFile))
                {
                    if (File.Exists(_temporaryFile))
                    {
                        File.Delete(_temporaryFile);
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
