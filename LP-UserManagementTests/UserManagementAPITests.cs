using System.Net;
using System.Net.Http.Json;
using LP_UserManagementTests.Data;

namespace LP_UserManagementTests
{
    public class UserManagementApiTests
    {
        private string _baseAddress;
        private string _token;

        [OneTimeSetUp]
        public void Setup()
        {
            Console.WriteLine("Starting API Testing setup");
            Console.WriteLine("Checking if environment has been specified:");
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            if (environment == null)
            {
                Console.WriteLine("Environment not specified, using dev as default");
                environment = "dev";
            }
            else
            {
                Console.WriteLine($"Environment value found, using {environment} as environment for testing");
            }

            Console.WriteLine("Checking if base url has been specified:");
            var baseURL = Environment.GetEnvironmentVariable("API_BASE_URL");
            if(baseURL == null)
            {
                Console.WriteLine("Base url not specified, using http://localhost:3000 as default");
                baseURL = "http://localhost:3000";
            }
            else
            {
                Console.WriteLine($"Base url found, using {baseURL} as base url for testing");
            }


            _baseAddress = $"{baseURL}/{environment}/";
            Console.WriteLine($"API base address to be used: {_baseAddress}");
            _token = "mysecrettoken";
        }
        #region Post /users

        [Test]
        public async Task Given_NewUser_When_UserInfoIsValid_Then_CreateUser()
        {
            var newUser = new User("John Doe", GetMockEmail(), 21);

            var user = await CreateUserAsync(newUser);

            Assert.That(user, Is.Not.Null, "Api failed to create new user");
        }

        [Test]
        public async Task Given_NewUser_UserInfoIsInvalid_Then_Error400()
        {
            var newUser = new User("John Doe", string.Empty, 21);

            var exception = Assert.ThrowsAsync<Exception>(async () => await CreateUserAsync(newUser));

            Assert.That(exception.Message, Is.EqualTo("400"), "Api is not producing 400 error when data is invalid");
        }

        [Test]
        public async Task Given_NewUser_UserAgeOver150_Then_Error400()
        {
            var newUser = new User("John Doe", GetMockEmail(), 151);

            var exception = Assert.ThrowsAsync<Exception>(async () => await CreateUserAsync(newUser));

            Assert.That(exception.Message, Is.EqualTo("400"), "Api is not producing 400 error when age is over 150");
        }

        [Test]
        public async Task Given_NewUser_When_UserEmailIsDuplicated_Then_Error409()
        {
            var email = await GetValidEmail();

            var duplicateUser = new User("John Doe", email, 21);

            var exception = Assert.ThrowsAsync<Exception>(async () => await CreateUserAsync(duplicateUser));

            Assert.That(exception.Message, Is.EqualTo("409"), "Api is not producing 409 error when email is duplicated");
        }

        #endregion

        #region Get /users

        [Test]
        public async Task Given_UsersAreRequested_Then_ListUsers()
        {
            using var client = new HttpClient{BaseAddress = new Uri(_baseAddress)};

            var users = await client.GetFromJsonAsync<List<User>>("users");

            Assert.That(users, Is.Not.Null, "Api failed to provide users list");
        }

        #endregion

        #region Get /users/{email}

        [Test]
        public async Task Given_UserIsRequested_When_ValidEmail_GetUser()
        {
            //Arrange user
            var userEmail = await GetValidEmail();

            //Act
            var existingUser = await GetUser(userEmail);

            //Assert
            Assert.That(existingUser, Is.Not.Null, "Api failed to provide user");
        }

        [Test]
        public async Task Given_UserIsRequested_When_InvalidEmail_Error404()
        {
            var userEmail = GetMockEmail();

            var exception = Assert.ThrowsAsync<Exception>(async () => await GetUser(userEmail));

            Assert.That(exception.Message, Is.EqualTo("404"), "Api is not producing 404 error when email doesn't match");
        }

        #endregion

        #region Put /users/{email}

        [Test]
        public async Task Given_UserInfoChanged_When_ValidUser_Update()
        {
            //Arrange user
            var userEmail = await GetValidEmail();

            //Act
            var infoChangedUser = new User("Jane Doe", userEmail, 25);
            var updatedUser = await UpdateUserAsync(infoChangedUser);

            //Assert
            Assert.That(updatedUser, Is.Not.Null, "Api failed to update user");
            Assert.That(updatedUser.Email, Is.EqualTo(infoChangedUser.Email), "Updated user email doesn't match");
            Assert.That(updatedUser.Name, Is.EqualTo(infoChangedUser.Name), "Updated user name doesn't match");
            Assert.That(updatedUser.Age, Is.EqualTo(infoChangedUser.Age), "Updated user age doesn't match");

        }

