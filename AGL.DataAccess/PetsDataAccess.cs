using AGL.DataAccess.Interfaces;
using AGL.Models.EntityModels;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace AGL.DataAccess
{
    /// <summary>
    /// Data Access layer
    /// </summary>
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

        /// <summary>
        /// Retrieve pets by calling the api
        /// </summary>
        /// <returns></returns>
        public OwnerPetsData RetrievePets()
        {
            OwnerPetsData ownerPetsData = new OwnerPetsData();
            IRestRequest request;
            
            try
            {
                _client.BaseUrl = new Uri(PetsBaseUri);
                request = new RestRequest(ResourceUri, Method.GET);

                // call the api
                var restResponse = _client.Execute<List<Owners>>(request);

                // Api returns successful response
                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    ownerPetsData.Owners = restResponse.Data;
                }
                else
                {
                    // Api returned invalid response, map the exception
                    Error error = new Error();
                    error.ErrorMessage = restResponse.ErrorException.Message;
                    error.StatusCode = restResponse.StatusCode;
                    ownerPetsData.Errors.Add(error);
                }
            }

            //RestSharp.dll returns all the exceptions in the RestResponse.ErrorException object.
            // Hence handling only generic exceptions
            catch (Exception ex)
            {
                ownerPetsData.Errors.Add(new Error { ErrorMessage = ex.Message, StatusCode = HttpStatusCode.InternalServerError });
            }

            return ownerPetsData;
        }
    }
}
