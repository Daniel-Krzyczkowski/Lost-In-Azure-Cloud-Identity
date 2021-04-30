using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMF.Identity.Core.Interfaces;
using TMF.Identity.Core.Model;
using TMF.Identity.Infrastructure.Configuration.Interfaces;
using TMF.Identity.Infrastructure.Logger;

namespace TMF.Identity.Infrastructure.Services
{
    public class MsGraphUserManagementService : IUserManagementService
    {
        private readonly IGraphServiceClient _graphServiceClient;
        private readonly IMsGraphServiceConfiguration _msGraphServiceConfiguration;
        private readonly ILogger<MsGraphUserManagementService> _logger;

        public MsGraphUserManagementService(IGraphServiceClient graphServiceClient,
                                            IMsGraphServiceConfiguration msGraphServiceConfiguration,
                                            ILogger<MsGraphUserManagementService> logger)
        {
            _graphServiceClient = graphServiceClient
                 ?? throw new ArgumentNullException(nameof(graphServiceClient));

            _msGraphServiceConfiguration = msGraphServiceConfiguration
                 ?? throw new ArgumentNullException(nameof(msGraphServiceConfiguration));

            _logger = logger
                 ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<UserEntity> GetUserAsync(string userId)
        {
            try
            {
                var user = await _graphServiceClient.Users[userId]
                                 .Request()
                                 .Select(e => new
                                 {
                                     e.Id,
                                     e.GivenName,
                                     e.Surname,
                                     e.Identities
                                 })
                                 .GetAsync();

                var email = user.Identities.ToList()
                            .FirstOrDefault(i => i.SignInType == "emailAddress")
                            ?.IssuerAssignedId;

                return new UserEntity
                {
                    Id = user.Id,
                    FirstName = user.GivenName,
                    LastName = user.Surname,
                    Email = email
                };
            }

            catch (ServiceException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                else
                {
                    throw;
                }
            }
        }

        public async Task<UserEntity> CreateUserAsync(UserEntity userEntity)
        {
            var user = new User
            {
                AccountEnabled = true,
                GivenName = userEntity.FirstName,
                Surname = userEntity.LastName,
                DisplayName = string.Concat(userEntity.FirstName, " ", userEntity.LastName),
                Identities = new List<ObjectIdentity>
                {
                    new ObjectIdentity
                    {
                        SignInType = "emailAddress",
                        Issuer = _msGraphServiceConfiguration.TenantName,
                        IssuerAssignedId = userEntity.Email
                    }
                },
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = "TempPass123@"
                }
            };

            var addedUser = await _graphServiceClient.Users
                     .Request()
                     .AddAsync(user);

            userEntity.Id = addedUser.Id;
            return userEntity;
        }

        public async Task DeleteUserAsync(string userId)
        {
            await _graphServiceClient.Users[userId]
                    .Request()
                    .DeleteAsync();
        }

        public async Task<IReadOnlyList<UserEntity>> GetAllUsersAsync()
        {
            List<User> users = new List<User>();
            try
            {

                IGraphServiceUsersCollectionPage iGraphServiceUsersCollectionPage = await _graphServiceClient.Users
                                                                       .Request()
                                                                       .Select($"id," +
                                                                       $" userPrincipalName," +
                                                                       $" givenName," +
                                                                       $" surname")
                                                                       .Top(50)
                                                                       .GetAsync();

                var userPageIterator = PageIterator<User>.CreatePageIterator(_graphServiceClient,
                                                           iGraphServiceUsersCollectionPage,
                                                           entity => { users.Add(entity); return true; });

                await userPageIterator.IterateAsync();
            }

            catch (ServiceException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogError(LoggerEvents.MicrosoftGraphTooManyRequests, string.Concat(ex.Message, " ", ex.InnerException?.Message));
                    var retryAfter = ex.ResponseHeaders.RetryAfter.Delta.Value;
                    await Task.Delay(TimeSpan.FromSeconds(retryAfter.TotalSeconds));
                    await GetAllUsersAsync();
                }

                else
                {
                    throw;
                }
            }

            var userEntities = users
                                  .Select(u => new UserEntity()
                                  {
                                      Id = u.Id,
                                      Email = u.UserPrincipalName,
                                      FirstName = u.GivenName,
                                      LastName = u.Surname
                                  })
                                  .ToList();

            return userEntities;
        }
    }
}
