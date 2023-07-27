﻿using System;
using System.Net;
using Facebook.ApiClient.Constants;
using Facebook.ApiClient.Exceptions;

namespace Facebook.ApiClient.ApiEngine
{
    /// <summary>
    /// Represents Facebook api client. It contains information required for doing api calls
    /// </summary>
    [Serializable]
    public class ApiClient
    {
        /// <summary>
        /// An access token is an opaque string that identifies a user, app, or Page and can be used by the app to make graph API calls.
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Facebook api version to use while making api calls
        /// </summary>
        public string Version { get; private set; }

        /// <summary>
        /// Facebook Application Id
        /// </summary>
        public string AppId { get; private set; }

        /// <summary>
        /// Facebook Application secret key
        /// </summary>
        public string AppSecret { get; private set; }

        /// <summary>
        /// Create new instance of <see cref="ApiClient"/> using given <see cref="AccessToken"/> &amp; <see cref="Version"/>
        /// </summary>
        /// <param name="accessToken"><see cref="AccessToken"/></param>
        /// <param name="version"><see cref="Version"/></param>
        public ApiClient(string accessToken, string version)
        {
            // force TLSv1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _setApiVersion(version);
            _setAccessToken(accessToken);
        }

        /// <summary>
        /// Create new instance of <see cref="ApiClient"/> using given <see cref="AppId"/>, <see cref="AppSecret"/>, <see cref="AccessToken"/> &amp; <see cref="Version"/>
        /// </summary>
        /// <param name="appId"><see cref="AppId"/></param>
        /// <param name="appSecret"><see cref="AppSecret"/></param>
        /// <param name="accessToken"><see cref="AccessToken"/></param>
        /// <param name="version"><see cref="Version"/></param>
        public ApiClient(string appId, string appSecret, string accessToken, string version)
        {
            // force TLSv1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            _setApiVersion(version);
            _setAccessToken(accessToken);
            _setAppId(appId, appSecret);
        }

        private void _setApiVersion(string version)
        {
            Version = version;
        }

        private void _setAccessToken(string accessToken)
        {
            this.AccessToken = accessToken;
        }

        private void _setAppId(string appId, string appSecret)
        {
            AppId = appId;
            AppSecret = appSecret;
        }
    }
}
