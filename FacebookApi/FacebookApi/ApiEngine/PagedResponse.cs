﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacebookApi.Constants;
using FacebookApi.Entities.ApiEngine;
using FacebookApi.Exceptions;
using FacebookApi.Interfaces.IApiEngine;
using RestSharp;
using RestSharp.Deserializers;

namespace FacebookApi.ApiEngine
{
    /// <summary>
    /// Represents paged API response
    /// </summary>
    /// <typeparam name="TApiEntity">Entity class which can be used to represent received API response</typeparam>
    public class PagedResponse<TApiEntity> : IPagedResponse<TApiEntity> where TApiEntity : class, new()
    {
        /// <summary>
        /// <see cref="ApiClient"/> used to execute <see cref="PagedApiRequest"/>
        /// </summary>
        public ApiClient ApiClient { get; private set; }

        /// <summary>
        /// Data from paged API response
        /// </summary>
        [DeserializeAs(Name = "data")]
        public List<TApiEntity> Data { get; set; }

        /// <summary>
        /// Paging information from API response
        /// </summary>
        [DeserializeAs(Name = "paging")]
        public Paging Paging { get; set; }

        /// <summary>
        /// API Response headers collection
        /// </summary>
        public IDictionary<string,string> ResponseHeaders { get; set; }

        /// <summary>
        /// API response exceptions
        /// </summary>
        public IEnumerable<FacebookOAuthException> Exceptions { get; set; }

        /// <summary>
        /// Initialize new instance of <see cref="PagedResponse{TApiEntity}"/>
        /// </summary>
        public PagedResponse()
        {
        }

        /// <summary>
        /// Set response headers
        /// </summary>
        /// <param name="headers"></param>
        internal void SetResponseHeaders(IEnumerable<Parameter> headers)
        {
            var parameters = headers as IList<Parameter> ?? headers.ToList();
            ResponseHeaders = parameters.ToDictionary(parameter => parameter.Name, parameter => parameter.Value.ToString());
        }

        /// <summary>
        /// Set response excecptions
        /// </summary>
        /// <param name="exceptions"></param>
        internal void SetResponseExceptions(IEnumerable<FacebookOAuthException> exceptions)
        {
            Exceptions = exceptions;
        }

        /// <summary>
        /// Set <see cref="ApiClient"/> value
        /// </summary>
        /// <param name="apiClient"></param>
        internal void SetApiClient(ApiClient apiClient)
        {
            this.ApiClient = apiClient;
        }

        private PagedApiRequest _getNextPageRequest()
        {
            if (string.IsNullOrEmpty(Paging.Next))
            {
                throw new NullReferenceException("Next page link is empty.");
            }

            if (!Uri.TryCreate(Paging.Next, UriKind.Absolute, out Uri nextPageUri))
                throw new UriFormatException("Next page uri is invalid.");

            //stupid logic :(
            var url = $"{string.Join(string.Empty, nextPageUri.Segments.Skip(2))}{nextPageUri.Query}";

            var nextPageRequest = new PagedApiRequest(url, ApiClient);

            return nextPageRequest;
        }

        private PagedApiRequest _getPreviousPageRequest()
        {
            if (string.IsNullOrEmpty(Paging.Previous))
            {
                throw new NullReferenceException("Previous page link is empty.");
            }

            if (!Uri.TryCreate(Paging.Previous, UriKind.Absolute, out Uri previousPageUri))
                throw new UriFormatException("Previous page uri is invalid.");

            //stupid logic :(
            var url = $"{previousPageUri.Segments.Skip(0).Skip(1)}{previousPageUri.Query}";

            var previousPageRequest = new PagedApiRequest(url, ApiClient);
            return previousPageRequest;
        }

        /// <summary>
        /// Get next page data from API
        /// </summary>
        /// <returns>Next page data</returns>
        public IPagedResponse<TApiEntity> GetNextPageData()
        {
            var nextPageRequest = _getNextPageRequest();
            return nextPageRequest.ExecutePage<TApiEntity>();
        }

        /// <summary>
        /// Get next page data from API
        /// </summary>
        /// <returns>Next page data</returns>
        public async Task<IPagedResponse<TApiEntity>> GetNextPageDataAsync()
        {
            var nextPageRequest = _getNextPageRequest();
            return await nextPageRequest.ExecutePageAsync<TApiEntity>();
        }

        /// <summary>
        /// Get previous page data.
        /// </summary>
        /// <returns>Previous page data</returns>
        public IPagedResponse<TApiEntity> GetPreviousPageData()
        {
            var previousPageRequest = _getPreviousPageRequest();
            return previousPageRequest.ExecutePage<TApiEntity>();
        }

        /// <summary>
        /// Get previous page data.
        /// </summary>
        /// <returns>Previous page data</returns>
        public async Task<IPagedResponse<TApiEntity>> GetPreviousPageDataAsync()
        {
            var previousPageRequest = _getPreviousPageRequest();
            return await previousPageRequest.ExecutePageAsync<TApiEntity>();
        }

        /// <summary>
        /// Get API response data
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TApiEntity> GetResultData()
        {
            return this.Data;
        }

        /// <summary>
        /// Check if next page data is available or not
        /// </summary>
        /// <returns>True of <see cref="Paging.Next"/> is not null</returns>
        public bool IsNextPageDataAvailable()
        {
            return !string.IsNullOrEmpty(Paging?.Next);
        }

        /// <summary>
        /// Check if previous page data is available or not
        /// </summary>
        /// <returns>True of <see cref="Paging.Previous"/> is not null</returns>
        public bool IsPreviousPageDataAvailable()
        {
            return !string.IsNullOrEmpty(Paging?.Previous);
        }

        /// <summary>
        /// Get next page url
        /// </summary>
        /// <returns>Next data page url</returns>
        public string GetNextPageUrl()
        {
            return this.Paging.Next;
        }

        /// <summary>
        /// Checks if API has returned any data or not
        /// </summary>
        /// <returns>True if data is available</returns>
        public bool IsDataAvailable()
        {
            return Data != null;
        }

        /// <summary>
        /// Get list of exceptions from API response.
        /// </summary>
        /// <returns>List of exceptions</returns>
        public IEnumerable<Exception> GetApiExceptions()
        {
            return Exceptions;
        }
    }
}
