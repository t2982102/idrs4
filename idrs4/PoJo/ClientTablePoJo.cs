using IdentityServer4.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace idrs4.PoJo
{
    public class ClientTablePoJo
    {
        public bool AllowOfflineAccess { get; set; }
        public List<string> AllowedScopes { get; set; }
        public int IdentityTokenLifetime { get; set; }
        public int AccessTokenLifetime { get; set; }
        public int AuthorizationCodeLifetime { get; set; }
        public int? ConsentLifetime { get; set; }
        public int AbsoluteRefreshTokenLifetime { get; set; }
        public int SlidingRefreshTokenLifetime { get; set; }
        public int RefreshTokenUsage { get; set; }
        public bool UpdateAccessTokenClaimsOnRefresh { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public int AccessTokenType { get; set; }
        public bool EnableLocalLogin { get; set; }
        public List<string> IdentityProviderRestrictions { get; set; }
        public bool IncludeJwtId { get; set; }
        public List<string> Claims { get; set; }
        public bool AlwaysSendClientClaims { get; set; }
        public string ClientClaimsPrefix { get; set; }
        public string PairWiseSubjectSalt { get; set; }
        public bool BackChannelLogoutSessionRequired { get; set; }
        public List<string> AllowedCorsOrigins { get; set; }
        public string BackChannelLogoutUri { get; set; }
        public string FrontChannelLogoutUri { get; set; }
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string ClientId { get; set; }
        public string ProtocolType { get; set; }
        public List<string> ClientSecrets { get; set; }
        public bool RequireClientSecret { get; set; }
        public string ClientName { get; set; }
        public string Description { get; set; }
        public string ClientUri { get; set; }
        public string LogoUri { get; set; }
        public bool RequireConsent { get; set; }
        public bool AllowRememberConsent { get; set; }
        public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
        public List<string> AllowedGrantTypes { get; set; }
        public bool RequirePkce { get; set; }
        public bool AllowPlainTextPkce { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public List<string> RedirectUris { get; set; }
        public List<string> PostLogoutRedirectUris { get; set; }
        public bool FrontChannelLogoutSessionRequired { get; set; }
        public List<string> Properties { get; set; }


        public ClientTablePoJo(Client c)
        {
            AllowOfflineAccess = c.AllowOfflineAccess;
            AllowedScopes = c.AllowedScopes.Select(o => o.Scope).ToList();
            IdentityTokenLifetime = c.IdentityTokenLifetime;
            AccessTokenLifetime = c.AccessTokenLifetime;
            AuthorizationCodeLifetime = c.AuthorizationCodeLifetime;
            ConsentLifetime = c.ConsentLifetime;
            AbsoluteRefreshTokenLifetime = c.AbsoluteRefreshTokenLifetime;
            SlidingRefreshTokenLifetime = c.SlidingRefreshTokenLifetime;
            RefreshTokenUsage = c.RefreshTokenUsage;
            UpdateAccessTokenClaimsOnRefresh = c.UpdateAccessTokenClaimsOnRefresh;
            RefreshTokenExpiration = c.RefreshTokenExpiration;
            AccessTokenType = c.AccessTokenType;
            EnableLocalLogin = c.EnableLocalLogin;
            IdentityProviderRestrictions = c.IdentityProviderRestrictions.Select(o => o.Provider).ToList();
            IncludeJwtId = c.IncludeJwtId;
            Claims = c.Claims.Select(o=>o.Value).ToList();
            AlwaysSendClientClaims = c.AlwaysSendClientClaims;
            ClientClaimsPrefix = c.ClientClaimsPrefix;
            PairWiseSubjectSalt = c.PairWiseSubjectSalt;
            BackChannelLogoutSessionRequired = c.BackChannelLogoutSessionRequired;
            AllowedCorsOrigins = c.AllowedCorsOrigins.Select(o=>o.Origin).ToList();
            BackChannelLogoutUri = c.BackChannelLogoutUri;
            FrontChannelLogoutUri = c.FrontChannelLogoutUri;
            Id = c.Id;
            Enabled = c.Enabled;
            ClientId = c.ClientId;
            ProtocolType = c.ProtocolType;
            ClientSecrets = c.ClientSecrets.Select(o => o.Value).ToList();
            RequireClientSecret = c.RequireClientSecret;
            ClientName = c.ClientName;
            Description = c.Description;
            ClientUri = c.ClientUri;
            LogoUri = c.LogoUri;
            RequireConsent = c.RequireConsent;
            AllowRememberConsent = c.AllowRememberConsent;
            AlwaysIncludeUserClaimsInIdToken = c.AlwaysIncludeUserClaimsInIdToken;
            AllowedGrantTypes = c.AllowedGrantTypes.Select(o=>o.GrantType).ToList();
            RequirePkce = c.RequirePkce;
            AllowPlainTextPkce = c.AllowPlainTextPkce;
            AllowAccessTokensViaBrowser = c.AllowAccessTokensViaBrowser;
            RedirectUris = c.RedirectUris.Select(o=>o.RedirectUri).ToList();
            PostLogoutRedirectUris = c.PostLogoutRedirectUris.Select(o=>o.PostLogoutRedirectUri).ToList();
            FrontChannelLogoutSessionRequired = c.FrontChannelLogoutSessionRequired;
            Properties = c.Properties.Select(o => o.Value).ToList();
        }
        public static List<ClientTablePoJo> GetClientTablePoJoList(List<Client> clist)
        {
            List<ClientTablePoJo> ctplist = new List<ClientTablePoJo>();
            foreach (var c in clist)
            {
                var a = new ClientTablePoJo(c);
                ctplist.Add(a);
            }
            return ctplist;
        }
    }
}
