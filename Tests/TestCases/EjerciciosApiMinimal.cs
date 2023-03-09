using API.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using Tests.Helpers;
using Xunit;

namespace Tests.TestCases
{
    public class EjerciciosApiMinimal : BaseTest
    {
        private const string adminsEndpoint = "/admins";
        private const string usersEndpoint = "/users";
        private const string credentials = "admin:8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92";
        private const string deleteAllUsersEndpoint = "/users/deleteAll";
        public HttpStatusCode statusCode;

        #region Post
        [Theory] //POST - 1
        [InlineData("Test_Admin_13", "1234")]
        public void Post_CreateNewAdmin_VerifyIfItIsOnTheList(string username, string passHash)
        {
            bool flag = false;
            //Post Admin Creation
            var payload = new { username, passHash };
            var adminToBeCreated = client.Post<Admin>(adminsEndpoint, payload,
                headers: Data.GetAuthorizationHeader(credentials));

            Assert.Equal(adminToBeCreated.UserName, username);
            Assert.Equal(adminToBeCreated.PassHash.Length, 64);
            int adminToBeCreatedId = adminToBeCreated.Id;


            //Get admin ID created above
            var adminCreated = client.Get<Admin>($"{adminsEndpoint}/{adminToBeCreatedId}",
                headers: Data.GetAuthorizationHeader(credentials));
            if (adminCreated.Id.Equals(adminToBeCreatedId))
            {
                flag = true;
            }
            Assert.True(flag);
        }

        [Theory] //POST -2
        [InlineData(99999, "Password")]
        public void Post_CreateAdmin_InvalidPayload(int username, string password)
        {
            var payload = new { username, password };

            var response = client.Post(adminsEndpoint, payload,
           headers: Data.GetAuthorizationHeader(credentials));

            var code = (int)response.StatusCode;

            Assert.Equal(code, 400);

        }
        #endregion

        #region GET
        [Theory] //GET - 1
        [InlineData(1, "admin", "49dc52e6bf2abe5ef6e2bb5b0f1ee2d765b922ae6cc8b95d39dc06c21c848f8c")]
        public void Get_AdminByID_ApiResponseCode200AndApiResponseTimeLowerThan1(int id, string username, string passHash)
        {
            //get admin by id, and count time to get it
            var sw = Stopwatch.StartNew();
            
            var response = client.Get($"{adminsEndpoint}/{id}",
                headers: Data.GetAuthorizationHeader(credentials));
            
            var time = TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds);

            Assert.True(time.Seconds <= 1);
       
            //Esto lo dijo Bruno:
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory]// GET - 2
        [InlineData(1)]
        public void Get_UserById_UserDataIsPresentJsonSchema(int id)
        {
            
            var user = client.Get<User>($"{usersEndpoint}/{id}",
                headers: Data.GetAuthorizationHeader(credentials));
  
            //Assert user data is not empty 
            Assert.NotNull(user.Id);
            Assert.NotNull(user.Name);
            Assert.NotNull(user.LastName);
            Assert.NotNull(user.PhoneNumber);
            Assert.NotNull(user.Email);

            //Assert json schema
            var response = client.Get($"{usersEndpoint}/{id}",
                 headers: Data.GetAuthorizationHeader(credentials));

            Assert.Equal(response.ContentType, "application/json");
        }

        #endregion

        #region Put
        [Theory]
        [InlineData(1, 2147483648)]
        public void Put_UpdateUserOutOfBoundPhoneNumber(int id, long phonenumber)
        {
            var payload = new { phonenumber };

            var response = client.Put($"{usersEndpoint}/{id}", payload,
           headers: Data.GetAuthorizationHeader(credentials));


            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


        }

        [Theory] //PUT - 2
        [InlineData(1, "UpdatedName","UpdateLastName",099787777,"update@gmail.com")]
        public void Put_UpdateUserAndVerify(int id,string name, string lastname, long phonenumber, string email)
        {
            //Update user
            var payload = new { name, lastname,phonenumber,email};

            var response = client.Put($"{usersEndpoint}/{id}", payload,
           headers: Data.GetAuthorizationHeader(credentials));


            //Verify if updated
            var user = client.Get<User>($"{usersEndpoint}/{id}",
              headers: Data.GetAuthorizationHeader(credentials));

            Assert.Equal(user.Name, name);
            Assert.Equal(user.LastName, lastname);
            Assert.Equal(user.Email, email);
            Assert.Equal((long)user.PhoneNumber, phonenumber);
        }
        #endregion


        #region Delete
        [Theory] //DELETE - 1 
        [InlineData(1,15)]
        public void Delete_DeleteUserAndValidateResponseCode(int notExistingId,int existingId)
        {
            //Verify error if user does not exist
            var notExistingUserToBeDeleted = client.Delete($"{usersEndpoint}/delete/{notExistingId}",
                headers: Data.GetAuthorizationHeader(credentials));

            Assert.Equal(HttpStatusCode.NoContent, notExistingUserToBeDeleted.StatusCode);

            //Verify error with a user that exists
            var existingUserToBeDeleted = client.Delete($"{usersEndpoint}/delete/{existingId}",
               headers: Data.GetAuthorizationHeader(credentials));

            Assert.Equal(HttpStatusCode.NoContent, notExistingUserToBeDeleted.StatusCode);
        }

