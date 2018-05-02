﻿using System;
using System.IO;
using System.Threading;
using AudioWorks.Common;
using AudioWorks.Extensions;
using JetBrains.Annotations;

namespace AudioWorks.Api
{
    static class ExtensionMethods
    {
        internal static void ProcessSamples(
            [NotNull] this ISampleProcessor sampleProcessor,
            [NotNull] string inputFilePath,
            [CanBeNull] IProgress<int> progress,
            int minFramesToReport,
            CancellationToken cancellationToken)
        {
            using (var inputStream = File.OpenRead(inputFilePath))
            {
                // Try each decoder that supports this file extension
                foreach (var decoderFactory in ExtensionProvider.GetFactories<IAudioDecoder>(
                    "Extension", Path.GetExtension(inputFilePath)))
                    try
                    {
                        var framesSinceLastProgress = 0;

                        using (var decoderExport = decoderFactory.CreateExport())
                        {
                            decoderExport.Value.Initialize(inputStream);

                            while (!decoderExport.Value.Finished)
                            {
                                cancellationToken.ThrowIfCancellationRequested();

                                using (var samples = decoderExport.Value.DecodeSamples())
                                {
                                    sampleProcessor.Submit(samples);

                                    if (progress == null ||
                                        (framesSinceLastProgress += samples.Frames) < minFramesToReport) continue;
                                    progress.Report(framesSinceLastProgress);
                                    framesSinceLastProgress = 0;
                                }
                            }
                        }

                        // Report any unreported frames
                        if (framesSinceLastProgress > 0)
                            progress?.Report(framesSinceLastProgress);

                        return;
                    }
                    catch (AudioUnsupportedException)
                    {
                        // If a decoder wasn't supported, rewind the stream and try another:
                        inputStream.Position = 0;
                    }

                throw new AudioUnsupportedException("No supporting decoders are available.");
            }
        }
    }
}