        [Test]
        public async Task Given_UserInfoChanged_When_UserIsInvalid_Error400()
        {
            var userToUpdate = new User("John Doe", string.Empty, 21);

            var exception = Assert.ThrowsAsync<Exception>(async () => await UpdateUserAsync(userToUpdate));

            Assert.That(exception.Message, Is.EqualTo("400"), "Api is not producing 400 error when data is invalid");
        }

        [Test]
        public async Task Given_UserInfoChanged_When_UserAgeIsOver150_Error400()
        {
            var userToUpdate = new User("John Doe", GetMockEmail(), 151);

            var exception = Assert.ThrowsAsync<Exception>(async () => await UpdateUserAsync(userToUpdate));

            Assert.That(exception.Message, Is.EqualTo("400"), "Api is not producing 400 error when age is over 150");
        }


        [Test]
        public async Task Given_UserInfoChanged_When_EmailIsInvalid_Error404()
        {
            var userToUpdate = new User("John Doe", GetMockEmail(), 21);

            var exception = Assert.ThrowsAsync<Exception>(async () => await UpdateUserAsync(userToUpdate));

            Assert.That(exception.Message, Is.EqualTo("404"), "Api is not producing 404 when email doesn't match");
        }


        [Test]
        public async Task Given_UserInfoChanged_When_EmailIsDuplicated_Error409()
        {
            var emailToDuplicate = await GetValidEmail();
            var emailToUpdate = await GetValidEmail();
            

            var userToUpdate = new User("John Doe", emailToDuplicate, 21);

            var exception = Assert.ThrowsAsync<Exception>(async () => await UpdateUserAsync(userToUpdate, emailToUpdate));

            Assert.That(exception.Message, Is.EqualTo("409"), "Api is not producing 409 when updated email is duplicated");
        }

        #endregion


        #region Delete /users/{email}
        [Test]
        public async Task Given_UserToBeRemoved_When_ValidUser_Delete()
        {
            //Arrange user
            var userEmail = await GetValidEmail();

            //Act
            var isDeleted = await DeleteUserAsync(userEmail, _token);

            //Assert
            Assert.That(isDeleted, Is.True, "Api failed to delete user");

        }

        [Test]
        public async Task Given_UserToBeRemoved_When_TokenInvalid_Error401()
        {
            //Arrange user
            var userEmail = await GetValidEmail();

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await DeleteUserAsync(userEmail, "notSoSecretToken"), "Api is not throwing exception when token is invalid");

            //Assert
            Assert.That(exception.Message, Is.EqualTo("401"), "Api is not producing 401 when token is invalid");
        }

        [Test]
        public async Task Given_UserToBeRemoved_When_InvalidUser_Error404()
        {
            //Arrange user
            var userEmail = GetMockEmail();

            //Act
            var exception = Assert.ThrowsAsync<Exception>(async () => await DeleteUserAsync(userEmail, _token), "Api is not throwing exception when email is invalid");

            //Assert
            Assert.That(exception.Message, Is.EqualTo("404"), "Api is not producing 404 when user is invalid");

        }

        #endregion


        #region Integration

        //Integration
        // Create User
        // --> Update User
        // --> Get user and verify data has been updated
        // Create User
        // --> Delete User
        // --> Get user and verify is not there anymore

        [Test]
        public async Task Given_CreateUpdateAndVerify_When_IntegrationWorking_Pass()
        {
            //Create user
            var newUser = new User("John Doe", GetMockEmail(), 21);
            var user = await CreateUserAsync(newUser);

            Assert.That(user, Is.Not.Null, "Api failed to create new user");
            Assert.That(user.Name, Is.EqualTo(newUser.Name), "Name of the user sent for creation and user created do not match");
            Assert.That(user.Email, Is.EqualTo(newUser.Email), "Email of the user sent for creation and user created do not match");
            Assert.That(user.Age, Is.EqualTo(newUser.Age), "Age of the user sent for creation and user created do not match");

            //Update User
            user.Age = 50;
            user.Name = "Jhonny";
            var updatedUser = await UpdateUserAsync(user);

            Assert.That(updatedUser, Is.Not.Null, "Api failed to update user");
            Assert.That(updatedUser.Name, Is.EqualTo(user.Name), "Name of the user send for update and user updated doesn't match");
            Assert.That(updatedUser.Age, Is.EqualTo(user.Age), "Age of the user send for update and user updated doesn't match");

            //Fetch the user and verify data on the backend has the last state
            var existingUser = await GetUser(user.Email);

            //Assert
            Assert.That(existingUser, Is.Not.Null, "Api failed to provide user");
            Assert.That(existingUser.Name, Is.EqualTo(user.Name), "Name did not get updated on the backend with the update operation");
            Assert.That(existingUser.Age, Is.EqualTo(user.Age), "Age did not get updated on the backend with the update operation");

        }

