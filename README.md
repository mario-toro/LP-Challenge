# LoanPro SDET Challenge

# API Bugs Report ❌ 
### Dev Environment failures

<p>
<strong>Total Tests:</strong> 17 <br />
<strong>Failure Count:</strong> 6 <br />
</p>

<details>
<summary>❌ Failed (6)</summary>
<table>
<thead>
<tr>
<th>Test</th>
<th>Duration</th>
</tr>
</thead>
<tbody>
<tr>
<td>
<details>
<summary>
❌ Given_UserToBeRemoved_When_TokenInvalid_Error401
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_UserToBeRemoved_When_TokenInvalid_Error401</blockquote>
Message:
<blockquote>  Api is not throwing exception when token is invalid
Assert.That(caughtException, expression)
  Expected: <System.Exception>
  But was:  null
</blockquote>
Description:
<blockquote>
  Authoritative source specifies that when the token is not valid it should throw an http 401 and it is not doing it.
<blockquote>
</details>
</td>
<td>16ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_CreateUpdateAndVerify_When_IntegrationWorking_Pass
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_CreateUpdateAndVerify_When_IntegrationWorking_Pass</blockquote>
Message:
<blockquote>  Name did not get updated on the backend with the update operation
Assert.That(existingUser.Name, Is.EqualTo(user.Name))
  Expected string length 6 but was 8. Strings differ at index 1.
  Expected: "Jhonny"
  But was:  "John Doe"
  ------------^
</blockquote>
Description:
<blockquote>
  This test tries to verify the combination of different operations, on this case it tries to do 3 things
  <ol>
  <li>Create a new User </li>
  <li>Update the information for the user created on the previous step.</li>
  <li>Query the api for the user and the expectation is to see the user with the updated information, not the original one.</li>
 </ol>
 For this case the error is that apparently the update operation works, but when you query the api, the user comes back with the original values, when it should have the updated ones.
<blockquote>
</details>
</td>
<td>21ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_UserInfoChanged_When_UserIsInvalid_Error400
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_UserInfoChanged_When_UserIsInvalid_Error400</blockquote>
Message:
<blockquote>  Api is not producing 400 error when data is invalid
Assert.That(exception.Message, Is.EqualTo("400"))
  String lengths are both 3. Strings differ at index 0.
  Expected: "400"
  But was:  "500"
  -----------^
</blockquote>
Description:
<blockquote>
  Authoritative Source specifies that when updating a user with invalid information, such as an empty email (which is the case) then should throw an http 400 Error, and it is failing, but not with a 400.
<blockquote>
</details>
</td>
<td>6ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_UserIsRequested_When_InvalidEmail_Error404
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_UserIsRequested_When_InvalidEmail_Error404</blockquote>
Message:
<blockquote>  Api is not producing 404 error when email doesn't match
Assert.That(exception.Message, Is.EqualTo("404"))
  Expected string length 3 but was 76. Strings differ at index 0.
  Expected: "404"
  But was:  "Response status code does not indicate success: 500 (INTERNAL..."
  -----------^
</blockquote>
Description:
<blockquote>
  Authoritative Source specifies that when an invalid email its used to fetch a user, api should respond with an http 404, but if's failing to do so.
<blockquote>
</details>
</td>
<td>4ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_NewUser_When_UserEmailIsDuplicated_Then_Error409
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_NewUser_When_UserEmailIsDuplicated_Then_Error409</blockquote>
Message:
<blockquote>  Api is not producing 409 error when email is duplicated
Assert.That(exception.Message, Is.EqualTo("409"))
  String lengths are both 3. Strings differ at index 0.
  Expected: "409"
  But was:  "500"
  -----------^
</blockquote>
Description:
<blockquote>
  Authoritative Source specifies that when a new user is to be created, if the email for the user already exists it should throw an http 409, and it's failing but with another code, so it's failing to provide the right error response.
<blockquote>
</details>
</td>
<td>8ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_CreateDeleteAndVerify_When_IntegrationWorking_Pass
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_CreateDeleteAndVerify_When_IntegrationWorking_Pass</blockquote>
Message:
<blockquote>  User was not deleted on the backend with the delete operation
Assert.That(exception.Message, Is.EqualTo("404"))
  Expected string length 3 but was 76. Strings differ at index 0.
  Expected: "404"
  But was:  "Response status code does not indicate success: 500 (INTERNAL..."
  -----------^
</blockquote>
Description:
<blockquote>
  This integration tries to verify the combination of different operations, on this case it tries to do 3 things
  <ol>
  <li>Create a new User</li>
  <li>Delete the user created on step 1</li>
  <li>Query the api for the user and confirm the user is not there which means an http 404 response.</li>
 </ol>
 Under this scenario, the create and delete operations are working, but when we query the api instead of the http 404 response, we get a 500 error.
<blockquote>
</details>
</td>
<td>120ms</td>
</tr>
</tbody>
</table>
</details>

---
### Prod Environment failures

<p>
<strong>Total Tests:</strong> 17 <br />
<strong>Failure Count:</strong> 7 <br />
</p>

