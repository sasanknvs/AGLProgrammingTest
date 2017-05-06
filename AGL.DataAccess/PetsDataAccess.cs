using AGL.DataAccess.Interfaces;
using AGL.Models.EntityModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace AGL.DataAccess
{
    public class PetsDataAccess : IPetsDataAccess
    {
        private IRestClient _client;

        private readonly string PetsBaseUri = ConfigurationManager.AppSettings["BaseUri"];
        private readonly string ResourceUri = ConfigurationManager.AppSettings["RetrievePetsUri"];

        public PetsDataAccess(IRestClient client)
        {
            if (client == null)
            {
                throw new ArgumentNullException();
            }
            _client = client;
        }

        public OwnerPetsData RetrievePets() // remove baseuri and resource uri
        {
            OwnerPetsData ownerPetsData = new OwnerPetsData();
            IRestRequest request;
            
            try
            {
                _client.BaseUrl = new Uri(PetsBaseUri);
                request = new RestRequest(ResourceUri, Method.GET);

                var restResponse = _client.Execute<List<Owners>>(request);

                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    ownerPetsData.Owners = restResponse.Data;
                }
                else
                {
                    Error error = new Error();
                    error.ErrorMessage = restResponse.ErrorException.Message;
                    error.StatusCode = restResponse.StatusCode;
                    ownerPetsData.Errors.Add(error);
                }
            }

            //RestSharp.dll returns all the exception in the RestResponse.ErrorException object.
            // Hence cant catch specific exceptions except the generic excpetion. 
            catch (Exception ex)
            {
                ownerPetsData.Errors.Add(new Error { ErrorMessage = ex.Message, StatusCode = HttpStatusCode.InternalServerError });
            }

            return ownerPetsData;
        }
    }
}