        [Test]
        public async Task Given_CreateDeleteAndVerify_When_IntegrationWorking_Pass()
        {
            //Create user
            var newUser = new User("John Doe", GetMockEmail(), 21);
            var user = await CreateUserAsync(newUser);

            Assert.That(user, Is.Not.Null, "Api failed to create new user");
            Assert.That(user.Name, Is.EqualTo(newUser.Name), "Name of the user sent for creation and user created do not match");
            Assert.That(user.Email, Is.EqualTo(newUser.Email), "Email of the user sent for creation and user created do not match");
            Assert.That(user.Age, Is.EqualTo(newUser.Age), "Age of the user sent for creation and user created do not match");

            //Delete User
            var isDeleted = await DeleteUserAsync(user.Email, _token);

            Assert.That(isDeleted, Is.True, "Api failed to update user");

            //Fetch the user and verify data on the backend has the last state
            //This should return a 404
            var exception = Assert.ThrowsAsync<Exception>(async () => await GetUser(user.Email));
            Assert.That(exception.Message, Is.EqualTo("404"), "User was not deleted on the backend with the delete operation");            
        }


        #endregion


        #region Private Async Methods

        private async Task<User> GetUser(string email)
        {
            try
            {
                using var client = new HttpClient { BaseAddress = new Uri(_baseAddress) };
                var existingUser = await client.GetFromJsonAsync<User>($"users/{email}");

                if (existingUser == null)
                {
                    throw new Exception("404");
                }

                return existingUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        private async Task<User?> CreateUserAsync(User user)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_baseAddress) };

            var response = await client.PostAsJsonAsync("users", user);

            //Accepted responses 201, 400 and 409

            switch (response.StatusCode)
            {
                case HttpStatusCode.Created: return await response.Content.ReadFromJsonAsync<User>();

                case HttpStatusCode.BadRequest: throw new Exception(((int)response.StatusCode).ToString());

                case HttpStatusCode.Conflict: throw new Exception(((int)response.StatusCode).ToString());

                default: throw new Exception(((int)response.StatusCode).ToString());
            }

        }

        private async Task<User?> UpdateUserAsync(User user, string existingEmailAccount = "")
        {
            using var client = new HttpClient { BaseAddress = new Uri(_baseAddress) };

            var email = string.IsNullOrEmpty(existingEmailAccount) ? user.Email : existingEmailAccount;
            var response = await client.PutAsJsonAsync($"users/{email}", user);

            //Accepted responses 200, 400, 404 and 409

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK: return await response.Content.ReadFromJsonAsync<User>();

                case HttpStatusCode.BadRequest: throw new Exception(((int)response.StatusCode).ToString());

                case HttpStatusCode.NotFound: throw new Exception(((int)response.StatusCode).ToString());

                case HttpStatusCode.Conflict: throw new Exception(((int)response.StatusCode).ToString());

                default: throw new Exception(((int)response.StatusCode).ToString());
            }

        }

        private async Task<bool> DeleteUserAsync(string email, string token)
        {
            using var client = new HttpClient { BaseAddress = new Uri(_baseAddress) };
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token);

            var response = await client.DeleteAsync($"users/{email}");

            //Accepted responses 204, 401, 404
            switch (response.StatusCode)
            {
                case HttpStatusCode.NoContent: return true;

                case HttpStatusCode.Unauthorized: throw new Exception(((int)response.StatusCode).ToString());

                case HttpStatusCode.NotFound: throw new Exception(((int)response.StatusCode).ToString());

                default: throw new Exception(((int)response.StatusCode).ToString());
            }
        }

        private async Task<string> GetValidEmail()
        {
            //Arrange user
            var userEmail = GetMockEmail();
            var user = await CreateUserAsync(new User("John Doe", userEmail, 21));
            if (user == null)
                throw new Exception("Could not create valid user");

            return userEmail;
        }


        private string GetMockEmail()
        {
            return  $"John{DateTime.Now.TimeOfDay.TotalNanoseconds}@mail.com";
        }

        #endregion
    }
}