<details>
<summary>❌ Failed (7)</summary>
<table>
<thead>
<tr>
<th>Test</th>
<th>Duration</th>
</tr>
</thead>
<tbody>
<tr>
<td>
<details>
<summary>
❌ Given_CreateUpdateAndVerify_When_IntegrationWorking_Pass
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_CreateUpdateAndVerify_When_IntegrationWorking_Pass</blockquote>
Message:
<blockquote>  Name did not get updated on the backend with the update operation
Assert.That(existingUser.Name, Is.EqualTo(user.Name))
  Expected string length 6 but was 8. Strings differ at index 1.
  Expected: "Jhonny"
  But was:  "John Doe"
  ------------^
</blockquote>
Description:
<blockquote>
  This test tries to verify the combination of different operations, on this case it tries to do 3 things
  <ol>
  <li>Create a new User </li>
  <li>Update the information for the user created on the previous step.</li>
  <li>Query the api for the user and the expectation is to see the user with the updated information, not the original one.</li>
 </ol>
 For this case the error is that apparently the update operation works, but when you query the api, the user comes back with the original values, when it should have the updated ones.
<blockquote>
</details>
</td>
<td>43ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_UserIsRequested_When_InvalidEmail_Error404
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_UserIsRequested_When_InvalidEmail_Error404</blockquote>
Message:
<blockquote>  Api is not producing 404 error when email doesn't match
Assert.That(exception.Message, Is.EqualTo("404"))
  Expected string length 3 but was 76. Strings differ at index 0.
  Expected: "404"
  But was:  "Response status code does not indicate success: 500 (INTERNAL..."
  -----------^
</blockquote>
Description:
<blockquote>
  Authoritative Source specifies that when an invalid email its used to fetch a user, api should respond with an http 404, but if's failing to do so.
<blockquote>
</details>
</td>
<td>8ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_UserInfoChanged_When_UserIsInvalid_Error400
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_UserInfoChanged_When_UserIsInvalid_Error400</blockquote>
Message:
<blockquote>  Api is not producing 400 error when data is invalid
Assert.That(exception.Message, Is.EqualTo("400"))
  String lengths are both 3. Strings differ at index 0.
  Expected: "400"
  But was:  "500"
  -----------^
</blockquote>
Description:
<blockquote>
  Authoritative Source specifies that when updating a user with invalid information, such as an empty email (which is the case) then should throw an http 400 Error, and it is failing, but not with a 400.
<blockquote>
</details>
</td>
<td>5ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_UserToBeRemoved_When_ValidUser_Delete
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_UserToBeRemoved_When_ValidUser_Delete</blockquote>
Message:
<blockquote>System.Exception : 401</blockquote>
Description:
<blockquote>
  This is the happy path scenario, an existing user's email is provided along with the right token (according to the authoritative source), and based on it, user should be removed, but instead it failed with an http 401.
<blockquote>
</details>
</td>
<td>9ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_NewUser_When_UserEmailIsDuplicated_Then_Error409
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_NewUser_When_UserEmailIsDuplicated_Then_Error409</blockquote>
Message:
<blockquote>  Api is not producing 409 error when email is duplicated
Assert.That(exception.Message, Is.EqualTo("409"))
  String lengths are both 3. Strings differ at index 0.
  Expected: "409"
  But was:  "500"
  -----------^
</blockquote>
Description:
<blockquote>
  Authoritative Source specifies that when a new user is to be created, if the email for the user already exists it should throw an http 409, and it's failing but with another code, so it's failing to provide the right error response.
<blockquote>
</details>
</td>
<td>9ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_CreateDeleteAndVerify_When_IntegrationWorking_Pass
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_CreateDeleteAndVerify_When_IntegrationWorking_Pass</blockquote>
Message:
<blockquote>System.Exception : 401</blockquote>
Description:
<blockquote>
  This integration tries to verify the combination of different operations, on this case it tries to do 3 things
  <ol>
  <li>Create a new User</li>
  <li>Delete the user created on step 1</li>
  <li>Query the api for the user and confirm the user is not there which means an http 404 response.</li>
 </ol>
 Under this scenario, the create operation works, but the delete fails with a 401, according to the authoritative source the token is the same for both environment, but it could be that in reality it is not, and that could be the reason for having this test pass on dev but fail on prod.
<blockquote>
</details>
</td>
<td>97ms</td>
</tr>
<tr>
<td>
<details>
<summary>
❌ Given_UserToBeRemoved_When_InvalidUser_Error404
</summary>
Source:
<blockquote>LP_UserManagementTests.UserManagementApiTests.Given_UserToBeRemoved_When_InvalidUser_Error404</blockquote>
Message:
<blockquote>  Api is not producing 404 when user is invalid
Assert.That(exception.Message, Is.EqualTo("404"))
  String lengths are both 3. Strings differ at index 2.
  Expected: "404"
  But was:  "401"
  -------------^
</blockquote>
Description:
<blockquote>
This test is trying to get an http 404 as specified by the authoritative source when trying to delete a user with an email account that doesn't exists and the provided token, the operation it's failing I presume because of the incorrect token being use on prod.
<blockquote>
</details>
</td>
<td>6ms</td>
</tr>
</tbody>
</table>

---