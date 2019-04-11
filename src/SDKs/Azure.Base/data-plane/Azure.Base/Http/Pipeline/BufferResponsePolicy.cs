﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Threading.Tasks;

namespace Azure.Base.Http.Pipeline
{
    public class BufferResponsePolicy: HttpPipelinePolicy
    {
        public static HttpPipelinePolicy Singleton { get; set; } = new BufferResponsePolicy();

        public override async Task ProcessAsync(HttpPipelineMessage message, ReadOnlyMemory<HttpPipelinePolicy> pipeline)
        {
            await ProcessNextAsync(pipeline, message);

            if (message.Response.ResponseContentStream != null)
            {
                Stream responseContentStream = message.Response.ResponseContentStream;
                var bufferedStream = new MemoryStream();
                await responseContentStream.CopyToAsync(bufferedStream);
                bufferedStream.Position = 0;
                message.Response.ResponseContentStream = bufferedStream;
            }
        }
    }
}