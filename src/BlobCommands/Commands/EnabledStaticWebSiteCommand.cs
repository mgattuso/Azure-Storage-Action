﻿using Azure.Storage.Blobs.Models;

using AzureStorageAction.Arguments;
using AzureStorageAction.BlobCommands.Interfaces;
using AzureStorageAction.Singletons;

using System;
using System.Threading.Tasks;

namespace AzureStorageAction.BlobCommands.Commands
{
    public class EnabledStaticWebSiteCommand : ICommand
    {
        public async Task ExecuteAction()
        {
            if (bool.TryParse(ArgumentContext.Instance.GetValue(ArgumentEnum.EnableStaticWebSite), out bool enabled))
            {
                Azure.Response<BlobServiceProperties> response = await BlobServiceClientSingleton._instance.GetBlobServiceClient().GetPropertiesAsync();
                BlobServiceProperties properties = response.Value;
                BlobStaticWebsite blobStaticWebsite = new BlobStaticWebsite
                {
                    Enabled = enabled,
                    ErrorDocument404Path = enabled ? "404.html" : null,
                    IndexDocument = enabled ? "index.html" : null
                };

                properties.StaticWebsite = blobStaticWebsite;

                await BlobServiceClientSingleton._instance.GetBlobServiceClient().SetPropertiesAsync(properties);

                if (enabled)
                {
                    Console.WriteLine("Enabled Static Web Site:");
                    Console.WriteLine("IndexDocument: index.html");
                    Console.WriteLine("ErrorDocument404Path: 404.html");
                }
                else
                {
                    Console.WriteLine("Disabled Static Web Site:");
                }
            }
        }
    }
}