        [Theory] //DELETE - 2
        [InlineData("Juan","Perez",099998877,"juanperez@gmail.com")]
        public void Delete_CreateUserDeleteItAndVerify(string name, string lastName, int phoneNumber, string email)
        {
            //Create user to be deleted
            var payload = new { name, lastName, phoneNumber, email };
            var userToBeCreated = client.Post<User>(usersEndpoint, payload,
                headers: Data.GetAuthorizationHeader(credentials));

            //Verify User is created
            Assert.Equal(name, userToBeCreated.Name);
            Assert.Equal(lastName, userToBeCreated.LastName);
            Assert.Equal(phoneNumber, userToBeCreated.PhoneNumber);
            Assert.Equal(email, userToBeCreated.Email);

            //Delete user with id created above and verify its deleted
            var userToBeDeleted = client.Delete($"{usersEndpoint}/delete/{userToBeCreated.Id}",
                headers: Data.GetAuthorizationHeader(credentials));

            Assert.Equal(HttpStatusCode.NoContent, userToBeDeleted.StatusCode);

            //Verify if user  was deleted correctly
            var response = client.Get($"{usersEndpoint}/{userToBeCreated.Id}",
                      headers: Data.GetAuthorizationHeader(credentials));

            Assert.Equal(0, response.ContentLength);

        }

        [Fact] //DELETE - 3
        public void Delete_DeleteAll()
        {
            var deleteAllUsers = client.Delete($"{deleteAllUsersEndpoint}",
                headers: Data.GetAuthorizationHeader(credentials));
 
        }

        [Fact] //DELETE - 4
        public void Delete_VerifyIfUsersExists()
        {
            var listaDeUsuarios = client.Get<List<User>>($"{usersEndpoint}",
                headers: Data.GetAuthorizationHeader(credentials));
            
            Assert.False(listaDeUsuarios.Any());
        }
        #endregion



        // PARA TODOS LOS EJERCICIOS

        // Authorization: Basic YWRtaW46OGQ5NjllZWY2ZWNhZDNjMjlhM2E2MjkyODBlNjg2Y2YwYzNmNWQ1YTg2YWZmM2NhMTIwMjBjOTIzYWRjNmM5Mg==
        // http://localhost:9999/admins
        // http://localhost:9999/users

        /**
         * Para todos los casos, incluir pruebas sin autenticación para validar que los
         * métodos no son accesibles sin los permisos adecuados. Asimismo, incluir pruebas
         * negativas para validar los tipos de datos enviados en cada payload (cuando sea posible).
         **/

        // GET

        /**
         * EJ 1
         * Validar que al obtener los datos del user admin el código de respuesta sea 200.
         *
         **/

        /**
         * EJ 2
         * Validar que al obtener los datos de un usuario,
         * la respuesta contenga los datos de todos los usuarios (id, name, lastName, phoneNumber, email).
         * Como adicional, validar que la estructura de la respuesta sea correcta (JSON schema).
         *
         **/

        // POST

        /**
         * EJ 3
         * Crear un nuevo user. Validar que los datos de usuario enviados correspondan
         * con los del body de la respuesta. Posteriormente con el ID obtenido,
         * validar mediante un GET que el registro creado anteriormente haya sido
         * persistido correctamente y ubicado al final de la lista de usuarios.
         **/

        /**
         * EJ 4
         * Validar que enviando un payload malformado o algún tipo de dato incorrecto o fuera de rango,
         * el servicio responde con un error 400 Bad Request
         **/

        // PUT

        /**
         * EJ 5
         * Actualizar el telefono de un usuario con un número mayor al permitido (mayor a 2147483647).
         * Validar que el servicio responde con un mensaje de error.
         **/

        /**
         * EJ 6
         * Actualizar todos los datos de un usuario. Validar que los datos del usuario enviado
         * correspondan con los del body de la respuesta. Adicionalmente, validar mediante un
         * GET que los datos actualizados anteriormente hayan sido persistidos correctamente.
         **/

        // DELETE

        /**
         * EJ 7
         * Validar que al intentar borrar un registro inexistente el servicio responde con un mensaje de error.
         * Luego borrar un registro existente y validar que el response code es 204 - No Content.
         **/

        /**
         * EJ 8
         * Validar mediante un GET que el registro borrado anteriormente haya sido eliminado correctamente.
         **/

        /**
         * EJ 9
         * Mediante el uso del endpoint deleteAll, eliminar todos los usuarios persistidos.
         **/

        /**
         * EJ 10
         * Validar mediante un GET que todos los regitros de usuarios han sido eliminados de la persistencia.
         **/
    }
}