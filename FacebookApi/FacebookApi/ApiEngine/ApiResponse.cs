﻿using System;
using System.Collections.Generic;
using System.Linq;
using FacebookApi.Constants;
using RestSharp;
using FacebookApi.Interfaces.IApiEngine;

namespace FacebookApi.ApiEngine
{
    /// <summary>
    /// Represents response received from api
    /// </summary>
    /// <typeparam name="TEntity">Entity class which can be used to represent received API response</typeparam>
    public class ApiResponse<TEntity> : IApiResponse<TEntity> where TEntity : class 
    {
        /// <summary>
        /// Api result in format of TEntity 
        /// </summary>
        public TEntity Result { get; private set; }

        /// <summary>
        /// ETag response header value
        /// </summary>
        public string ETag { get; private set; }

        /// <summary>
        /// FBTrace response header value
        /// </summary>
        public string FBTraceId { get; private set; }

        /// <summary>
        /// FBApiVersion response header value
        /// </summary>
        public string FBApiVersion { get; private set; }

        /// <summary>
        /// FBDebug response header value
        /// </summary>
        public string FBDebug { get; private set; }

        /// <summary>
        /// List of exceptions from api response
        /// </summary>
        public IEnumerable<Exception> Exceptions { get; set; }

        /// <summary>
        /// Initialize instance of <see cref="ApiResponse{TEntity}"/>
        /// </summary>
        /// <param name="responce">Api Response of type TEntity</param>
        /// <param name="headerParameters">Response headers</param>
        /// <param name="exceptions">API response exceptions</param>
        public ApiResponse(TEntity responce, IList<Parameter> headerParameters, IEnumerable<Exception> exceptions)
        {
            Result = responce;
            Exceptions = exceptions;
            SetValuesFromResponceHeaders(headerParameters);
        }

        private void SetValuesFromResponceHeaders(IList<Parameter> headerParameters)
        {
            var tempParameter = headerParameters.FirstOrDefault(e => e.Name.Equals(FacebookApiResponceHeaders.X_FB_TRACE_ID));
            if (tempParameter != null)
                FBTraceId = tempParameter.Value.ToString();

            tempParameter = headerParameters.FirstOrDefault(e => e.Name.Equals(FacebookApiResponceHeaders.FACEBOOK_API_VERSION));
            if (tempParameter != null)
                FBApiVersion = tempParameter.Value.ToString();

            tempParameter = headerParameters.FirstOrDefault(e => e.Name.Equals(FacebookApiResponceHeaders.X_FB_DEBUG));
            if (tempParameter != null)
                FBDebug = tempParameter.Value.ToString();

            tempParameter = headerParameters.FirstOrDefault(e => e.Name.Equals(FacebookApiResponceHeaders.ETAG));
            if (tempParameter != null)
                ETag = tempParameter.Value.ToString();
        }

        /// <summary>
        /// Returns <see cref="Result"/>
        /// </summary>
        /// <returns><see cref="Result"/></returns>
        public TEntity GetApiResult()
        {
            return Result;
        }
    }
}